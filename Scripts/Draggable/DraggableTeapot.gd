class_name DraggableTeapot
extends Node2D

@export var liquidTop: Sprite2D
@export var liquidBottom: Sprite2D
@export var insideArea: DragAreaTeapotInside
@export var dragOffset: Vector2
@export var animationPlayer: AnimationPlayer
var originalPosition := Vector2.ZERO
var hasContent := false
var liquidContent: ProductExpression
var HasContent: bool:
	get: return hasContent
var LiquidContent: ProductExpression:
	get: return liquidContent

func _process(delta: float) -> void:
	if InputManager.Instance.currentDragItem == self:
		var target_position := get_global_mouse_position() + dragOffset
		position = position.lerp(target_position, 20.0 * delta)
		z_index = DraggableUtil.DRAG_Z_INDEX
	else:
		position = position.lerp(originalPosition, 10.0 * delta)
		z_index = 0
	liquidTop.visible = hasContent
	liquidBottom.visible = hasContent
	if hasContent:
		liquidTop.modulate = liquidContent.color.to_godot_color()
		liquidBottom.modulate = liquidContent.color.to_godot_color()

func _ready() -> void:
	originalPosition = position

func OnPick() -> void:
	print("Teapot picked up!")
	SoundManager.Play(SFXType.TEAPOT_PICK)

func OnDrop(dropArea: DragArea) -> void:
	SoundManager.Play(SFXType.TEAPOT_PUT)
	if dropArea is DragAreaContainer:
		if hasContent and dropArea.TryFill(liquidContent):
			hasContent = false
			liquidContent = null
			SoundManager.Play(SFXType.TEAPOT_POUR)
		else:
			print("Failed to pour teapot into container.")
	elif dropArea is DragAreaTrash:
		dropArea.OnTrash()
		if hasContent:
			hasContent = false
			liquidContent = null
			SoundManager.Play(SFXType.TEAPOT_POUR)
			print("Teapot liquid dropped into trash. Emptying teapot.")
		elif insideArea.HasLeaf():
			insideArea.RemoveLeaf()
			SoundManager.Play(SFXType.LEAF_PUT)
			print("Leaf dropped into trash.")
		else:
			print("Nothing in teapot to drop into trash.")
		return
	print("Teapot dropped on %s!" % (dropArea.name if dropArea != null else "<null>"))

func OnCancelDrag() -> void:
	print("Teapot drag cancelled.")

func TryFill(liquid: ProductExpression) -> bool:
	if !liquid.is_category(ProductCategory.LIQUID):
		print("Cannot fill teapot with non-liquid product.")
		return false
	if hasContent:
		var mix := MixedLiquidExpression.new(liquidContent, liquid)
		liquidContent = mix
		print("Teapot already has content. Mixed to %s." % mix.display_name)
		animationPlayer.play("shake")
		return true
	liquidContent = liquid
	hasContent = true
	print("Teapot filled with %s." % liquid.display_name)
	animationPlayer.play("shake")
	return true

func CanBrew() -> bool:
	return hasContent and insideArea.HasLeaf()

func TryBrew() -> bool:
	if !hasContent:
		print("Cannot brew tea with an empty teapot.")
		return false
	if !insideArea.HasLeaf():
		print("Cannot brew tea without a leaf inside the teapot.")
		return false
	var leaf = insideArea.GetLeaf()
	var brewed := TeaExpression.new(leaf, liquidContent)
	insideArea.SetLeaf(BrewedLeafExpression.new(leaf, liquidContent))
	liquidContent = brewed
	print("Brewed tea in teapot. Now contains %s." % brewed.display_name)
	animationPlayer.play("shake")
	SoundManager.Play(SFXType.TEAPOT_BREW)
	return true
