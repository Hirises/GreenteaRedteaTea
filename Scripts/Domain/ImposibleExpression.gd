class_name ImposibleExpression
extends ProductExpression

var name_value: String

func _init(p_name: String = "") -> void:
	name_value = p_name

func get_categories() -> int: return ProductCategory.PRODUCT | ProductCategory.LIQUID
func get_depth() -> int: return 1
func get_length() -> int: return 1
func get_display_name() -> String: return name_value
func get_display_name_with_brackets() -> String: return ProductVisualCatalog.current().wrap_depth_increased_name(name_value)
func get_color() -> ProductColor: return ProductColor.from_rgb255(0, 0, 0, 1)
func get_is_wet() -> bool: return false
