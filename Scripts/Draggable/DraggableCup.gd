class_name DraggableCup
extends Node2D

@export var liquidSprite: Sprite2D
@export var hoverHighlight: HoverHighlightable
@export var animationPlayer: AnimationPlayer
var returnArea: DragAreaContainer
var hasContent := false
var liquidContent: ProductExpression
var HasContent: bool:
	get: return hasContent
var LiquidContent: ProductExpression:
	get: return liquidContent

func _process(delta: float) -> void:
	DraggableUtil.DefaultDragBehavior(self, self, delta, returnArea)
	liquidSprite.visible = hasContent
	if hasContent:
		liquidSprite.modulate = liquidContent.color.to_godot_color()

func OnPick() -> void:
	print("Cup picked up!")
	SoundManager.Play(SFXType.CUP_PICK)

func OnDrop(dropArea: DragArea) -> void:
	print("Cup dropped on %s!" % (dropArea.name if dropArea != null else "<null>"))
	if dropArea is DragAreaContainer:
		if dropArea.TryDropDraggable(self):
			returnArea = dropArea
			print("Cup successfully dropped into container.")
			SoundManager.Play(SFXType.CUP_PUT)
		elif hasContent and dropArea.TryFill(liquidContent):
			hasContent = false
			liquidContent = null
			_ReturnToOriginalPosition()
			print("Cup successfully poured into container.")
			SoundManager.Play(SFXType.CUP_POUR)
		else:
			print("Failed to drop cup into container. Returning to original position.")
			_ReturnToOriginalPosition()
			SoundManager.Play(SFXType.CUP_PUT)
		return
	if dropArea is DragAreaTeapot:
		if hasContent and dropArea.TryFill(liquidContent):
			hasContent = false
			liquidContent = null
			_ReturnToOriginalPosition()
			print("Cup successfully poured into teapot.")
			SoundManager.Play(SFXType.CUP_POUR)
		else:
			print("Failed to pour cup into teapot. Returning to original position.")
			_ReturnToOriginalPosition()
			SoundManager.Play(SFXType.CUP_PUT)
		return
	if dropArea is DragAreaTeapotInside:
		if hasContent and dropArea.TryFillTeapot(liquidContent):
			hasContent = false
			liquidContent = null
			_ReturnToOriginalPosition()
			print("Cup successfully poured into teapot.")
			SoundManager.Play(SFXType.CUP_POUR)
		else:
			print("Failed to pour cup into teapot. Returning to original position.")
			_ReturnToOriginalPosition()
			SoundManager.Play(SFXType.CUP_PUT)
		return
	if dropArea is DragAreaTrash:
		print("Cup dropped into trash. Destroying cup.")
		dropArea.OnTrash()
		_Destroy()
		return
	if dropArea is DragAreaSubmit:
		if dropArea.TrySubmit(self):
			_Destroy()
			SoundManager.Play(SFXType.CUP_PUT)
			print("Cup successfully submitted.")
		else:
			SoundManager.Play(SFXType.CUP_PUT)
			print("Failed to submit cup. Returning to original position.")
			_ReturnToOriginalPosition()
		return
	SoundManager.Play(SFXType.CUP_PUT)
	_ReturnToOriginalPosition()

func OnCancelDrag() -> void:
	print("Cup drag cancelled. Returning to original position.")
	_ReturnToOriginalPosition()

func _Destroy() -> void:
	print("Cup destroyed.")
	queue_free()

func _ReturnToOriginalPosition() -> void:
	if returnArea == null:
		print("No return area set. Destroying cup.")
		_Destroy()
		return
	if returnArea.TryDropDraggable(self):
		print("Cup returned to original position.")
	else:
		print("Failed to return cup to original position. Destroying cup.")
		_Destroy()

func TryFill(liquid: ProductExpression) -> bool:
	if !liquid.is_category(ProductCategory.LIQUID):
		print("Cannot fill cup with non-liquid product.")
		return false
	if hasContent:
		var mix := MixedLiquidExpression.new(liquidContent, liquid)
		liquidContent = mix
		print("Cup already has content. Mixed to %s." % mix.display_name)
		animationPlayer.play("shake")
		return true
	liquidContent = liquid
	hasContent = true
	print("Cup filled with %s." % liquid.display_name)
	animationPlayer.play("shake")
	return true

func GetHoverHighlight() -> HoverHighlightable:
	return hoverHighlight
