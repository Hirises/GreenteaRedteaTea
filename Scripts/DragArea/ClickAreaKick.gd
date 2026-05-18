class_name ClickAreaKick
extends ClickArea

@export var gameManager: GameManager

func OnClick() -> void:
	SoundManager.Play(SFXType.KICK)
	gameManager.KickOutCustomer()

func CanClick() -> bool:
	return gameManager.CanKick()
