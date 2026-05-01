using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DraggableLeaf : Node2D, IDraggable
{
    [Export] Sprite2D leafSprite;
    DragAreaContainer returnArea;
    ProductExpression leafContent;

    public void Initialize(BasicLeafKind kind)
    {
        leafContent = new BasicLeafExpression(kind);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        DraggableUtil.DefaultDragBehavior(this, this, delta, returnArea);

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
            else
            {
                GD.Print("Failed to drop leaf into container. Returning to original position.");
                ReturnToOriginalPosition();
            }
            return;
        }
        Destroy();
    }

    public void OnCancelDrag()
    {
        GD.Print("Leaf drag cancelled. Returning to original position.");
        ReturnToOriginalPosition();
    }

    void Destroy()
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
}