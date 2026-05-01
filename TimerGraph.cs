using Godot;
using System;

public partial class TimerGraph : Node2D
{
    [Export] Sprite2D graphSprite;

    Vector2 graphOriginalPosition;


    public void OnOrderTimerChanged(float remainingSeconds, float timeLimitSeconds)
    {
        SetGraphFill(remainingSeconds / timeLimitSeconds);
    }

    public void SetGraphFill(float fillAmount)
    {
        var textureSize = graphSprite.Texture.GetSize();
        var currentSize = textureSize * graphSprite.Scale;
        graphSprite.RegionRect = new Rect2(
            0, textureSize.Y * (1f - fillAmount),
            textureSize.X, textureSize.Y * fillAmount);
        // godot 스프라이트 원점은 중심이므로 /2 해줘야 함
        graphSprite.Position = graphOriginalPosition + new Vector2(0, currentSize.Y * (1f - fillAmount) / 2);
    }
}