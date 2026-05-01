using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SoundPlayer : Node
{
    [Export] SFXType sfxType;
    public SFXType SFXType => sfxType;
    [Export] AudioStreamPlayer player;

    public void Play()
    {
        player.PitchScale = 1f + (float)Random.Shared.NextDouble() * 0.2f;
        player.Play();
    }
}
