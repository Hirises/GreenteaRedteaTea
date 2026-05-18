class_name DragAreaPlate
extends DragArea

var currentDraggable: DraggableLeaf

func GetNode() -> Node2D:
	return self

func GetDraggable():
	var draggable = currentDraggable
	currentDraggable = null
	SetHoverHighlight(null)
	return draggable

func TryDropDraggable(draggable) -> bool:
	if currentDraggable != null:
		print("Container already has a draggable item!")
		return false
	if !(draggable is DraggableLeaf):
		print("Only leaves can be placed on the plate!")
		return false
	currentDraggable = draggable
	SetHoverHighlight(currentDraggable.GetHoverHighlight())
	return true

func HasLeaf() -> bool:
	return currentDraggable != null

func GetLeaf() -> DraggableLeaf:
	return currentDraggable

func _exit_tree() -> void:
	super._exit_tree()
	if currentDraggable != null:
		print("Plate removed from scene while it still had a leaf on it! Destroying the leaf as well.")
		currentDraggable.Destroy()

func TryMergeLeaf(newLeaf: DraggableLeaf) -> bool:
	if currentDraggable == null:
		print("No existing leaf to merge with on the plate!")
		return false
	var existingLeaf = currentDraggable
	var mix := CombinedLeafExpression.new(existingLeaf.GetLeafContent(), newLeaf.GetLeafContent())
	existingLeaf.SetLeafContent(mix)
	print("Merged leaf on plate to create %s." % mix.display_name)
	existingLeaf.Shake()
	return true

func GetTooltipText() -> String:
	return "" if currentDraggable == null else currentDraggable.GetLeafContent().display_name

func CanDrag() -> bool:
	return currentDraggable != null
