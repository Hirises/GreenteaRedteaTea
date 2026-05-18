class_name FlowerPot
extends Node2D

@export var animPlayer: AnimationPlayer
@export var tree1: Node2D
@export var tree2: Node2D
@export var deco1: Sprite2D
@export var deco2: Sprite2D
@export var leafSprites1: Array[Sprite2D]
@export var leafSprites2: Array[Sprite2D]
@export var draggableLeafScene: PackedScene
@export var LeafRoot: Node
@export var bloomTimeSeconds: float = 5.0

var isLeaf1 := true
var bloomed := false
var Bloomed: bool:
	get: return bloomed
var bloomCount := 0
var bloomTimer := 0.0
var leafContent: ProductExpression
var LeafContent: ProductExpression:
	get: return leafContent

func _ready() -> void:
	tree1.visible = false
	tree2.visible = false

func TryBloom(p_leafContent: ProductExpression) -> bool:
	if p_leafContent == null or !p_leafContent.is_category(ProductCategory.LEAF):
		push_error("FlowerPot can only bloom with a leaf product.")
		return false
	if bloomed:
		push_warning("FlowerPot is already bloomed. Ignoring additional bloom.")
		return false
	isLeaf1 = randi_range(0, 1) == 0
	bloomed = true
	bloomCount = 1
	bloomTimer = 0.0
	leafContent = p_leafContent
	var tree = tree1 if isLeaf1 else tree2
	var leaf_sprites = leafSprites1 if isLeaf1 else leafSprites2
	var deco = deco1 if isLeaf1 else deco2
	tree1.visible = false
	tree2.visible = false
	tree.visible = true
	var leaf_color := leafContent.color.to_godot_color()
	for leaf in leaf_sprites:
		leaf.modulate = leaf_color
	deco.modulate = leaf_color
	animPlayer.play("bloom")
	SoundManager.Play(SFXType.TREE_BLOOM)
	return true

func PickLeaf() -> DraggableLeaf:
	if !bloomed:
		push_warning("FlowerPot has no leaves to pick.")
		return null
	if bloomCount <= 0:
		bloomed = false
		animPlayer.play("disappear")
		SoundManager.Play(SFXType.TREE_DIE)
		return null
	bloomCount -= 1
	OnBloomCountChanged()
	var leaf: DraggableLeaf = draggableLeafScene.instantiate()
	LeafRoot.add_child(leaf)
	leaf.position = global_position
	leaf.SetLeafContent(leafContent)
	SoundManager.Play(SFXType.TREE_PICK)
	return leaf

func _process(delta: float) -> void:
	var leafs = leafSprites1 if isLeaf1 else leafSprites2
	if bloomed:
		bloomTimer += delta
		if bloomCount < 3 and bloomTimer >= bloomTimeSeconds:
			bloomTimer = 0.0
			bloomCount += 1
			OnBloomCountChanged()
			SoundManager.Play(SFXType.TREE_GROW)
		for i in range(leafs.size()):
			leafs[i].visible = i < bloomCount
	else:
		for leaf in leafs:
			leaf.visible = false

func OnBloomCountChanged() -> void:
	animPlayer.play("shake")
