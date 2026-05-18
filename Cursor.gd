class_name Cursor
extends Node2D

@export var defaultSpr: Sprite2D
@export var draggableSpr: Sprite2D
@export var draggingSpr: Sprite2D
@export var clickableSpr: Sprite2D
@export var kickSpr: Sprite2D

func _ready() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_HIDDEN
	_set_cursor(defaultSpr)

func _set_cursor(current: Sprite2D) -> void:
	defaultSpr.visible = false
	draggableSpr.visible = false
	draggingSpr.visible = false
	clickableSpr.visible = false
	kickSpr.visible = false
	current.visible = true

func _process(_delta: float) -> void:
	position = get_global_mouse_position()
	var draggable = InputManager.Instance.currentDragItem
	if draggable != null:
		_set_cursor(draggingSpr)
		return
	var click_area = InputManager.Instance.CurrentHoverClickArea
	if click_area != null and click_area.CanClick():
		_set_cursor(kickSpr if click_area is ClickAreaKick else clickableSpr)
		return
	var drag_area = InputManager.Instance.currentHoverArea
	if drag_area != null and drag_area.CanDrag():
		_set_cursor(draggableSpr)
		return
	_set_cursor(defaultSpr)
