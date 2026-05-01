using Godot;
using System;

public partial class DragAreaTeapot : DragArea
{
    [Export] DraggableTeapot draggableTeapot;
    [Export] DragAreaTeapotInside insideArea;
    public DragAreaTeapotInside InsideArea => insideArea;

    public override IDraggable GetDraggable()
    {
        return draggableTeapot;
    }
}