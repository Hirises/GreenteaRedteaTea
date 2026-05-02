using Godot;
using System;

public partial class ClickAreaCandle : ClickArea
{
	[Export] float candleMaxTime = 3f;
	[Export] DraggableTeapot teapot;
	[Export] Node2D flameOn;
	[Export] Node2D flameOff;
	[Export] GpuParticles2D particle;
	float candleTime = 0f;
	bool isLit = false;

	public override void OnClick()
	{
		if (isLit)
		{
			isLit = false;
			SoundManager.Stop(SFXType.TeapotBrewing);
			SoundManager.Play(SFXType.CandleExtinguish);
			GD.Print($"Extinguishing candle {Name}.");
			SetFlameState(false);
			return;
		}
		if (!teapot.CanBrew()) {
			GD.Print($"Cannot lit candle {Name} because teapot is unable to brew.");
			SoundManager.Play(SFXType.CandleLitFail);
			return;
		}
		GD.Print($"Lit candle {Name}!");
		isLit = true;
		candleTime = 0f;
		SetFlameState(true);
		SoundManager.Play(SFXType.CandleLit);
		SoundManager.Play(SFXType.TeapotBrewing);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (isLit)
		{
			candleTime += (float)delta;
			if (candleTime >= candleMaxTime)
			{
				isLit = false;
				GD.Print($"Candle {Name} has burned out. Trying brew the tea in teapot...");
				teapot.TryBrew();
				SetFlameState(false);
				return;
			}
			if (InputManager.Instance.currentDragItem == teapot)
			{
				isLit = false;
				SoundManager.Stop(SFXType.TeapotBrewing);
				GD.Print($"Teapot is lifted while candle {Name} is lit. Extinguishing candle.");
				SetFlameState(false);
				return;
			}
		}
	}

	void SetFlameState(bool lit)
	{
		if (lit)
		{
			flameOff.Hide();
			flameOn.Show();
			particle.Emitting = true;
		}
		else
		{
			flameOff.Show();
			flameOn.Hide();
			particle.Emitting = false;
		}
	}

	public override bool CanClick()
	{
		return teapot.CanBrew();
	}
}
