using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public enum SFXType
{
    None,
    HourglassFlip,
    HourglassLand,
    PlatePick,
    PlatePut,
    CupPick,
    CupPut,
    CupPour,
    LeafJarPick,
    LeafPick,
    LeafPut,
    JarPick,
    JarPour,
    TeapotPick,
    TeapotPour,
    TeapotBrew,
    CandleLit,
    TreeBloom,
    TreePick,
    Trashbin,
    Success,
    Fail,
}

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }

    Dictionary<SFXType, SoundPlayer> soundDictionary = new();

    public override void _Ready()
    {
        base._Ready();

        Instance = this;

        foreach (var node in GetChildren())
        {
            if (node is SoundPlayer)
            {
                var player = (node as SoundPlayer);
                var sfxType = player.SFXType;
                soundDictionary[sfxType] = player;
            }
        }
    }


    public static void Play(SFXType sfxType)
    {
        Instance.PlayInternal(sfxType);
    }

    void PlayInternal(SFXType sfxType)
    {
        if (soundDictionary.ContainsKey(sfxType))
        {
            soundDictionary[sfxType].Play();
        }
    }


    public static void PlayHourglassFlip() => Play(SFXType.HourglassFlip);
    public static void PlayHourglassLand() => Play(SFXType.HourglassLand);
}
