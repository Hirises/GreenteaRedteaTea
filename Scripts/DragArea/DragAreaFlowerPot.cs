using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class DragAreaFlowerPot : DragArea
{
    [Export] FlowerPot flowerPot;

    public override IDraggable GetDraggable()
    {
        return flowerPot.PickLeaf();
    }

    public bool TryBloom(DraggableLeaf leaf)
    {
        var leafContent = leaf.GetLeafContent();
        return flowerPot.TryBloom(leafContent);
    }
}
