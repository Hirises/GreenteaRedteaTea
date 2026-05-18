class_name DraggablePlate
extends Node2D

@export var dragArea: DragAreaPlate
@export var hoverHighlight: HoverHighlightable
var returnArea: DragAreaContainer
var DragArea: DragAreaPlate:
	get: return dragArea

func _process(delta: float) -> void:
	DraggableUtil.DefaultDragBehavior(self, self, delta, returnArea)
	dragArea.visible = dragArea.HasLeaf() and InputManager.Instance.currentDragItem != self
	dragArea.z_index = z_index

func OnPick() -> void:
	print("Plate picked up!")
	SoundManager.Play(SFXType.PLATE_PICK)

func OnDrop(dropArea: DragArea) -> void:
	print("Plate dropped on %s!" % (dropArea.name if dropArea != null else "<null>"))
	SoundManager.Play(SFXType.PLATE_PUT)
	if dropArea == dragArea:
		print("Plate dropped back on its own drag area. Returning to original position.")
		_ReturnToOriginalPosition()
		return
	if dropArea is DragAreaContainer:
		if dropArea.TryDropDraggable(self):
			returnArea = dropArea
			print("Plate successfully dropped into container.")
		else:
			print("Failed to drop plate into container. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	if dropArea is DragAreaTrash:
		print("Plate dropped into trash. Destroying plate.")
		dropArea.OnTrash()
		_Destroy()
		return
	if dropArea is DragAreaSubmit:
		if dropArea.TrySubmit(self):
			_Destroy()
			print("Plate successfully submitted.")
		else:
			print("Failed to submit plate. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	_ReturnToOriginalPosition()

func OnCancelDrag() -> void:
	print("Plate drag cancelled. Returning to original position.")
	_ReturnToOriginalPosition()

func _Destroy() -> void:
	print("Plate destroyed.")
	queue_free()

func _ReturnToOriginalPosition() -> void:
	if returnArea == null:
		print("No return area set. Destroying plate.")
		_Destroy()
		return
	if returnArea.TryDropDraggable(self):
		print("Plate returned to original position.")
	else:
		print("Failed to return plate to original position. Destroying plate.")
		_Destroy()

func TryPutOnPlate(leaf: DraggableLeaf) -> bool:
	print("Putting leaf on plate.")
	return dragArea.TryDropDraggable(leaf)

func TryMergeLeaf(leaf: DraggableLeaf) -> bool:
	print("Trying to merge leaf with existing leaf on plate.")
	return dragArea.TryMergeLeaf(leaf)

func GetHoverHighlight() -> HoverHighlightable:
	return hoverHighlight
