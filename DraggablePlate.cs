using Godot;
using System;

public partial class DraggablePlate : Node2D, IDraggable
{
    DragAreaContainer returnArea;

    public override void _Process(double delta)
    {
        if (InputManager.Instance?.currentDragItem == this)
        {
            Position = Position.Lerp(GetGlobalMousePosition(), 20f * (float)delta);
            ZIndex = 2; // Ensure the dragged item is on top
        }
        else if (returnArea != null)
        {
            Position = Position.Lerp(returnArea.GlobalPosition, 10f * (float)delta);
            ZIndex = 1; // Reset ZIndex when not being dragged
        }
    }

    public void OnPick()
    {
        GD.Print("Plate picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"Plate dropped on {dropArea?.Name}!");
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            if (container.TryDropDraggable(this))
            {
                returnArea = container;
                GD.Print("Plate successfully dropped into container.");
            }
            else
            {
                GD.Print("Failed to drop plate into container. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        Destroy();
    }

    public void OnCancelDrag()
    {
        GD.Print("Plate drag cancelled. Returning to original position.");
        ReturnToOriginalPosition();
    }

    void Destroy()
    {
        GD.Print("Plate destroyed.");
        QueueFree();
    }

    void ReturnToOriginalPosition()
    {
        if (returnArea == null)
        {
            GD.Print("No return area set. Destroying plate.");
            Destroy();
            return;
        }
        if (returnArea.TryDropDraggable(this))
        {
            GD.Print("Plate returned to original position.");
        }
        else
        {
            GD.Print("Failed to return plate to original position. Destroying plate.");
            Destroy();
        }
    }
}
