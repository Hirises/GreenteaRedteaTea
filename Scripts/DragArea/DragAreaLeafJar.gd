class_name DragAreaLeafJar
extends DragArea

@export var leafScene: PackedScene
@export_enum("Green", "Black") var leafKind: int = BasicLeafKind.GREEN

func GetDraggable():
	var leaf = leafScene.instantiate()
	add_sibling(leaf)
	leaf.position = global_position
	leaf.Initialize(leafKind)
	SoundManager.Play(SFXType.LEAF_JAR_PICK)
	return leaf

func GetTooltipText() -> String:
	return BasicLeafExpression.new(leafKind).display_name
