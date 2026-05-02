using Godot;
using System;
using System.Collections.Generic;

public partial class ClickAreaKick : ClickArea
{
	[Export] GameManager gameManager;

	public override void OnClick()
	{
		SoundManager.Play(SFXType.Kick);
		gameManager.KickOutCustomer();
	}
}
