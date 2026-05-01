using Godot;
using System;

public partial class DragAreaContainer : DragArea
{
    IDraggable currentDraggable;

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
        currentDraggable = draggable;
        return true;
    }
}