using Godot;
using System;
using RedteaGreenteaTea.Domain;

public partial class DraggableCup : Node2D, IDraggableContained
{
    [Export] Sprite2D liquidSprite;
    [Export] HoverHighlightable hoverHighlight;

    DragAreaContainer returnArea;

    bool hasContent = false;
    ProductExpression liquidContent;

    public override void _Process(double delta)
    {
        base._Process(delta);
        DraggableUtil.DefaultDragBehavior(this, this, delta, returnArea);

        liquidSprite.Visible = hasContent;
        if (hasContent)
        {
            liquidSprite.Modulate = liquidContent.Color.ToGodotColor();
        }
    }

    public void OnPick()
    {
        GD.Print("Cup picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"Cup dropped on {dropArea?.Name}!");
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            if (container.TryDropDraggable(this))
            {
                returnArea = container;
                GD.Print("Cup successfully dropped into container.");
            }
            else if (hasContent && container.TryFill(liquidContent))
            {
                hasContent = false;
                liquidContent = null;
                returnArea = container;
                ReturnToOriginalPosition();
                GD.Print("Cup successfully poured into container.");
            }
            else
            {
                GD.Print("Failed to drop cup into container. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        if (dropArea is DragAreaTeapot)
        {
            var teapotArea = dropArea as DragAreaTeapot;
            if (hasContent && teapotArea.TryFill(liquidContent))
            {
                hasContent = false;
                liquidContent = null;
                ReturnToOriginalPosition();
                GD.Print("Cup successfully poured into teapot.");
            }
            else
            {
                GD.Print("Failed to pour cup into teapot. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        ReturnToOriginalPosition();
    }

    public void OnCancelDrag()
    {
        GD.Print("Cup drag cancelled. Returning to original position.");
        ReturnToOriginalPosition();
    }

    void Destroy()
    {
        GD.Print("Cup destroyed.");
        QueueFree();
    }

    void ReturnToOriginalPosition()
    {
        if (returnArea == null)
        {
            GD.Print("No return area set. Destroying cup.");
            Destroy();
            return;
        }
        if (returnArea.TryDropDraggable(this))
        {
            GD.Print("Cup returned to original position.");
        }
        else
        {
            GD.Print("Failed to return cup to original position. Destroying cup.");
            Destroy();
        }
    }

    public bool TryFill(ProductExpression liquid)
    {
        if (!liquid.Is(ProductCategory.Liquid))
        {
            GD.Print("Cannot fill cup with non-liquid product.");
            return false;
        }
        if (hasContent)
        {
            GD.Print("Cup already has content! Cannot fill.");
            return false;
        }
        liquidContent = liquid;
        hasContent = true;
        GD.Print($"Cup filled with {liquid.DisplayName}.");
        return true;
    }

    public HoverHighlightable GetHoverHighlight()
    {
        return hoverHighlight;
    }
}
