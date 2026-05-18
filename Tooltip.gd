class_name Tooltip
extends Node2D

@export var textNode: Label
@export var tooltipDelaySeconds: float = 0.3
var showing := false
var currentHoverDragArea: DragArea = null
var hoverTimer := 0.0

func _ready() -> void:
	scale = Vector2.ZERO

func _process(delta: float) -> void:
	if currentHoverDragArea != null and InputManager.Instance.currentHoverArea == currentHoverDragArea:
		hoverTimer += delta
		if !showing and hoverTimer >= tooltipDelaySeconds:
			ShowTooltip(currentHoverDragArea)
	else:
		currentHoverDragArea = InputManager.Instance.currentHoverArea
		hoverTimer = 0.0
		if showing:
			HideTooltip()
	position = get_global_mouse_position()
	if showing:
		scale = scale.lerp(Vector2.ONE, 10.0 * delta)
		textNode.text = currentHoverDragArea.GetTooltipText()
	else:
		scale = scale.lerp(Vector2.ZERO, 10.0 * delta)

func ShowTooltip(dragArea: DragArea) -> void:
	showing = true
	textNode.text = dragArea.GetTooltipText()

func HideTooltip() -> void:
	showing = false
