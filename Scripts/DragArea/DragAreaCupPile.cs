using Godot;
using System;

public partial class DragAreaCupPile : DragArea
{
    [Export] PackedScene cupScene;

    public override IDraggable GetDraggable()
    {
        var cup = cupScene.Instantiate();
        AddSibling(cup);
        (cup as Node2D).Position = GlobalPosition; // Start the cup at the pile's position
        return cup as IDraggable;
    }
}