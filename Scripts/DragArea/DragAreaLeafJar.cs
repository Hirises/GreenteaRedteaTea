using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaLeafJar : DragArea
{
    [Export] PackedScene leafScene;
    [Export] BasicLeafKind leafKind;

    public override IDraggable GetDraggable()
    {
        var leaf = leafScene.Instantiate();
        AddSibling(leaf);
        (leaf as Node2D).Position = GlobalPosition; // Start the leaf at the jar's position
        (leaf as DraggableLeaf).Initialize(leafKind); // Set the leaf's content based on the jar's leaf kind
        SoundManager.Play(SFXType.LeafJarPick);
        return leaf as IDraggable;
    }

    public override string GetTooltipText()
    {
        return new BasicLeafExpression(leafKind).DisplayName;
    }
}
