class_name TeaRecipeBook

static func tea_base() -> BaseExpression:
	return BaseExpression.new(BaseKind.TEA)

static func milk_tea_base() -> BaseExpression:
	return BaseExpression.new(BaseKind.MILK_TEA)

static func green_leaf() -> BasicLeafExpression:
	return BasicLeafExpression.new(BasicLeafKind.GREEN)

static func black_leaf() -> BasicLeafExpression:
	return BasicLeafExpression.new(BasicLeafKind.BLACK)

static func brew_tea(leaf: ProductExpression, liquid: ProductExpression) -> TeaExpression:
	return TeaExpression.new(leaf, liquid)

static func brew_leaf(leaf: ProductExpression, liquid: ProductExpression) -> BrewedLeafExpression:
	return BrewedLeafExpression.new(leaf, liquid)

static func combine_leaves(left: ProductExpression, right: ProductExpression) -> CombinedLeafExpression:
	return CombinedLeafExpression.new(left, right)

static func mix_liquids(left: ProductExpression, right: ProductExpression) -> MixedLiquidExpression:
	return MixedLiquidExpression.new(left, right)

static func impossible(name: String) -> ImposibleExpression:
	return ImposibleExpression.new(name)

static func can_serve(made: ProductExpression, order: ProductExpression) -> bool:
	if order is ImposibleExpression:
		return false
	return _same_expression(made, order)

static func _same_expression(a: ProductExpression, b: ProductExpression) -> bool:
	if a == null or b == null or a.get_script() != b.get_script():
		return false
	if a is BaseExpression:
		return a.kind == b.kind
	if a is BasicLeafExpression:
		return a.kind == b.kind
	if a is BrewedLeafExpression or a is TeaExpression:
		return _same_expression(a.leaf, b.leaf) and _same_expression(a.liquid, b.liquid)
	if a is CombinedLeafExpression or a is MixedLiquidExpression:
		return _same_expression(a.left, b.left) and _same_expression(a.right, b.right)
	if a is ImposibleExpression:
		return a.name_value == b.name_value
	return false
