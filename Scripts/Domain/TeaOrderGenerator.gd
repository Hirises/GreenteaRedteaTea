class_name TeaOrderGenerator

var _random := RandomNumberGenerator.new()

func _init(seed = null) -> void:
	if seed == null:
		_random.randomize()
	else:
		_random.seed = int(seed)

func generate_product(max_depth: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	var choices: Array[Callable] = [generate_base.bind(rules), generate_leaf.bind(rules), generate_liquid.bind(rules)]
	if max_depth > 0:
		choices.append(generate_tea.bind(rules))
	return _pick(choices).call(max_depth)

func generate_base(max_depth: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return _create_base(_pick(rules.base_kinds))

func generate_leaf(max_depth: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	var choices: Array[Callable] = []
	for kind in rules.basic_leaf_kinds:
		choices.append(func(_depth): return _create_basic_leaf(kind))
	if max_depth > 0:
		choices.append(func(depth): return TeaRecipeBook.brew_leaf(generate_leaf(depth - 1, rules), generate_liquid(depth - 1, rules)))
		choices.append(func(depth): return TeaRecipeBook.combine_leaves(generate_leaf(depth - 1, rules), generate_leaf(depth - 1, rules)))
	return _pick(choices).call(max_depth)

func generate_tea(max_depth: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return TeaRecipeBook.brew_tea(generate_leaf(max_depth - 1, rules), generate_liquid(max_depth - 1, rules))

func generate_liquid(max_depth: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	var choices: Array[Callable] = [generate_base.bind(rules)]
	if max_depth > 0:
		choices.append(generate_tea.bind(rules))
		choices.append(func(depth): return TeaRecipeBook.mix_liquids(generate_liquid(depth - 1, rules), generate_liquid(depth - 1, rules)))
	return _pick(choices).call(max_depth)

func generate_product_from_base(max_depth: int, base_kind: int) -> ProductExpression:
	return generate_product(max_depth, TeaOrderGenerationRules.for_base(base_kind))

func generate_product_from_leaf(max_depth: int, leaf_kind: int) -> ProductExpression:
	return generate_product(max_depth, TeaOrderGenerationRules.for_leaf(leaf_kind))

func generate_product_by_max_length(max_length: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return generate_product_by_length(_pick_possible_length(max_length, 1, _can_generate_product_length), rules)

func generate_leaf_by_max_length(max_length: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return generate_leaf_by_length(_pick_possible_length(max_length, 1, _can_generate_leaf_length), rules)

func generate_tea_by_max_length(max_length: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return generate_tea_by_length(_pick_possible_length(max_length, 2, _can_generate_tea_length), rules)

func generate_liquid_by_max_length(max_length: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return generate_liquid_by_length(_pick_possible_length(max_length, 1, _can_generate_liquid_length), rules)

func generate_product_by_length(length_value: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	length_value = _normalize_length(length_value, _can_generate_product_length)
	var choices: Array[Callable] = []
	if _can_generate_leaf_length(length_value): choices.append(func(): return generate_leaf_by_length(length_value, rules))
	if _can_generate_liquid_length(length_value): choices.append(func(): return generate_liquid_by_length(length_value, rules))
	if length_value == 1: choices.append(func(): return generate_base_by_length(length_value, rules))
	if _can_generate_tea_length(length_value): choices.append(func(): return generate_tea_by_length(length_value, rules))
	return _pick(choices).call()

func generate_base_by_length(length_value: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	return _create_base(_pick(rules.base_kinds))

func generate_leaf_by_length(length_value: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	length_value = _normalize_length(length_value, _can_generate_leaf_length)
	if length_value == 1:
		return _create_basic_leaf(_pick(rules.basic_leaf_kinds))
	var choices: Array[Callable] = []
	for split in _split_brewed_leaf_child_lengths(length_value):
		choices.append(func(): return TeaRecipeBook.brew_leaf(generate_leaf_by_length(split[0], rules), generate_liquid_by_length(split[1], rules)))
	for split in _split_combined_leaf_child_lengths(length_value):
		choices.append(func(): return TeaRecipeBook.combine_leaves(generate_leaf_by_length(split[0], rules), generate_leaf_by_length(split[1], rules)))
	return _pick(choices).call()

func generate_tea_by_length(length_value: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	length_value = _normalize_length(length_value, _can_generate_tea_length)
	var split = _pick(_split_tea_child_lengths(length_value))
	return TeaRecipeBook.brew_tea(generate_leaf_by_length(split[0], rules), generate_liquid_by_length(split[1], rules))

func generate_liquid_by_length(length_value: int, rules: TeaOrderGenerationRules = null) -> ProductExpression:
	if rules == null: rules = TeaOrderGenerationRules.any()
	length_value = _normalize_length(length_value, _can_generate_liquid_length)
	if length_value == 1:
		return generate_base_by_length(length_value, rules)
	var choices: Array[Callable] = [func(): return generate_tea_by_length(length_value, rules)]
	for split in _split_mixed_liquid_child_lengths(length_value):
		choices.append(func(): return TeaRecipeBook.mix_liquids(generate_liquid_by_length(split[0], rules), generate_liquid_by_length(split[1], rules)))
	return _pick(choices).call()

func generate_impossible(name: String) -> ProductExpression:
	return TeaRecipeBook.impossible(name)

func generate_invalid(max_depth: int = 2) -> ProductExpression:
	var imp := generate_impossible(_pick(ImpossibleStringCatalog.current().strings))
	var safe_depth = maxi(0, max_depth)
	var choices: Array[Callable] = [
		func(): return TeaRecipeBook.brew_tea(TeaRecipeBook.combine_leaves(generate_leaf(safe_depth), imp), generate_liquid(safe_depth)),
		func(): return TeaRecipeBook.brew_tea(generate_leaf(safe_depth), TeaRecipeBook.mix_liquids(imp, generate_liquid(safe_depth))),
		func(): return TeaRecipeBook.brew_tea(generate_leaf(safe_depth), TeaRecipeBook.mix_liquids(generate_liquid(safe_depth), imp)),
		func(): return TeaRecipeBook.brew_leaf(TeaRecipeBook.combine_leaves(generate_leaf(safe_depth), imp), generate_liquid(safe_depth)),
		func(): return TeaRecipeBook.brew_leaf(generate_leaf(safe_depth), TeaRecipeBook.mix_liquids(generate_liquid(safe_depth), imp)),
		func(): return TeaRecipeBook.combine_leaves(TeaRecipeBook.brew_leaf(generate_leaf(safe_depth), imp), generate_leaf(safe_depth)),
		func(): return TeaRecipeBook.mix_liquids(TeaRecipeBook.brew_tea(generate_leaf(safe_depth), imp), generate_liquid(safe_depth)),
		func(): return TeaRecipeBook.mix_liquids(generate_liquid(safe_depth), TeaRecipeBook.brew_tea(generate_leaf(safe_depth), imp)),
	]
	if max_depth > 0:
		choices.append(func(): return TeaRecipeBook.brew_tea(generate_leaf(max_depth - 1), TeaRecipeBook.mix_liquids(imp, generate_liquid(max_depth - 1))))
		choices.append(func(): return TeaRecipeBook.brew_leaf(TeaRecipeBook.combine_leaves(generate_leaf(max_depth - 1), imp), generate_liquid(max_depth - 1)))
	return _pick(choices).call()

func _create_base(kind: int) -> BaseExpression:
	return TeaRecipeBook.milk_tea_base() if kind == BaseKind.MILK_TEA else TeaRecipeBook.tea_base()

func _create_basic_leaf(kind: int) -> BasicLeafExpression:
	return TeaRecipeBook.black_leaf() if kind == BasicLeafKind.BLACK else TeaRecipeBook.green_leaf()

func _pick_possible_length(max_length: int, min_length: int, can_generate: Callable) -> int:
	var lengths: Array[int] = []
	for value in range(min_length, max_length + 1):
		if can_generate.call(value): lengths.append(value)
	return _pick(lengths)

func _normalize_length(length_value: int, can_generate: Callable) -> int:
	if can_generate.call(length_value): return length_value
	if length_value % 2 == 0 and can_generate.call(length_value - 1): return length_value - 1
	return max(1, length_value)

static func _can_generate_product_length(length_value: int) -> bool: return length_value >= 1
static func _can_generate_base_length(length_value: int) -> bool: return length_value == 1
static func _can_generate_leaf_length(length_value: int) -> bool: return length_value == 1 or length_value >= 3
static func _can_generate_tea_length(length_value: int) -> bool: return length_value >= 2
static func _can_generate_liquid_length(length_value: int) -> bool: return length_value >= 1

static func _split_brewed_leaf_child_lengths(length_value: int) -> Array:
	var splits := []
	var remaining := length_value - 1
	for left in range(1, remaining):
		var right := remaining - left
		if _can_generate_leaf_length(left) and _can_generate_liquid_length(right): splits.append([left, right])
	return splits

static func _split_combined_leaf_child_lengths(length_value: int) -> Array:
	var splits := []
	var remaining := length_value - 1
	for left in range(1, remaining):
		var right := remaining - left
		if _can_generate_leaf_length(left) and _can_generate_leaf_length(right): splits.append([left, right])
	return splits

static func _split_tea_child_lengths(length_value: int) -> Array:
	var splits := []
	for left in range(1, length_value):
		var right := length_value - left
		if _can_generate_leaf_length(left) and _can_generate_liquid_length(right): splits.append([left, right])
	return splits

static func _split_mixed_liquid_child_lengths(length_value: int) -> Array:
	var splits := []
	var remaining := length_value - 1
	for left in range(1, remaining):
		var right := remaining - left
		if _can_generate_liquid_length(left) and _can_generate_liquid_length(right): splits.append([left, right])
	return splits

func _pick(choices: Array):
	return choices[_random.randi_range(0, choices.size() - 1)]
