class_name Title
extends Node2D

@export var shutterLogoAnimation: AnimationPlayer
@export var mainScene: PackedScene
@export var shutterDown: SoundPlayer
@export var shutterUp: SoundPlayer

enum State { TITLE, GAME, GAME_OVER }
var state := State.TITLE
var mainSceneInst: MainScene
var gameManager: GameManager

func _unhandled_input(event: InputEvent) -> void:
	if state != State.TITLE:
		return
	if event is InputEventMouseButton and !event.pressed:
		mainSceneInst = mainScene.instantiate()
		add_child(mainSceneInst)
		gameManager = mainSceneInst.gameManager
		shutterLogoAnimation.play("open")
		state = State.GAME
		shutterUp.Play()

func _process(_delta: float) -> void:
	if state != State.GAME:
		return
	if gameManager.gameover:
		OnGameOver()

func OnGameOver() -> void:
	shutterLogoAnimation.play("close")
	state = State.GAME_OVER
	shutterDown.Play()

func OnGameOverAnimationEnd() -> void:
	mainSceneInst.queue_free()
	mainSceneInst = mainScene.instantiate()
	add_child(mainSceneInst)
	gameManager = mainSceneInst.gameManager
	state = State.GAME
	shutterLogoAnimation.play("reopen")
	shutterUp.Play()
