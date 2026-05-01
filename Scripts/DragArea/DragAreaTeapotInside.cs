using Godot;
using System;

public partial class DragAreaTeapotInside : DragArea, IDragAreaContainer
{
    DraggableLeaf currentDraggable;

    public override IDraggable GetDraggable()
    {
        return currentDraggable;
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
            GD.Print("Only leaves can be placed on the teapot inside!");
            return false;
        }
        currentDraggable = draggable as DraggableLeaf;
        SetHoverHighlight(currentDraggable.GetHoverHighlight());
        
        return true;
    }

    public Node2D GetNode()
    {
        return this;
    }
}