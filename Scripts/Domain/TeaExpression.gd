class_name TeaExpression
extends ProductExpression

var leaf: ProductExpression
var liquid: ProductExpression

func _init(p_leaf: ProductExpression = null, p_liquid: ProductExpression = null) -> void:
	ProductExpression.require_category(p_leaf, ProductCategory.LEAF, "leaf")
	ProductExpression.require_category(p_liquid, ProductCategory.LIQUID, "liquid")
	leaf = p_leaf
	liquid = p_liquid

func get_categories() -> int: return ProductCategory.PRODUCT | ProductCategory.TEA | ProductCategory.LIQUID
func get_depth() -> int: return 1 + maxi(leaf.depth, liquid.depth)
func get_length() -> int: return leaf.length + liquid.length
func get_display_name() -> String: return "%s%s%s" % [leaf.display_name, liquid.display_name, ProductVisualCatalog.current().tea_suffix()]
func get_display_name_with_brackets() -> String: return ProductVisualCatalog.current().wrap_depth_increased_name("%s%s%s" % [leaf.display_name_with_brackets, liquid.display_name_with_brackets, ProductVisualCatalog.current().tea_suffix()])
func get_color() -> ProductColor: return ProductVisualCatalog.current().calculate_brewed_tea_color(leaf.color, liquid.color)
func get_is_wet() -> bool: return true
