class_name SoundPlayer
extends Node

@export var sfxType: int = SFXType.NONE
@export var player: AudioStreamPlayer


func Play() -> void:
	player.pitch_scale = 1.0 + randf() * 0.2
	player.play()

func Stop() -> void:
	player.stop()

