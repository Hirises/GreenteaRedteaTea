class_name SoundManager
extends Node

static var Instance: SoundManager
var soundDictionary := {}

@export var bgmReady: AudioStreamPlayer
@export var bgmIngame: AudioStreamPlayer

func _ready() -> void:
	Instance = self
	for node in get_children():
		if node is SoundPlayer:
			soundDictionary[node.sfxType] = node
	bgmReady.play()

func _exit_tree() -> void:
	bgmReady.stop()
	bgmIngame.stop()

static func Play(sfx_type: int) -> void:
	if Instance != null:
		Instance._play_internal(sfx_type)

static func Stop(sfx_type: int) -> void:
	if Instance != null:
		Instance._stop_internal(sfx_type)

func _play_internal(sfx_type: int) -> void:
	if soundDictionary.has(sfx_type):
		soundDictionary[sfx_type].Play()

func _stop_internal(sfx_type: int) -> void:
	if soundDictionary.has(sfx_type):
		soundDictionary[sfx_type].Stop()

func PlayIngameBGM() -> void:
	bgmIngame.play()
	bgmReady.stop()

static func PlayHourglassFlip() -> void:
	Play(SFXType.HOURGLASS_FLIP)

static func PlayHourglassLand() -> void:
	Play(SFXType.HOURGLASS_LAND)

