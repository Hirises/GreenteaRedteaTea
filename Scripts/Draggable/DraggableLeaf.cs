using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggableLeaf : Node2D, IDraggableContained
{
    [Export] Sprite2D leafSprite;
    [Export] HoverHighlightable hoverHighlight;
    IDragAreaContainer returnArea;
    ProductExpression leafContent;

    public void Initialize(BasicLeafKind kind)
    {
        leafContent = new BasicLeafExpression(kind);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        bool contained = returnArea is DragAreaPlate or DragAreaTeapotInside;
        DraggableUtil.DefaultDragBehavior(this, this, delta, returnArea?.GetNode(),
            contained ? 1 : 10, contained ? 90f : 20f);

        if (leafContent == null)
        {
            GD.PrintErr("Leaf content is null! This should not happen.");
            return;
        }

        leafSprite.Modulate = leafContent.Color.ToGodotColor();
    }

    public void OnPick()
    {
        GD.Print("Leaf picked up!");
    }

    public void OnDrop(DragArea dropArea)
    {
        GD.Print($"Leaf dropped on {dropArea?.Name}!");
        if (dropArea is DragAreaContainer)
        {
            var container = dropArea as DragAreaContainer;
            if (container.TryDropDraggable(this))
            {
                returnArea = container;
                GD.Print("Leaf successfully dropped into container.");
            }
            else if (container.TryPutLeafOnPlate(this))
            {
                returnArea = container.GetPlateDragArea() as DragAreaPlate;
                GD.Print("Leaf successfully put on plate.");
            }
            else
            {
                GD.Print("Failed to drop leaf into container. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        else if (dropArea is DragAreaTeapot)
        {
            var teapot = dropArea as DragAreaTeapot;
            if (teapot.TryPutLeafInTeapot(this))
            {
                returnArea = teapot.GetInsideArea();
                GD.Print("Leaf successfully put in teapot.");
            }
            else
            {
                GD.Print("Failed to put leaf in teapot. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        ReturnToOriginalPosition();
    }

    public void OnCancelDrag()
    {
        GD.Print("Leaf drag cancelled. Returning to original position.");
        ReturnToOriginalPosition();
    }

    public void Destroy()
    {
        GD.Print("Leaf destroyed.");
        QueueFree();
    }

    void ReturnToOriginalPosition()
    {
        if (returnArea == null)
        {
            GD.Print("No return area set. Destroying leaf.");
            Destroy();
            return;
        }
        if (returnArea.TryDropDraggable(this))
        {
            GD.Print("Leaf returned to original position.");
        }
        else
        {
            GD.Print("Failed to return leaf to original position. Destroying leaf.");
            Destroy();
        }
    }

    public HoverHighlightable GetHoverHighlight()
    {
        return hoverHighlight;
    }

    public ProductExpression GetLeafContent()
    {
        if (leafContent == null)
        {
            GD.PrintErr("Leaf content is null! This should not happen.");
            return null;
        }
        return leafContent;
    }

    public void SetLeafContent(ProductExpression content)
    {
        if (content == null)
        {
            GD.PrintErr("Cannot set leaf content to null!");
            return;
        }
        if (!content.Is(ProductCategory.Leaf))
        {
            GD.PrintErr("Cannot set leaf content to non-leaf product!");
            return;
        }
        leafContent = content;
    }
}
