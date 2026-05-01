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
        if (draggable is not DraggableLeaf)
        {
            GD.Print("Only leaves can be placed on the plate!");
            return false;
        }
        currentDraggable = draggable as DraggableLeaf;
        SetHoverHighlight(currentDraggable.GetHoverHighlight());
        
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

    public bool TryMergeLeaf(DraggableLeaf newLeaf)
    {
        if (currentDraggable == null)
        {
            GD.Print("No existing leaf to merge with on the plate!");
            return false;
        }

        var existingLeaf = currentDraggable;
        var mix = new CombinedLeafExpression(existingLeaf.GetLeafContent(), newLeaf.GetLeafContent());
        existingLeaf.SetLeafContent(mix);
        GD.Print($"Merged leaf on plate to create {mix.DisplayName}.");
        return true;
    }
}