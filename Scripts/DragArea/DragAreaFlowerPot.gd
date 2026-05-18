class_name DragAreaFlowerPot
extends DragArea

@export var flowerPot: FlowerPot

func GetDraggable():
	return flowerPot.PickLeaf()

func TryBloom(leaf: DraggableLeaf) -> bool:
	return flowerPot.TryBloom(leaf.GetLeafContent())

func GetTooltipText() -> String:
	if !flowerPot.Bloomed:
		return ""
	return flowerPot.LeafContent.display_name

func CanDrag() -> bool:
	return flowerPot.Bloomed
