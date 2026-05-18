class_name DragAreaCupPile
extends DragArea

@export var cupScene: PackedScene

func GetDraggable():
	var cup = cupScene.instantiate()
	add_sibling(cup)
	cup.position = global_position
	return cup
