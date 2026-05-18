class_name DragAreaTeapotInside
extends DragArea

@export var teapot: DragAreaTeapot
var currentDraggable: DraggableLeaf

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
		print("Only leaves can be placed on the teapot inside!")
		return false
	currentDraggable = draggable
	SetHoverHighlight(currentDraggable.GetHoverHighlight())
	return true

func GetNode() -> Node2D:
	return self

func HasLeaf() -> bool:
	return currentDraggable != null

func GetLeaf() -> ProductExpression:
	if currentDraggable == null:
		print("No leaf in teapot inside to get!")
		return null
	return currentDraggable.GetLeafContent()

func SetLeaf(leafContent: ProductExpression) -> void:
	if currentDraggable == null:
		print("No leaf in teapot inside to set content of!")
		return
	currentDraggable.SetLeafContent(leafContent)
	currentDraggable.Shake()

func RemoveLeaf() -> void:
	if currentDraggable == null:
		print("No leaf in teapot inside to remove!")
		return
	currentDraggable.Destroy()
	currentDraggable = null
	SetHoverHighlight(null)

func TryFillTeapot(liquid: ProductExpression) -> bool:
	return teapot.TryFill(liquid)

func GetTooltipText() -> String:
	return "" if currentDraggable == null else currentDraggable.GetLeafContent().display_name

func CanDrag() -> bool:
	return currentDraggable != null
