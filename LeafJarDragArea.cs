using Godot;
using System;

public partial class LeafJarDragArea : DragArea
{
    [Export] PackedScene leafScene;

    public override IDraggable GetDraggable()
    {
        var leaf = leafScene.Instantiate();
        AddSibling(leaf);
        return leaf as IDraggable;
    }
}