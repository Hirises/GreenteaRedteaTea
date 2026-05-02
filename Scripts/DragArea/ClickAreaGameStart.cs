using Godot;
using System;

public partial class ClickAreaGameStart : ClickArea
{
	[Export] AnimationPlayer animationPlayer;
	[Export] GameManager gameManager;

	enum State
	{
		Before,
		Animating,
		After,
	}

	State state = State.Before;

	public override void OnClick()
	{
		if (state != State.Before)
			return;
		
		state = State.Animating;
		animationPlayer.Play("flip_close");
		SetHoverHighlight(null);
	}

	public void OnAnimationEnd()
	{
		state = State.After;

		gameManager.StartGame();
	}
}
