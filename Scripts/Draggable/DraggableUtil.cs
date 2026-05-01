using Godot;
using System;

public static class DraggableUtil
{
    public const int DragZIndex = 100; // ZIndex to use for dragged items

    public static void DefaultDragBehavior(Node2D node, IDraggable draggable, double delta, Node2D returnArea, int zIndexMult = 10)
    {
        if (InputManager.Instance?.currentDragItem == draggable)
        {
            node.Position = node.Position.Lerp(node.GetGlobalMousePosition(), 20f * (float)delta);
            node.ZIndex = DragZIndex; // Ensure the dragged item is on top
        }
        else if (returnArea != null)
        {
            node.Position = node.Position.Lerp(returnArea.GlobalPosition, 10f * (float)delta);
            node.ZIndex = returnArea?.ZIndex*zIndexMult + 1 ?? 1; // Reset ZIndex when not being dragged
        }
    }
}