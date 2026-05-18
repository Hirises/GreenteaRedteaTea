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
	return _settings.get("Names", {})

func _colors() -> Dictionary:
	return _settings.get("Colors", {})

func _mixing() -> Dictionary:
	return _settings.get("ColorMixing", {})

func tea_suffix() -> String:
	return _names().get("Suffix", {}).get("Tea", "")

func brewed_leaf_suffix() -> String:
	return _names().get("Suffix", {}).get("BrewedLeaf", "")

func combined_leaf_suffix() -> String:
	return _names().get("Suffix", {}).get("CombinedLeaf", "")

func mixed_liquid_suffix() -> String:
	return _names().get("Suffix", {}).get("MixedLiquid", "")

func get_base_name(kind: int) -> String:
	var base: Dictionary = _names().get("Base", {})
	match kind:
		BaseKind.TEA:
			return base.get("Tea", "")
		BaseKind.MILK_TEA:
			return base.get("MilkTea", "")
	return ""

func get_basic_leaf_name(kind: int) -> String:
	var leaf: Dictionary = _names().get("Leaf", {})
	match kind:
		BasicLeafKind.GREEN:
			return leaf.get("Green", "")
		BasicLeafKind.BLACK:
			return leaf.get("Black", "")
	return ""

func get_base_color(kind: int) -> ProductColor:
	var base: Dictionary = _colors().get("Base", {})
	match kind:
		BaseKind.TEA:
			return _to_product_color(base.get("Tea", {}))
		BaseKind.MILK_TEA:
			return _to_product_color(base.get("MilkTea", {}))
	return ProductColor.new()

func get_basic_leaf_color(kind: int) -> ProductColor:
	var leaf: Dictionary = _colors().get("Leaf", {})
	match kind:
		BasicLeafKind.GREEN:
			return _to_product_color(leaf.get("Green", {}))
		BasicLeafKind.BLACK:
			return _to_product_color(leaf.get("Black", {}))
	return ProductColor.new()

func wrap_depth_increased_name(name: String) -> String:
	var brackets: Dictionary = _names().get("Brackets", {})
	return "%s%s%s" % [brackets.get("Open", ""), name, brackets.get("Close", "")]

func calculate_combined_leaf_color(left: ProductColor, right: ProductColor) -> ProductColor:
	var weights: Dictionary = _mixing().get("CombineLeaves", {})
	return _weighted_rgb(left, float(weights.get("Left", 1.0)), right, float(weights.get("Right", 1.0))).with_alpha(float(weights.get("ResultAlpha", 1.0)))

func calculate_brewed_tea_color(leaf: ProductColor, liquid: ProductColor) -> ProductColor:
	var weights: Dictionary = _mixing().get("BrewTea", {}).get("Tea", {})
	var rgb: ProductColor = _weighted_rgb(liquid, float(weights.get("Liquid", 1.0)), leaf, float(weights.get("Leaf", 1.0)))
	return rgb.with_alpha(minf(1.0, liquid.a + float(weights.get("AlphaAdd", 0.0))))

func calculate_brewed_leaf_color(leaf: ProductColor, liquid: ProductColor) -> ProductColor:
	var tea: ProductColor = calculate_brewed_tea_color(leaf, liquid)
	var weights: Dictionary = _mixing().get("BrewTea", {}).get("SteepedLeaf", {})
	return _weighted_rgb(leaf, float(weights.get("OriginalLeaf", 1.0)), tea, float(weights.get("BrewedTea", 1.0))).with_alpha(float(weights.get("ResultAlpha", 1.0)))

func calculate_mixed_liquid_color(left: ProductColor, right: ProductColor) -> ProductColor:
	var total_alpha := left.a + right.a
	var left_weight := left.a / total_alpha * 2.0 if total_alpha > 0.0 else 1.0
	var right_weight := right.a / total_alpha * 2.0 if total_alpha > 0.0 else 1.0
	var alpha := total_alpha / 2.0 * float(_mixing().get("MixLiquids", {}).get("AlphaMultiplier", 1.0))
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
	return ProductColor.from_rgb255(float(value.get("R", 0.0)), float(value.get("G", 0.0)), float(value.get("B", 0.0)), float(value.get("A", 1.0)))

