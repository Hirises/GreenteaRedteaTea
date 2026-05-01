using Godot;
using RedteaGreenteaTea.Domain;
using System;

public partial class FlowerPot : Node2D
{
    [Export] AnimationPlayer animPlayer;
    [Export] Node2D tree1;
    [Export] Node2D tree2;
    [Export] Sprite2D deco1;
    [Export] Sprite2D deco2;
    [Export] Sprite2D[] leafSprites1;
    [Export] Sprite2D[] leafSprites2;
    [Export] PackedScene draggableLeafScene;
    [Export] HoverHighlightable hoverHighlight;
    public HoverHighlightable HoverHighlight => hoverHighlight;
    bool isLeaf1 = true;
    bool bloomed = false;
    public bool Bloomed => bloomed;
    int bloomCount = 0;

    [Export] float bloomTimeSeconds = 5f;
    float bloomTimer = 0f;

    ProductExpression leafContent;

    public override void _Ready()
    {
        tree1.Visible = false;
        tree2.Visible = false;
    }

    public bool TryBloom(ProductExpression leafContent)
    {
        if (leafContent == null || !leafContent.Is(ProductCategory.Leaf))
        {
            GD.PushError("FlowerPot can only bloom with a leaf product.");
            return false;
        }

        if (bloomed)
        {
            GD.PushWarning("FlowerPot is already bloomed. Ignoring additional bloom.");
            return false;
        }

        isLeaf1 = Random.Shared.Next(2) == 0;
        bloomed = true;
        bloomCount = 1;
        bloomTimer = 0f;
        this.leafContent = leafContent;

        var tree = isLeaf1 ? tree1 : tree2;
        var leafSprites = isLeaf1 ? leafSprites1 : leafSprites2;
        var deco = isLeaf1 ? deco1 : deco2;
        tree.Visible = true;
        var leafColor = leafContent.Color.ToGodotColor();
        foreach (var leaf in leafSprites)
        {
            leaf.Modulate = leafColor;
        }
        deco.Modulate = leafColor;
        animPlayer.Play("bloom");
        return true;
    }

    public DraggableLeaf PickLeaf()
    {
        if (!bloomed)
        {
            GD.PushWarning("FlowerPot has no leaves to pick.");
            return null;
        }

        bloomCount -= 1;
        OnBloomCountChanged();
        if (bloomCount <= 0)
        {
            bloomed = false;
            animPlayer.Play("disappear");
        }

        var leaf = draggableLeafScene.Instantiate<DraggableLeaf>();
        AddSibling(leaf);
        leaf.Position = GlobalPosition; // Start at the flower pot's position
        leaf.SetLeafContent(leafContent);
        return leaf;
    }

    public override void _Process(double delta)
    {
        if (bloomed)
        {
            bloomTimer += (float)delta;
            if (bloomCount < 3 && bloomTimer >= bloomTimeSeconds)
            {
                bloomTimer = 0f;
                bloomCount += 1;
                OnBloomCountChanged();
            }

            var leafs = isLeaf1 ? leafSprites1 : leafSprites2;
            int i;
            for (i = 0; i < bloomCount; i++)
            {
                var leaf = leafs[i];
                leaf.Visible = true;
            }
            for (; i < leafs.Length; i++)
            {
                leafs[i].Visible = false;
            }
        }
        else
        {
            var leafs = isLeaf1 ? leafSprites1 : leafSprites2;
            for (int i = 0; i < leafs.Length; i++)
            {
                leafs[i].Visible = false;
            }
        }
    }

    public void OnBloomCountChanged()
    {
        animPlayer.Play("shake");
    }
}
