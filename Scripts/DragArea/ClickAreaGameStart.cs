using Godot;
using System;
using System.Collections.Generic;

public partial class ClickAreaGameStart : ClickArea
{
	[Export] AnimationPlayer animationPlayer;
	[Export] GameManager gameManager;

	[Export] Sprite2D closePanel;
	[Export] Sprite2D openPanel;
	[Export] Sprite2D[] numPanel10;
	[Export] Sprite2D[] numPanel01;

	List<Sprite2D> nextOpenPanel = new();
	int prevScore = 0;


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
		nextOpenPanel.Clear();
		nextOpenPanel.Add(openPanel);
		animationPlayer.Play("flip_close");
		SetHoverHighlight(null);
		SoundManager.Play(SFXType.CalendarFlip);
	}

	public void OnOrderEnded(int result, string orderName)
	{
		OnScoreChange(gameManager.score);
	}

	public void OnScoreChange(int score)
	{
		nextOpenPanel.Clear();
		if (score == prevScore)
			return;
		prevScore = score;
		if (score > 99) score = 99;
		nextOpenPanel.Add(numPanel01[score % 10]);
		nextOpenPanel.Add(numPanel10[score / 10]);
		animationPlayer.Play("flip_close");
	}

	public void OnAnimationEnd()
	{
		if (state == State.Animating)
		{
			state = State.After;
			gameManager.StartGame();
		}
	}

	public void OnChangePanel()
	{
		closePanel.Visible = false;
		openPanel.Visible = false;
		foreach (var panel in numPanel01)
			panel.Visible = false;
		foreach (var panel in numPanel10)
			panel.Visible = false;

		foreach (var panel in nextOpenPanel)
			panel.Visible = true;
	}
}
