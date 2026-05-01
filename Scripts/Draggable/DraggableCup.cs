using Godot;
using System;
using RedteaGreenteaTea.Domain;

public partial class DraggableCup : Node2D, IDraggable
{
    [Export] Sprite2D liquidSprite;

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
            else
            {
                GD.Print("Failed to drop cup into container. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        Destroy();
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

    public void Fill(ProductExpression liquid)
    {
        if (!liquid.Is(ProductCategory.Liquid))
        {
            GD.Print("Cannot fill cup with non-liquid product.");
            return;
        }
        liquidContent = liquid;
        hasContent = true;
        GD.Print($"Cup filled with {liquid.DisplayName}.");
    }
}
