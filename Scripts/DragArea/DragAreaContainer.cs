using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaContainer : DragArea, IDragAreaContainer
{
    IDraggableContained currentDraggable;

    public Node2D GetNode()
    {
        return this;
    }

    public override IDraggable GetDraggable()
    {
        var draggable = currentDraggable;
        currentDraggable = null;
        SetHoverHighlight(null);
        return draggable;
    }

    public bool TryDropDraggable(IDraggable draggable)
    {
        if (currentDraggable != null)
        {
            GD.Print("Container already has a draggable item!");
            return false;
        }
        if (draggable is not IDraggableContained)
        {
            GD.Print("Only draggable items that can be contained can be placed in the container!");
            return false;
        }
        currentDraggable = draggable as IDraggableContained;
        SetHoverHighlight(currentDraggable.GetHoverHighlight());
        return true;
    }

    public bool TryFill(ProductExpression liquid)
    {
        if (currentDraggable == null)
        {
            GD.Print("No draggable item in container to fill!");
            return false;
        }

        if (currentDraggable is not DraggableCup)
        {
            GD.Print("Only cups can be filled in the container!");
            return false;
        }

        var cup = currentDraggable as DraggableCup;
        return cup.TryFill(liquid);
    }

    public bool TryPutLeafOnPlate(DraggableLeaf leaf)
    {
        if (currentDraggable == null)
        {
            GD.Print("No draggable item in container to put leaf on!");
            return false;
        }

        if (currentDraggable is not DraggablePlate)
        {
            GD.Print("Only plates can have leaves put on them in the container!");
            return false;
        }

        var plate = currentDraggable as DraggablePlate;
        return plate.TryPutOnPlate(leaf);
    }

    public DragArea GetPlateDragArea()
    {
        if (currentDraggable is DraggablePlate)
        {
            var plate = currentDraggable as DraggablePlate;
            return plate.DragArea;
        }
        return null;
    }
}