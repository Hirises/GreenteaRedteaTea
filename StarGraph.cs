using Godot;
using System;

public partial class StarGraph : Node2D
{
    [Export] Sprite2D graphSprite;
    [Export] GameManager gameManager;

    float currentFill = 0f;

    public override void _Ready()
    {
        graphSprite.RegionEnabled = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        currentFill = Mathf.Lerp(currentFill, gameManager.rating/10f, 10f * (float)delta);
        SetGraphFill(currentFill);
    }


    public void SetGraphFill(float fillAmount)
    {
        var textureSize = graphSprite.Texture.GetSize();
        graphSprite.RegionRect = new Rect2(
            0, 0, textureSize.X * fillAmount, textureSize.Y);
    }
}