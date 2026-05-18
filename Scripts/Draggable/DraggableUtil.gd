class_name DraggableUtil

const DRAG_Z_INDEX := 100

static func DefaultDragBehavior(node: Node2D, draggable, delta: float, returnArea: Node2D, zIndexMult: int = 10, weight: float = 10.0) -> void:
	if InputManager.Instance.currentDragItem == draggable:
		var w := clampf(20.0 * delta, 0.0, 1.0)
		node.position = node.position.lerp(node.get_global_mouse_position(), w)
		node.z_index = DRAG_Z_INDEX
	elif returnArea != null:
		var w := clampf(weight * delta, 0.0, 1.0)
		node.position = node.position.lerp(returnArea.global_position, w)
		node.z_index = returnArea.z_index * zIndexMult + 1
