using Godot;
using System;

public partial class DragAreaLeafJar : DragArea
{
    [Export] PackedScene leafScene;

    public override IDraggable GetDraggable()
    {
        var leaf = leafScene.Instantiate();
        AddSibling(leaf);
        (leaf as Node2D).Position = GlobalPosition; // Start the leaf at the jar's position
        return leaf as IDraggable;
    }
}
