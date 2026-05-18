class_name TeaOrderGenerationRules

var base_kinds: Array[int]
var basic_leaf_kinds: Array[int]

static func any() -> TeaOrderGenerationRules:
	return TeaOrderGenerationRules.new([BaseKind.TEA, BaseKind.MILK_TEA], [BasicLeafKind.GREEN, BasicLeafKind.BLACK])

func _init(p_base_kinds: Array = [BaseKind.TEA, BaseKind.MILK_TEA], p_basic_leaf_kinds: Array = [BasicLeafKind.GREEN, BasicLeafKind.BLACK]) -> void:
	base_kinds = []
	for kind in p_base_kinds:
		if !base_kinds.has(int(kind)):
			base_kinds.append(int(kind))
	basic_leaf_kinds = []
	for kind in p_basic_leaf_kinds:
		if !basic_leaf_kinds.has(int(kind)):
			basic_leaf_kinds.append(int(kind))

static func for_base(base_kind: int) -> TeaOrderGenerationRules:
	return TeaOrderGenerationRules.new([base_kind], any().basic_leaf_kinds)

static func for_leaf(leaf_kind: int) -> TeaOrderGenerationRules:
	return TeaOrderGenerationRules.new(any().base_kinds, [leaf_kind])

static func for_base_and_leaf(base_kind: int, leaf_kind: int) -> TeaOrderGenerationRules:
	return TeaOrderGenerationRules.new([base_kind], [leaf_kind])
