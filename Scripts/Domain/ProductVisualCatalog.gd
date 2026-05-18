class_name ProductVisualCatalog

const DEFAULT_JSON_PATH := "res://Scripts/Domain/ProductVisualSettings.json"
static var _current: ProductVisualCatalog

var _settings: Dictionary

static func current() -> ProductVisualCatalog:
	if _current == null:
		_current = load_from_file(DEFAULT_JSON_PATH)
	return _current

static func configure(catalog: ProductVisualCatalog) -> void:
	_current = catalog

static func configure_from_file(path: String) -> void:
	_current = load_from_file(path)

static func load_from_file(path: String) -> ProductVisualCatalog:
	if path.strip_edges().is_empty():
		push_error("Path cannot be empty.")
	var json := FileAccess.get_file_as_string(path)
	var parsed = JSON.parse_string(json)
	if typeof(parsed) != TYPE_DICTIONARY:
		push_error("Failed to read product visual settings from %s." % path)
		parsed = {}
	return ProductVisualCatalog.new(parsed)

func _init(settings: Dictionary = {}) -> void:
	_settings = settings

func _names() -> Dictionary:
	return _dict_get(_settings, "Names", {})

func _colors() -> Dictionary:
	return _dict_get(_settings, "Colors", {})

func _mixing() -> Dictionary:
	return _dict_get(_settings, "ColorMixing", {})

func tea_suffix() -> String:
	return _dict_get(_dict_get(_names(), "Suffix", {}), "Tea", "")

func brewed_leaf_suffix() -> String:
	return _dict_get(_dict_get(_names(), "Suffix", {}), "BrewedLeaf", "")

func combined_leaf_suffix() -> String:
	return _dict_get(_dict_get(_names(), "Suffix", {}), "CombinedLeaf", "")

func mixed_liquid_suffix() -> String:
	return _dict_get(_dict_get(_names(), "Suffix", {}), "MixedLiquid", "")

func get_base_name(kind: int) -> String:
	var base: Dictionary = _dict_get(_names(), "Base", {})
	match kind:
		BaseKind.TEA:
			return _dict_get(base, "Tea", "")
		BaseKind.MILK_TEA:
			return _dict_get(base, "MilkTea", "")
	return ""

func get_basic_leaf_name(kind: int) -> String:
	var leaf: Dictionary = _dict_get(_names(), "Leaf", {})
	match kind:
		BasicLeafKind.GREEN:
			return _dict_get(leaf, "Green", "")
		BasicLeafKind.BLACK:
			return _dict_get(leaf, "Black", "")
	return ""

func get_base_color(kind: int) -> ProductColor:
	var base: Dictionary = _dict_get(_colors(), "Base", {})
	match kind:
		BaseKind.TEA:
			return _to_product_color(_dict_get(base, "Tea", {}))
		BaseKind.MILK_TEA:
			return _to_product_color(_dict_get(base, "MilkTea", {}))
	return ProductColor.new()

func get_basic_leaf_color(kind: int) -> ProductColor:
	var leaf: Dictionary = _dict_get(_colors(), "Leaf", {})
	match kind:
		BasicLeafKind.GREEN:
			return _to_product_color(_dict_get(leaf, "Green", {}))
		BasicLeafKind.BLACK:
			return _to_product_color(_dict_get(leaf, "Black", {}))
	return ProductColor.new()

func wrap_depth_increased_name(name: String) -> String:
	var brackets: Dictionary = _dict_get(_names(), "Brackets", {})
	return "%s%s%s" % [_dict_get(brackets, "Open", ""), name, _dict_get(brackets, "Close", "")]

func calculate_combined_leaf_color(left: ProductColor, right: ProductColor) -> ProductColor:
	var weights: Dictionary = _dict_get(_mixing(), "CombineLeaves", {})
	return _weighted_rgb(left, float(_dict_get(weights, "Left", 1.0)), right, float(_dict_get(weights, "Right", 1.0))).with_alpha(float(_dict_get(weights, "ResultAlpha", 1.0)))

func calculate_brewed_tea_color(leaf: ProductColor, liquid: ProductColor) -> ProductColor:
	var brew_tea: Dictionary = _dict_get(_mixing(), "BrewTea", {})
	var weights: Dictionary = _dict_get(brew_tea, "Tea", {})
	var rgb: ProductColor = _weighted_rgb(liquid, float(_dict_get(weights, "Liquid", 1.0)), leaf, float(_dict_get(weights, "Leaf", 1.0)))
	return rgb.with_alpha(minf(1.0, liquid.a + float(_dict_get(weights, "AlphaAdd", 0.0))))

func calculate_brewed_leaf_color(leaf: ProductColor, liquid: ProductColor) -> ProductColor:
	var tea: ProductColor = calculate_brewed_tea_color(leaf, liquid)
	var brew_tea: Dictionary = _dict_get(_mixing(), "BrewTea", {})
	var weights: Dictionary = _dict_get(brew_tea, "SteepedLeaf", {})
	return _weighted_rgb(leaf, float(_dict_get(weights, "OriginalLeaf", 1.0)), tea, float(_dict_get(weights, "BrewedTea", 1.0))).with_alpha(float(_dict_get(weights, "ResultAlpha", 1.0)))

func calculate_mixed_liquid_color(left: ProductColor, right: ProductColor) -> ProductColor:
	var total_alpha := left.a + right.a
	var left_weight := left.a / total_alpha * 2.0 if total_alpha > 0.0 else 1.0
	var right_weight := right.a / total_alpha * 2.0 if total_alpha > 0.0 else 1.0
	var mix_liquids: Dictionary = _dict_get(_mixing(), "MixLiquids", {})
	var alpha := total_alpha / 2.0 * float(_dict_get(mix_liquids, "AlphaMultiplier", 1.0))
	return _weighted_rgb(left, left_weight, right, right_weight).with_alpha(alpha)

static func _weighted_rgb(left: ProductColor, left_weight: float, right: ProductColor, right_weight: float) -> ProductColor:
	left_weight /= 2.0
	right_weight /= 2.0
	return ProductColor.new(
		left.r * left_weight + right.r * right_weight,
		left.g * left_weight + right.g * right_weight,
		left.b * left_weight + right.b * right_weight,
		left.a * left_weight + right.a * right_weight).clamped()

static func _to_product_color(value: Dictionary) -> ProductColor:
	return ProductColor.from_rgb255(float(_dict_get(value, "R", 0.0)), float(_dict_get(value, "G", 0.0)), float(_dict_get(value, "B", 0.0)), float(_dict_get(value, "A", 1.0)))

static func _dict_get(source: Dictionary, key: String, fallback = null):
	if source.has(key):
		return source[key]
	var lower_key := key.to_lower()
	for existing_key in source.keys():
		if str(existing_key).to_lower() == lower_key:
			return source[existing_key]
	return fallback
