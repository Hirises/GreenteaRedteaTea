class_name TimerGraph
extends Node2D

@export var graphSprite: Sprite2D
var graphOriginalPosition := Vector2.ZERO

func _ready() -> void:
	graphOriginalPosition = graphSprite.position
	graphSprite.region_enabled = true

func OnOrderTimerChanged(remainingSeconds: float, timeLimitSeconds: float) -> void:
	SetGraphFill(remainingSeconds / timeLimitSeconds)

func SetGraphFill(fillAmount: float) -> void:
	var texture_size := graphSprite.texture.get_size()
	var current_size := texture_size * graphSprite.scale
	graphSprite.region_rect = Rect2(0, texture_size.y * (1.0 - fillAmount), texture_size.x, texture_size.y * fillAmount)
	graphSprite.position = graphOriginalPosition + Vector2(0, current_size.y * (1.0 - fillAmount) / 2.0)
