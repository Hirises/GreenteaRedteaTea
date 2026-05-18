class_name DragAreaSubmit
extends DragArea

@export var GameManager: GameManager

func GetDraggable():
	return null

func TrySubmit(draggable) -> bool:
	if draggable is DraggableCup:
		var cup: DraggableCup = draggable
		if !cup.HasContent:
			print("Cannot submit empty cup!")
			return false
		print("Submitting cup with content: %s" % cup.LiquidContent.display_name)
		GameManager.Serve(cup.LiquidContent)
		return true
	if draggable is DraggablePlate:
		var plate: DraggablePlate = draggable
		if !plate.DragArea.HasLeaf():
			print("Cannot submit empty plate!")
			return false
		var leaf = plate.DragArea.GetLeaf().GetLeafContent()
		print("Submitting plate with leaf: %s" % leaf.display_name)
		GameManager.Serve(leaf)
		return true
	print("Unknown draggable type submitted. Rejecting.")
	return false

func CanDrag() -> bool:
	return false
