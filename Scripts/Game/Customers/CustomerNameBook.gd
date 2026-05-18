class_name CustomerNameBook

const NAME_PATH := "res://Data/CustomerNames.json"
const DEFAULT_CUSTOMER_CLASS_NAME := "DefaultCustomer"
const DEFAULT_NAME := "Default"
static var _random := RandomNumberGenerator.new()
static var names = null

static func GetName(customerClassName: String) -> String:
	var lines := _get_names(customerClassName)
	if lines.size() == 0 and customerClassName != DEFAULT_CUSTOMER_CLASS_NAME:
		lines = _get_names(DEFAULT_CUSTOMER_CLASS_NAME)
	if lines.size() == 0:
		return DEFAULT_NAME
	var index := 0 if lines.size() == 1 else _random.randi_range(0, lines.size() - 1)
	return str(lines[index])

static func _get_names(customerClassName: String) -> Array:
	var loaded = _load_names()
	return loaded.get(customerClassName, [])

static func _load_names() -> Dictionary:
	if names != null:
		return names
	_random.randomize()
	if !FileAccess.file_exists(NAME_PATH):
		push_warning("Customer name file not found: %s" % NAME_PATH)
		names = {}
		return names
	var parsed = JSON.parse_string(FileAccess.get_file_as_string(NAME_PATH))
	names = parsed if typeof(parsed) == TYPE_DICTIONARY else {}
	return names
