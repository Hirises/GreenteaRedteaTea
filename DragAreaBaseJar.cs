using Godot;
using System;

public partial class DragAreaBaseJar : DragArea
{
    [Export] DraggableBaseJar draggableJar;

    public override IDraggable GetDraggable()
    {
        return draggableJar;
    }
}