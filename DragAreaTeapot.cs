using Godot;
using System;

public partial class DragAreaTeapot : DragArea
{
    [Export] DraggableTeapot draggableTeapot;

    public override IDraggable GetDraggable()
    {
        return draggableTeapot;
    }
}