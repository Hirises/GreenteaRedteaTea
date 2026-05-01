using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaTeapotInside : DragArea, IDragAreaContainer
{
    [Export] DragAreaTeapot teapot;
    DraggableLeaf currentDraggable;

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

    public bool HasLeaf()
    {
        return currentDraggable != null;
    }

    public ProductExpression GetLeaf()
    {
        if (currentDraggable == null)
        {
            GD.Print("No leaf in teapot inside to get!");
            return null;
        }

        if (currentDraggable is not DraggableLeaf)
        {
            GD.Print("Current draggable in teapot inside is not a leaf!");
            return null;
        }

        var leaf = currentDraggable;
        return leaf.GetLeafContent();
    }

    public void SetLeaf(ProductExpression leafContent)
    {
        if (currentDraggable == null)
        {
            GD.Print("No leaf in teapot inside to set content of!");
            return;
        }

        if (currentDraggable is not DraggableLeaf)
        {
            GD.Print("Current draggable in teapot inside is not a leaf!");
            return;
        }

        var leaf = currentDraggable;
        leaf.SetLeafContent(leafContent);
    }

    public bool TryFillTeapot(ProductExpression liquid)
    {
        return teapot.TryFill(liquid);
    }

    public override string GetTooltipText()
    {
        if (currentDraggable == null)
        {
            return "";
        }
        var leafContent = currentDraggable.GetLeafContent();
        return leafContent.DisplayName;
    }
}