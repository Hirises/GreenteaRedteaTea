class_name HourglassAnim
extends Node

@export var animPlayer: AnimationPlayer
@export var hourglassSprite: Sprite2D
@export var hourglassTextures: Array[Texture2D]
var currentIndex := 0

func OnGameStart(_orderName: String, _timeLimitSeconds: float) -> void:
	PlayFlipAnim()
	currentIndex = 0

func OnOrderTimerChanged(remainingSeconds: float, timeLimitSeconds: float) -> void:
	var progress := 1.0 - (remainingSeconds / timeLimitSeconds)
	var texture_index := clampi(int(progress * (hourglassTextures.size() - 1)), 0, hourglassTextures.size() - 1)
	if texture_index != currentIndex:
		currentIndex = texture_index
		PlayProgressAnim(hourglassTextures[texture_index])

func PlayFlipAnim() -> void:
	animPlayer.play("flip_hourglass")

func PlayProgressAnim(hourglassTexture: Texture2D) -> void:
	animPlayer.play("hourglass_progress")
	hourglassSprite.texture = hourglassTexture
