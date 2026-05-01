using Godot;
using System;

public static class DraggableUtil
{
    public const int DragZIndex = 100; // ZIndex to use for dragged items

    public static void DefaultDragBehavior(Node2D node, IDraggable draggable, double delta,
                    Node2D returnArea, int zIndexMult = 10, float weight = 10f)
    {
        if (InputManager.Instance?.currentDragItem == draggable)
        {
            float w = Mathf.Clamp(20f * (float)delta, 0f, 1f);
            node.Position = node.Position.Lerp(node.GetGlobalMousePosition(), w);
            node.ZIndex = DragZIndex; // Ensure the dragged item is on top
        }
        else if (returnArea != null)
        {
            float w = Mathf.Clamp(weight * (float)delta, 0f, 1f);
            node.Position = node.Position.Lerp(returnArea.GlobalPosition, w);
            node.ZIndex = returnArea?.ZIndex*zIndexMult + 1 ?? 1; // Reset ZIndex when not being dragged
        }
    }
}