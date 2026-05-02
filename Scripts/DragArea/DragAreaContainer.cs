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

    public bool TryMergeLeaf(DraggableLeaf leaf)
    {
        if (currentDraggable == null)
        {
            GD.Print("No draggable item in container to merge leaf with!");
            return false;
        }

        if (currentDraggable is DraggableLeaf)
        {
            var existingLeaf = currentDraggable as DraggableLeaf;
            var mix = new CombinedLeafExpression(existingLeaf.GetLeafContent(), leaf.GetLeafContent());
            existingLeaf.SetLeafContent(mix);
            GD.Print($"Merged leaf with existing leaf in container to create {mix.DisplayName}.");
            existingLeaf.Shake();
            return true;
        }

        if (currentDraggable is DraggablePlate)
        {
            var plate = currentDraggable as DraggablePlate;
            return plate.TryMergeLeaf(leaf);
        }

        GD.Print("Cannot merge leaf!");
        return false;
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

    public override string GetTooltipText()
    {
        if (currentDraggable == null)
        {
            return "";
        }
        if (currentDraggable is DraggableLeaf)
        {
            var leaf = currentDraggable as DraggableLeaf;
            var leafContent = leaf.GetLeafContent();
            return leafContent.DisplayName;
        }
        if (currentDraggable is DraggableCup)
        {
            var cup = currentDraggable as DraggableCup;
            if (cup.HasContent)
                return cup.LiquidContent.DisplayName;
            return "";
        }
        if (currentDraggable is DraggablePlate)
        {
            var plate = currentDraggable as DraggablePlate;
            if (plate.DragArea.HasLeaf())
                return plate.DragArea.GetLeaf().GetLeafContent().DisplayName;
            return "";
        }
        return "";
    }

    public override bool CanDrag()
    {
        return currentDraggable != null;
    }
}