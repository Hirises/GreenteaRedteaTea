class_name DraggableLeaf
extends Node2D

@export var leafSprite: Sprite2D
@export var hoverHighlight: HoverHighlightable
@export var animationPlayer: AnimationPlayer
var returnArea
var leafContent: ProductExpression

func Initialize(kind: int) -> void:
	leafContent = BasicLeafExpression.new(kind)

func _process(delta: float) -> void:
	var contained := returnArea is DragAreaPlate or returnArea is DragAreaTeapotInside
	DraggableUtil.DefaultDragBehavior(self, self, delta, returnArea.GetNode() if returnArea != null else null, 1 if contained else 10, 90.0 if contained else 20.0)
	if leafContent == null:
		push_error("Leaf content is null! This should not happen.")
		return
	leafSprite.modulate = leafContent.color.to_godot_color()

func OnPick() -> void:
	print("Leaf picked up!")
	SoundManager.Play(SFXType.LEAF_PICK)

func OnDrop(dropArea: DragArea) -> void:
	print("Leaf dropped on %s!" % (dropArea.name if dropArea != null else "<null>"))
	SoundManager.Play(SFXType.LEAF_PUT)
	if dropArea is DragAreaContainer:
		if dropArea.TryDropDraggable(self):
			returnArea = dropArea
			print("Leaf successfully dropped into container.")
		elif dropArea.TryPutLeafOnPlate(self):
			returnArea = dropArea.GetPlateDragArea()
			print("Leaf successfully put on plate.")
		elif dropArea.TryMergeLeaf(self):
			print("Leaf successfully merged with existing leaf on plate.")
			Destroy()
		else:
			print("Failed to drop leaf into container. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	if dropArea is DragAreaPlate:
		if dropArea.TryMergeLeaf(self):
			print("Leaf successfully merged with existing leaf on plate.")
			Destroy()
		else:
			print("Failed to merge with leaf in plate. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	if dropArea is DragAreaTeapot:
		if dropArea.TryPutLeafInTeapot(self):
			returnArea = dropArea.GetInsideArea()
			print("Leaf successfully put in teapot.")
		else:
			print("Failed to put leaf in teapot. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	if dropArea is DragAreaTrash:
		print("Leaf dropped into trash. Destroying leaf.")
		dropArea.OnTrash()
		Destroy()
		return
	if dropArea is DragAreaFlowerPot:
		if dropArea.TryBloom(self):
			print("Leaf successfully bloomed in flower pot.")
			Destroy()
		else:
			print("Failed to bloom leaf in flower pot. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	_ReturnToOriginalPosition()

func OnCancelDrag() -> void:
	print("Leaf drag cancelled. Returning to original position.")
	_ReturnToOriginalPosition()

func Destroy() -> void:
	print("Leaf destroyed.")
	queue_free()

func _ReturnToOriginalPosition() -> void:
	if returnArea == null:
		print("No return area set. Destroying leaf.")
		Destroy()
		return
	if returnArea.TryDropDraggable(self):
		print("Leaf returned to original position.")
	else:
		print("Failed to return leaf to original position. Destroying leaf.")
		Destroy()

func GetHoverHighlight() -> HoverHighlightable:
	return hoverHighlight

func GetLeafContent() -> ProductExpression:
	if leafContent == null:
		push_error("Leaf content is null! This should not happen.")
	return leafContent

func SetLeafContent(content: ProductExpression) -> void:
	if content == null:
		push_error("Cannot set leaf content to null!")
		return
	if !content.is_category(ProductCategory.LEAF):
		push_error("Cannot set leaf content to non-leaf product!")
		return
	leafContent = content

func Shake() -> void:
	animationPlayer.play("shake")
