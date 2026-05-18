class_name CustomerUi
extends Node

@export var TextPath: NodePath = "Text"
@export var SpeechAnimation: AnimationPlayer
@export var MoveAnimation: AnimationPlayer
@export var SpecialPath: NodePath = "Special"
@export var HeadPath: NodePath = "Head"
@export var BodyPath: NodePath = "Body"
@export var ClothPath: NodePath = "Cloth"
@export var ExpressionPath: NodePath = "Expression"
@export var gameManager: GameManager

const CUSTOMER_SPRITE_DIRECTORY := "res://Sprites/Customers"
const SPECIAL_DIR := "Special"
const HEAD_DIR := "머리"
const BODY_DIR := "몸"
const CLOTH_DIR := "옷"
const EXPRESSION_DIR := "표정"
const DEFAULT_CUSTOMER_NAME := "Default"
const TEXTURE_EXTENSION := ".png"
const RARE_GREEN_BODY_TEXTURE := "사람_몸_녹색.png"
const DEFAULT_BODY_TEXTURE_WEIGHT := 20
const RARE_GREEN_BODY_TEXTURE_WEIGHT := 1

var special: Sprite2D
var head: Sprite2D
var body: Sprite2D
var cloth: Sprite2D
var expression: Sprite2D
var textLabel: Text
var _rand := RandomNumberGenerator.new()

func _ready() -> void:
	_rand.randomize()
	special = get_node_or_null(SpecialPath)
	head = get_node_or_null(HeadPath)
	body = get_node_or_null(BodyPath)
	cloth = get_node_or_null(ClothPath)
	expression = get_node_or_null(ExpressionPath)
	if special == null: push_error("CustomerUI needs a Special child at path: %s" % SpecialPath)
	if head == null: push_error("CustomerUI needs a Head child at path: %s" % HeadPath)
	if body == null: push_error("CustomerUI needs a Body child at path: %s" % BodyPath)
	if cloth == null: push_error("CustomerUI needs a Cloth child at path: %s" % ClothPath)
	if expression == null: push_error("CustomerUI needs an Expression child at path: %s" % ExpressionPath)
	FindTextLabel()

func setCustomer(customer: Customer) -> void:
	if customer == null:
		_ClearCustomerTextures()
		return
	if customer.isSpecial:
		_SetSpecialTexture(customer.Name)
		return
	_GenerateTexture(_GetCustomerSeed(customer))

func _SetSpecialTexture(customer_name_value: String) -> void:
	_ClearCustomerTextures()
	_SetVisible(special, true)
	var customer_name := DEFAULT_CUSTOMER_NAME if customer_name_value.strip_edges().is_empty() else customer_name_value
	var texture_path := "%s/%s/%s%s" % [CUSTOMER_SPRITE_DIRECTORY, SPECIAL_DIR, customer_name, TEXTURE_EXTENSION]
	if !ResourceLoader.exists(texture_path):
		var fallback_path := "%s/%s%s" % [CUSTOMER_SPRITE_DIRECTORY, DEFAULT_CUSTOMER_NAME, TEXTURE_EXTENSION]
		push_warning("Special customer texture not found: %s. Loading default customer texture." % texture_path)
		texture_path = fallback_path
	_SetTexture(special, texture_path)

func _GenerateTexture(seed: int) -> void:
	_ClearCustomerTextures()
	_SetVisible(head, true)
	_SetVisible(body, true)
	_SetVisible(cloth, true)
	_SetVisible(expression, true)
	_SetRandomBodyTexture(body, seed + int(_rand.randi()))
	_SetRandomLayerTexture(cloth, CLOTH_DIR, seed + int(_rand.randi()))
	_SetRandomLayerTexture(head, HEAD_DIR, seed + int(_rand.randi()))
	_SetRandomLayerTexture(expression, EXPRESSION_DIR, seed + int(_rand.randi()))

