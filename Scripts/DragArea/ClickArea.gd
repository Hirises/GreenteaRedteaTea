class_name ClickArea
extends Area2D

@export var hoverHighlight: HoverHighlightable

func _process(_delta: float) -> void:
	if hoverHighlight == null:
		return
	if InputManager.Instance.CurrentHoverClickArea == self and InputManager.Instance.inputState == InputManager.InputState.NONE and visible:
		hoverHighlight.SetHover()

func OnArea2DEntered() -> void:
	if InputManager.Instance != null:
		InputManager.Instance.OnClickAreaEntered(self)

func OnArea2DExited() -> void:
	if InputManager.Instance != null:
		InputManager.Instance.OnClickAreaExited(self)

func SetHoverHighlight(highlight: HoverHighlightable) -> void:
	hoverHighlight = highlight

func _exit_tree() -> void:
	if InputManager.Instance != null:
		InputManager.Instance.OnClickAreaExited(self)

func OnClick() -> void:
	print("Clicked on %s!" % name)

func CanClick() -> bool:
	return true
