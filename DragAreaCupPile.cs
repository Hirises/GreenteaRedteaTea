using Godot;
using System;

public partial class DragAreaCupPile : DragArea
{
    [Export] PackedScene cupScene;

    public override IDraggable GetDraggable()
    {
        var cup = cupScene.Instantiate();
        AddSibling(cup);
        return cup as IDraggable;
    }
}