using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggableBaseJar : Node2D, IDraggable
{
    [Export] BaseKind baseKind;
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
        GD.Print("BaseJar picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            container.TryFill(new BaseExpression(baseKind));
        }
        GD.Print($"BaseJar dropped on {dropArea?.Name}!");
    }

    public void OnCancelDrag()
    {
        GD.Print("BaseJar drag cancelled.");
    }

    
}