using Godot;
using RedteaGreenteaTea.Domain;
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

    public void TryFill(ProductExpression liquid)
    {
        if (currentDraggable == null)
        {
            GD.Print("No draggable item in container to fill!");
            return;
        }

        if (currentDraggable is not DraggableCup)
        {
            GD.Print("Only cups can be filled in the container!");
            return;
        }

        if (!liquid.Is(ProductCategory.Liquid))
        {
            GD.Print("Only liquids can be filled into the cup!");
            return;
        }

        var cup = currentDraggable as DraggableCup;
        cup.Fill(liquid);
    }
}