class_name DragAreaContainer
extends DragArea

var currentDraggable

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
	if !(draggable is DraggableCup or draggable is DraggablePlate or draggable is DraggableLeaf):
		print("Only draggable items that can be contained can be placed in the container!")
		return false
	currentDraggable = draggable
	SetHoverHighlight(currentDraggable.GetHoverHighlight())
	return true

func TryFill(liquid: ProductExpression) -> bool:
	if currentDraggable == null:
		print("No draggable item in container to fill!")
		return false
	if !(currentDraggable is DraggableCup):
		print("Only cups can be filled in the container!")
		return false
	return currentDraggable.TryFill(liquid)

func TryPutLeafOnPlate(leaf: DraggableLeaf) -> bool:
	if currentDraggable == null:
		print("No draggable item in container to put leaf on!")
		return false
	if !(currentDraggable is DraggablePlate):
		print("Only plates can have leaves put on them in the container!")
		return false
	return currentDraggable.TryPutOnPlate(leaf)

func TryMergeLeaf(leaf: DraggableLeaf) -> bool:
	if currentDraggable == null:
		print("No draggable item in container to merge leaf with!")
		return false
	if currentDraggable is DraggableLeaf:
		var mix := CombinedLeafExpression.new(currentDraggable.GetLeafContent(), leaf.GetLeafContent())
		currentDraggable.SetLeafContent(mix)
		print("Merged leaf with existing leaf in container to create %s." % mix.display_name)
		currentDraggable.Shake()
		return true
	if currentDraggable is DraggablePlate:
		return currentDraggable.TryMergeLeaf(leaf)
	print("Cannot merge leaf!")
	return false

func GetPlateDragArea() -> DragArea:
	return currentDraggable.DragArea if currentDraggable is DraggablePlate else null

func GetTooltipText() -> String:
	if currentDraggable == null:
		return ""
	if currentDraggable is DraggableLeaf:
		return currentDraggable.GetLeafContent().display_name
	if currentDraggable is DraggableCup:
		return currentDraggable.LiquidContent.display_name if currentDraggable.HasContent else ""
	if currentDraggable is DraggablePlate:
		return currentDraggable.DragArea.GetLeaf().GetLeafContent().display_name if currentDraggable.DragArea.HasLeaf() else ""
	return ""

func CanDrag() -> bool:
	return currentDraggable != null
