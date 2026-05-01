using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaBaseJar : DragArea
{
    [Export] DraggableBaseJar draggableJar;

    public override IDraggable GetDraggable()
    {
        return draggableJar;
    }

    public override string GetTooltipText()
    {
        return new BaseExpression(draggableJar.BaseKind).DisplayName;
    }
}