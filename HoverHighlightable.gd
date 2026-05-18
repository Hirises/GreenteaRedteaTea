class_name HoverHighlightable
extends Node2D

var hovering := false

func _process(delta: float) -> void:
	if hovering:
		scale = scale.lerp(Vector2(1.1, 1.1), 20.0 * delta)
	else:
		scale = scale.lerp(Vector2.ONE, 20.0 * delta)
	hovering = false

func SetHover() -> void:
	hovering = true
