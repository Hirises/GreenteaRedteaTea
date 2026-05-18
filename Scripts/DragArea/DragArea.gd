class_name DragArea
extends Area2D

@export var hoverHighlight: HoverHighlightable

func _process(_delta: float) -> void:
	if hoverHighlight == null:
		return
	if InputManager.Instance.currentHoverArea == self and visible:
		hoverHighlight.SetHover()

func OnArea2DEntered() -> void:
	if InputManager.Instance != null:
		InputManager.Instance.OnAreaEntered(self)

func OnArea2DExited() -> void:
	if InputManager.Instance != null:
		InputManager.Instance.OnAreaExited(self)

func GetDraggable():
	return null

func SetHoverHighlight(highlight: HoverHighlightable) -> void:
	hoverHighlight = highlight

func _exit_tree() -> void:
	if InputManager.Instance != null:
		InputManager.Instance.OnAreaExited(self)

func GetTooltipText() -> String:
	return ""

func CanDrag() -> bool:
	return true
