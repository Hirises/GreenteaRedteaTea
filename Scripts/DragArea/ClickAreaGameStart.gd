class_name ClickAreaGameStart
extends ClickArea

@export var calendarAnimation: AnimationPlayer
@export var gameManager: GameManager
@export var closePanel: Sprite2D
@export var openPanel: Sprite2D
@export var numPanel10: Array[Sprite2D]
@export var numPanel01: Array[Sprite2D]
@export var gameStartAnimation: AnimationPlayer

var nextOpenPanel: Array[Sprite2D] = []
var prevScore := 0
enum State { BEFORE, ANIMATING, AFTER }
var state := State.BEFORE

func OnClick() -> void:
	if state != State.BEFORE:
		return
	state = State.ANIMATING
	nextOpenPanel.clear()
	nextOpenPanel.append(openPanel)
	calendarAnimation.play("flip_close")
	gameStartAnimation.play("game_start")
	SetHoverHighlight(null)
	SoundManager.Play(SFXType.CALENDAR_OPEN)
	SoundManager.Play(SFXType.SHOP_OPEN)

func OnOrderEnded(_result: int, _orderName: String) -> void:
	OnScoreChange(gameManager.score)

func OnScoreChange(score: int) -> void:
	nextOpenPanel.clear()
	if score == prevScore:
		return
	prevScore = score
	if score > 99:
		score = 99
	nextOpenPanel.append(numPanel01[score % 10])
	nextOpenPanel.append(numPanel10[score / 10])
	SoundManager.Play(SFXType.CALENDAR_FLIP)
	calendarAnimation.play("flip_close")

func OnAnimationEnd() -> void:
	if state == State.ANIMATING:
		state = State.AFTER
		gameManager.StartGame()

func OnChangePanel() -> void:
	closePanel.visible = false
	openPanel.visible = false
	for panel in numPanel01:
		panel.visible = false
	for panel in numPanel10:
		panel.visible = false
	for panel in nextOpenPanel:
		panel.visible = true

func CanClick() -> bool:
	return state == State.BEFORE
