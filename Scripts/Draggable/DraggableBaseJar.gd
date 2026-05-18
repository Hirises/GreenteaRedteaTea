class_name DraggableBaseJar
extends Node2D

@export_enum("Tea", "MilkTea") var baseKind: int = BaseKind.TEA
@export var dragOffset: Vector2
var originalPosition := Vector2.ZERO

func _process(delta: float) -> void:
	if InputManager.Instance.currentDragItem == self:
		var target_position := get_global_mouse_position() + dragOffset
		position = position.lerp(target_position, 20.0 * delta)
		z_index = DraggableUtil.DRAG_Z_INDEX
	else:
		position = position.lerp(originalPosition, 10.0 * delta)
		z_index = 0

func _ready() -> void:
	originalPosition = position

func OnPick() -> void:
	print("BaseJar picked up!")
	SoundManager.Play(SFXType.JAR_PICK)

func OnDrop(dropArea: DragArea) -> void:
	SoundManager.Play(SFXType.JAR_PUT)
	if dropArea is DragAreaContainer:
		if dropArea.TryFill(BaseExpression.new(baseKind)):
			SoundManager.Play(SFXType.JAR_POUR)
	elif dropArea is DragAreaTeapot:
		if dropArea.TryFill(BaseExpression.new(baseKind)):
			SoundManager.Play(SFXType.JAR_POUR)
	if dropArea is DragAreaTeapotInside:
		if dropArea.TryFillTeapot(BaseExpression.new(baseKind)):
			SoundManager.Play(SFXType.JAR_POUR)
	print("BaseJar dropped on %s!" % (dropArea.name if dropArea != null else "<null>"))

func OnCancelDrag() -> void:
	print("BaseJar drag cancelled.")

