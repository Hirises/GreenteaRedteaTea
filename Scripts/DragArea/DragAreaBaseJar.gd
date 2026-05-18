class_name DragAreaBaseJar
extends DragArea

@export var draggableJar: DraggableBaseJar

func GetDraggable():
	return draggableJar

func GetTooltipText() -> String:
	return BaseExpression.new(draggableJar.baseKind).display_name

