using Godot;
using System;

public partial class TimerGraph : Node2D
{
    [Export] Sprite2D graphSprite;

    Vector2 graphOriginalPosition;

    float timeLeft = 30f;
    float totalTime = 30f;

    public override void _Ready()
    {
        graphSprite.RegionEnabled = true;
        graphOriginalPosition = graphSprite.Position;
    }

    public override void _Process(double delta)
    {
        timeLeft -= (float)delta;
        UpdateGraph(timeLeft, totalTime);
    }

    public void UpdateGraph(float timeLeft, float totalTime)
    {
        float fillAmount = Mathf.Clamp(timeLeft / totalTime, 0f, 1f);
        var textureSize = graphSprite.Texture.GetSize();
        var currentSize = textureSize * graphSprite.Scale;
        graphSprite.RegionRect = new Rect2(
            0, textureSize.Y * (1f - fillAmount),
            textureSize.X, textureSize.Y * fillAmount);
        // godot 스프라이트 원점은 중심이므로 /2 해줘야 함
        graphSprite.Position = graphOriginalPosition + new Vector2(0, currentSize.Y * (1f - fillAmount) / 2);
    }
}