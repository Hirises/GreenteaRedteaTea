class_name DragAreaTrash
extends DragArea

@export var animationPlayer: AnimationPlayer

func GetDraggable():
	return null

func OnTrash() -> void:
	animationPlayer.play("trash")
	SoundManager.Play(SFXType.TRASHBIN)

func CanDrag() -> bool:
	return false