func _SetRandomBodyTexture(sprite: Sprite2D, seed: int) -> void:
	var textures := _GetTexturePaths("%s/%s" % [CUSTOMER_SPRITE_DIRECTORY, BODY_DIR])
	if textures.size() == 0:
		push_warning("Customer texture directory has no png files: %s/%s" % [CUSTOMER_SPRITE_DIRECTORY, BODY_DIR])
		_SetVisible(sprite, false)
		return
	_SetTexture(sprite, _SelectWeightedBodyTexture(textures, seed))

func _SelectWeightedBodyTexture(textures: Array[String], seed: int) -> String:
	var total_weight := 0
	for texture_path in textures:
		total_weight += _GetBodyTextureWeight(texture_path)
	var roll := _SelectIndex(seed, total_weight)
	for texture_path in textures:
		roll -= _GetBodyTextureWeight(texture_path)
		if roll < 0:
			return texture_path
	return textures[0]

func _GetBodyTextureWeight(texture_path: String) -> int:
	return RARE_GREEN_BODY_TEXTURE_WEIGHT if texture_path.to_lower().ends_with(("/" + RARE_GREEN_BODY_TEXTURE).to_lower()) else DEFAULT_BODY_TEXTURE_WEIGHT

func _SetRandomLayerTexture(sprite: Sprite2D, directory_name: String, seed: int) -> void:
	var textures := _GetTexturePaths("%s/%s" % [CUSTOMER_SPRITE_DIRECTORY, directory_name])
	if textures.size() == 0:
		push_warning("Customer texture directory has no png files: %s/%s" % [CUSTOMER_SPRITE_DIRECTORY, directory_name])
		_SetVisible(sprite, false)
		return
	_SetTexture(sprite, textures[_SelectIndex(seed, textures.size())])

func _SelectIndex(seed: int, count: int) -> int:
	return 0 if count <= 1 else abs(seed) % count

func _GetTexturePaths(directory_path: String) -> Array[String]:
	var paths: Array[String] = []
	for file_name in ResourceLoader.list_directory(directory_path):
		if !file_name.ends_with("/") and file_name.to_lower().ends_with(TEXTURE_EXTENSION):
			paths.append("%s/%s" % [directory_path, file_name])
	paths.sort()
	return paths

func _GetCustomerSeed(customer: Customer) -> int:
	var hash := 17
	hash = hash * 31 + customer.Number
	hash = _AddStableHash(hash, customer.Name)
	hash = _AddStableHash(hash, customer.GetCustomerClassName())
	return hash & 0x7fffffff

func _AddStableHash(hash: int, value: String) -> int:
	if value.is_empty():
		return hash
	for character in value:
		hash = hash * 31 + character.unicode_at(0)
	return hash

func _ClearCustomerTextures() -> void:
	_SetVisible(special, false)
	_SetVisible(head, false)
	_SetVisible(body, false)
	_SetVisible(cloth, false)
	_SetVisible(expression, false)
	_ClearTexture(special)
	_ClearTexture(head)
	_ClearTexture(body)
	_ClearTexture(cloth)
	_ClearTexture(expression)

func _SetTexture(sprite: Sprite2D, texture_path: String) -> void:
	if sprite == null or texture_path.is_empty():
		return
	if !ResourceLoader.exists(texture_path):
		push_error("Customer texture not found: %s" % texture_path)
		return
	var loaded_texture: Texture2D = ResourceLoader.load(texture_path)
	if loaded_texture == null:
		push_error("Failed to load customer texture: %s" % texture_path)
		return
	sprite.texture = loaded_texture

func _ClearTexture(sprite: Sprite2D) -> void:
	if sprite != null:
		sprite.texture = null

func _SetVisible(item: CanvasItem, visible_value: bool) -> void:
	if item != null:
		item.visible = visible_value

func sayText(value: String) -> void:
	if !FindTextLabel():
		push_error("CustomerUi cannot say text because Text node was not found at path: %s" % TextPath)
		return
	textLabel.setText(value)
	SpeechAnimation.play("speech_open")

func FindTextLabel() -> bool:
	if textLabel == null:
		textLabel = get_node_or_null(TextPath)
	return textLabel != null

func Appear() -> void:
	MoveAnimation.play("appear")

func Disappear() -> void:
	MoveAnimation.play("disappear")

func OnDisappear() -> void:
	gameManager.NextOrder()
