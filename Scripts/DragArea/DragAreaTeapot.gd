class_name DragAreaTeapot
extends DragArea

@export var draggableTeapot: DraggableTeapot
@export var insideArea: DragAreaTeapotInside

var InsideArea: DragAreaTeapotInside:
	get: return insideArea

func _process(delta: float) -> void:
	super._process(delta)
	insideArea.visible = insideArea.HasLeaf() and InputManager.Instance.currentDragItem != draggableTeapot
	insideArea.z_index = draggableTeapot.z_index

func GetDraggable():
	return draggableTeapot

func GetInsideArea() -> DragAreaTeapotInside:
	return insideArea

func TryPutLeafInTeapot(draggable: DraggableLeaf) -> bool:
	return insideArea.TryDropDraggable(draggable)

func TryFill(liquid: ProductExpression) -> bool:
	return draggableTeapot.TryFill(liquid)

func GetTooltipText() -> String:
	if !draggableTeapot.HasContent:
		return ""
	return draggableTeapot.LiquidContent.display_name
