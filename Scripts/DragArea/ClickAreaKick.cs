using Godot;
using System;
using System.Collections.Generic;

public partial class ClickAreaKick : ClickArea
{
	[Export] GameManager gameManager;

	public override void OnClick()
	{
		gameManager.KickOutCustomer();
	}
}
