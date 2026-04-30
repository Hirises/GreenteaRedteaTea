using Godot;
using System;

public partial class DraggableLeaf : Node2D, IDraggable
{
    public override void _Process(double delta)
    {
        if (InputManager.Instance?.currentDragItem == this)
        {
            Position = GetGlobalMousePosition();
        }
    }

    public void OnPick()
    {
        GD.Print("Leaf picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"Leaf dropped on {dropArea?.Name}!");
        QueueFree();
    }

    public void OnCancelDrag()
    {
        GD.Print("Leaf drag cancelled.");
        QueueFree();
    }
}