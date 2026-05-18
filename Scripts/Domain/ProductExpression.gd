class_name ProductExpression
extends RefCounted

var categories: int:
	get: return get_categories()
var depth: int:
	get: return get_depth()
var length: int:
	get: return get_length()
var display_name: String:
	get: return get_display_name()
var display_name_with_brackets: String:
	get: return get_display_name_with_brackets()
var color: ProductColor:
	get: return get_color()
var is_wet: bool:
	get: return get_is_wet()

func get_categories() -> int: return ProductCategory.NONE
func get_depth() -> int: return 0
func get_length() -> int: return 0
func get_display_name() -> String: return ""
func get_display_name_with_brackets() -> String: return get_display_name()
func get_color() -> ProductColor: return ProductColor.new()
func get_is_wet() -> bool: return false

func is_category(category: int) -> bool:
	return (get_categories() & category) == category

static func require_category(expression: ProductExpression, category: int, parameter_name: String) -> void:
	if expression == null:
		push_error("%s must not be null." % parameter_name)
		return
	if !expression.is_category(category) and !(expression is ImposibleExpression):
		push_error("%s must be %s." % [parameter_name, category])
