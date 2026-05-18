class_name InputManager
extends Node

static var Instance: InputManager

enum InputState { NONE, MOUSE_DOWN, DRAGGING }

var inputState: int = InputState.NONE
var lastClickArea: DragArea
var currentHoverAreas: Array[DragArea] = []
var CurrentHoverClickArea: ClickArea
var lastClickPosition := Vector2.ZERO
@export var dragThreshold: float = 10.0
var currentDragItem

var currentHoverArea: DragArea:
	get: return _get_smallest_area(currentHoverAreas) if currentHoverAreas.size() > 0 else null

func _ready() -> void:
	Instance = self

func OnAreaEntered(area: DragArea) -> void:
	if !currentHoverAreas.has(area):
		currentHoverAreas.append(area)

func OnAreaExited(area: DragArea) -> void:
	currentHoverAreas.erase(area)

func OnClickAreaEntered(area: ClickArea) -> void:
	CurrentHoverClickArea = area

func OnClickAreaExited(area: ClickArea) -> void:
	if CurrentHoverClickArea == area:
		CurrentHoverClickArea = null

func _get_smallest_area(areas: Array[DragArea]) -> DragArea:
	var smallest: DragArea = null
	var smallest_size := INF
	for area in areas:
		var size := area.scale.length()
		if size < smallest_size:
			smallest_size = size
			smallest = area
	return smallest

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.pressed:
			lastClickPosition = event.position
			lastClickArea = currentHoverArea
			inputState = InputState.MOUSE_DOWN
		else:
			if inputState == InputState.DRAGGING:
				_on_drag_end(currentHoverArea)
			elif inputState == InputState.MOUSE_DOWN:
				_on_click(lastClickArea)
				if CurrentHoverClickArea != null:
					CurrentHoverClickArea.OnClick()
			inputState = InputState.NONE
	elif event is InputEventMouseMotion:
		if inputState == InputState.MOUSE_DOWN and event.position.distance_to(lastClickPosition) > dragThreshold:
			inputState = InputState.DRAGGING
			_on_drag_start(lastClickArea)

func _on_click(area: DragArea) -> void:
	if currentDragItem != null:
		currentDragItem.OnDrop(area)
		currentDragItem = null
		return
	var draggable = area.GetDraggable() if area != null else null
	if draggable != null:
		currentDragItem = draggable
		currentDragItem.OnPick()

func _on_drag_start(area: DragArea) -> void:
	if area == null:
		return
	if currentDragItem != null:
		currentDragItem.OnCancelDrag()
		currentDragItem = null
	var draggable = area.GetDraggable()
	if draggable != null:
		currentDragItem = draggable
		currentDragItem.OnPick()

func _on_drag_end(area: DragArea) -> void:
	if currentDragItem != null:
		currentDragItem.OnDrop(area)
		currentDragItem = null
