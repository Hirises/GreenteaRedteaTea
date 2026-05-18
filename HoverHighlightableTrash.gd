class_name HoverHighlightableTrash
extends HoverHighlightable

@export var lid: Node2D

func _process(delta: float) -> void:
	if hovering:
		lid.rotation_degrees = lerpf(lid.rotation_degrees, 15.0, 20.0 * delta)
	else:
		lid.rotation_degrees = lerpf(lid.rotation_degrees, 0.0, 20.0 * delta)
	super._process(delta)
