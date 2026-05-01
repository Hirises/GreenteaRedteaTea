using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaTeapot : DragArea
{
    [Export] DraggableTeapot draggableTeapot;
    [Export] DragAreaTeapotInside insideArea;
    public DragAreaTeapotInside InsideArea => insideArea;

    public override void _Process(double delta)
    {
        base._Process(delta);

        insideArea.Visible = insideArea.HasLeaf() && InputManager.Instance.currentDragItem != draggableTeapot;
        insideArea.ZIndex = draggableTeapot.ZIndex;
    }

    public override IDraggable GetDraggable()
    {
        return draggableTeapot;
    }

    public DragAreaTeapotInside GetInsideArea()
    {
        return insideArea;
    }

    public bool TryPutLeafInTeapot(DraggableLeaf draggable)
    {
        return insideArea.TryDropDraggable(draggable);
    }

    public bool TryFill(ProductExpression liquid)
    {
        if (!liquid.Is(ProductCategory.Liquid))
        {
            GD.Print("Only liquids can be filled into the teapot!");
            return false;
        }

        draggableTeapot.Fill(liquid);
        return true;
    }
}