using Godot;
using System;

public partial class HourglassAnim : Node
{
	[Export] AnimationPlayer animPlayer;
	[Export] Sprite2D hourglassSprite;
	[Export] Texture2D[] hourglassTextures;

	int currentIndex = 0;

	public void OnGameStart(string orderName, float timeLimitSeconds)
	{
		PlayFlipAnim();
		currentIndex = 0;
	}

	public void OnOrderTimerChanged(float remainingSeconds, float timeLimitSeconds)
	{
		float progress = 1f - (remainingSeconds / timeLimitSeconds);
		int textureIndex = Mathf.Clamp((int)(progress * (hourglassTextures.Length-1)), 0, hourglassTextures.Length - 1);
		if (textureIndex != currentIndex)
		{
			currentIndex = textureIndex;
			PlayProgressAnim(hourglassTextures[textureIndex]);
		}
	}

	public void PlayFlipAnim()
	{
		animPlayer.Play("flip_hourglass");
	}

	public void PlayProgressAnim(Texture2D hourglassTexture)
	{
		animPlayer.Play("hourglass_progress");
		hourglassSprite.Texture = hourglassTexture;
	}
}
