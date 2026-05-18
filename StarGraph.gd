class_name StarGraph
extends Node2D

@export var graphSprite: Sprite2D
@export var gameManager: GameManager
var currentFill := 0.0

func _ready() -> void:
	graphSprite.region_enabled = true

func _process(delta: float) -> void:
	currentFill = lerpf(currentFill, gameManager.rating / 10.0, 10.0 * delta)
	SetGraphFill(currentFill)

func SetGraphFill(fillAmount: float) -> void:
	var texture_size := graphSprite.texture.get_size()
	graphSprite.region_rect = Rect2(0, 0, texture_size.x * fillAmount, texture_size.y)
