class_name CombinedLeafExpression
extends ProductExpression

var left: ProductExpression
var right: ProductExpression

func _init(p_left: ProductExpression = null, p_right: ProductExpression = null) -> void:
	ProductExpression.require_category(p_left, ProductCategory.LEAF, "left")
	ProductExpression.require_category(p_right, ProductCategory.LEAF, "right")
	left = p_left
	right = p_right

func get_categories() -> int: return ProductCategory.PRODUCT | ProductCategory.LEAF
func get_depth() -> int: return 1 + maxi(left.depth, right.depth)
func get_length() -> int: return 1 + left.length + right.length
func get_display_name() -> String: return "%s%s%s" % [left.display_name, right.display_name, ProductVisualCatalog.current().combined_leaf_suffix()]
func get_display_name_with_brackets() -> String: return ProductVisualCatalog.current().wrap_depth_increased_name("%s%s%s" % [left.display_name_with_brackets, right.display_name_with_brackets, ProductVisualCatalog.current().combined_leaf_suffix()])
func get_color() -> ProductColor: return ProductVisualCatalog.current().calculate_combined_leaf_color(left.color, right.color)
func get_is_wet() -> bool: return left.is_wet or right.is_wet
