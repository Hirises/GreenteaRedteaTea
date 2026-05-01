using Godot;
using System;

public partial class DraggableTeapot : Node2D, IDraggable
{
    Vector2 originalPosition;

    public override void _Process(double delta)
    {
        if (InputManager.Instance?.currentDragItem == this)
        {
            Position = Position.Lerp(GetGlobalMousePosition(), 20f * (float)delta);
            ZIndex = DraggableUtil.DragZIndex; // Ensure the dragged item is on top
        }
        else
        {
            Position = Position.Lerp(originalPosition, 10f * (float)delta);
            ZIndex = 0; // Reset ZIndex when not being dragged
        }
    }

    public override void _Ready()
    {
        originalPosition = Position;
    }

    public void OnPick()
    {
        GD.Print("Teapot picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"Teapot dropped on {dropArea?.Name}!");
    }

    public void OnCancelDrag()
    {
        GD.Print("Teapot drag cancelled.");
    }
}