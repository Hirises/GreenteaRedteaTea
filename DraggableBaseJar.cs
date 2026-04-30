using Godot;
using System;

public partial class DraggableBaseJar : Node2D, IDraggable
{
    Vector2 originalPosition;

    public override void _Process(double delta)
    {
        if (InputManager.Instance?.currentDragItem == this)
        {
            Position = GetGlobalMousePosition();
            ZIndex = 2; // Ensure the dragged item is on top
        }
        else
        {
            Position = originalPosition;
            ZIndex = 0; // Reset ZIndex when not being dragged
        }
    }

    public override void _Ready()
    {
        originalPosition = Position;
    }

    public void OnPick()
    {
        GD.Print("BaseJar picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"BaseJar dropped on {dropArea?.Name}!");
    }

    public void OnCancelDrag()
    {
        GD.Print("BaseJar drag cancelled.");
    }
}