class_name BaseExpression
extends ProductExpression

var kind: int

func _init(p_kind: int = BaseKind.TEA) -> void:
	kind = p_kind

func get_categories() -> int: return ProductCategory.PRODUCT | ProductCategory.BASE | ProductCategory.LIQUID
func get_depth() -> int: return 0
func get_length() -> int: return 1
func get_display_name() -> String: return ProductVisualCatalog.current().get_base_name(kind)
func get_display_name_with_brackets() -> String: return get_display_name()
func get_color() -> ProductColor: return ProductVisualCatalog.current().get_base_color(kind)
func get_is_wet() -> bool: return true
