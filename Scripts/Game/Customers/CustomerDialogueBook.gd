class_name CustomerDialogueBook

const DIALOGUE_PATH := "res://Data/CustomerDialogues.json"
const DEFAULT_CUSTOMER_CLASS_NAME := "DefaultCustomer"
const ORDER_TOKEN := "{order}"
static var _random := RandomNumberGenerator.new()
static var dialogues = null

static func GetOrder(customerClassName: String, orderName: String) -> String:
	return _get_line(customerClassName, "Order", ORDER_TOKEN + " please").replace(ORDER_TOKEN, orderName)

static func GetThank(customerClassName: String) -> String:
	return _get_line(customerClassName, "Thank", "thx")

static func GetComplaint(customerClassName: String, result: int) -> String:
	return _get_line(customerClassName, _complaint_key(result), "")

static func _get_line(customerClassName: String, key: String, fallback: String) -> String:
	var lines := _get_lines(customerClassName, key)
	if lines.size() == 0 and customerClassName != DEFAULT_CUSTOMER_CLASS_NAME:
		lines = _get_lines(DEFAULT_CUSTOMER_CLASS_NAME, key)
	if lines.size() == 0:
		return fallback
	var index := 0 if lines.size() == 1 else _random.randi_range(0, lines.size() - 1)
	return str(lines[index])

static func _get_lines(customerClassName: String, key: String) -> Array:
	var dialogue: Dictionary = _load_dialogues().get(customerClassName, {})
	if key.begins_with("Complaint."):
		return dialogue.get("Complaint", {}).get(key.get_slice(".", 1), [])
	return dialogue.get(key, [])

static func _complaint_key(result: int) -> String:
	match result:
		OrderResult.TIMEOUT:
			return "Complaint.Timeout"
		OrderResult.WRONG_MENU:
			return "Complaint.WrongMenu"
		OrderResult.KICKED_OUT:
			return "Complaint.KickedOut"
	return ""

static func _load_dialogues() -> Dictionary:
	if dialogues != null:
		return dialogues
	_random.randomize()
	if !FileAccess.file_exists(DIALOGUE_PATH):
		push_warning("Customer dialogue file not found: %s" % DIALOGUE_PATH)
		dialogues = {}
		return dialogues
	var parsed = JSON.parse_string(FileAccess.get_file_as_string(DIALOGUE_PATH))
	dialogues = parsed if typeof(parsed) == TYPE_DICTIONARY else {}
	return dialogues
