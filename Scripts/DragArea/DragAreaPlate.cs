using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaPlate : DragArea, IDragAreaContainer
{
    DraggableLeaf currentDraggable;

    public Node2D GetNode()
    {
        return this;
    }

    public override IDraggable GetDraggable()
    {
        var draggable = currentDraggable;
        currentDraggable = null;
        return draggable;
    }

    public bool TryDropDraggable(IDraggable draggable)
    {
        if (currentDraggable != null)
        {
            GD.Print("Container already has a draggable item!");
            return false;
        }
        if (draggable is not DraggableLeaf)
        {
            GD.Print("Only leaves can be placed on the plate!");
            return false;
        }
        currentDraggable = draggable as DraggableLeaf;
        SetHoverHighlight(currentDraggable.HoverHighlight);
        
        return true;
    }

    public bool HasLeaf()
    {
        return currentDraggable != null;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (currentDraggable != null)
        {
            GD.Print("Plate removed from scene while it still had a leaf on it! Destroying the leaf as well.");
            currentDraggable.Destroy();
        }
    }
}