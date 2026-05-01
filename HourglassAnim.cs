using Godot;
using System;

public partial class HourglassAnim : Node
{
    [Export] AnimationPlayer animPlayer;

    public void PlayAnim()
    {
        animPlayer.Play("flip_hourglass");
    }
}