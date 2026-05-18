class_name ImpossibleStringCatalog

const DEFAULT_JSON_PATH := "res://Scripts/Domain/ImpossibleStrings.json"
static var _current: ImpossibleStringCatalog
var strings: Array[String]

static func current() -> ImpossibleStringCatalog:
	if _current == null:
		_current = load_from_file(DEFAULT_JSON_PATH)
	return _current

static func configure(catalog: ImpossibleStringCatalog) -> void:
	_current = catalog

static func configure_from_file(path: String) -> void:
	_current = load_from_file(path)

static func load_from_file(path: String) -> ImpossibleStringCatalog:
	var parsed = JSON.parse_string(FileAccess.get_file_as_string(path))
	var values: Array[String] = []
	if typeof(parsed) == TYPE_DICTIONARY:
		for value in _dict_get(parsed, "Strings", []):
			var s := str(value)
			if !s.strip_edges().is_empty():
				values.append(s)
	return ImpossibleStringCatalog.new(values)

func _init(p_strings: Array[String] = []) -> void:
	strings = p_strings

static func _dict_get(source: Dictionary, key: String, fallback = null):
	if source.has(key):
		return source[key]
	var lower_key := key.to_lower()
	for existing_key in source.keys():
		if str(existing_key).to_lower() == lower_key:
			return source[existing_key]
	return fallback
