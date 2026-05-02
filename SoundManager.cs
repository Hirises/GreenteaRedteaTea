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
	TeapotBrewing,
	TeapotBrew,
	CandleLit,
	TreeBloom,
	TreePick,
	Trashbin,
	Success,
	Fail,
	CustomerEnter,
	CandleExtinguish,
	CandleLitFail,
	LeafPickWet,
	LeafPutWet,
	JarPut,
	TeapotPut,
	TreeGrow,
	TreeDie,
	CalendarFlip,
	Kick,
	CalendarOpen,
	DoorOpen,
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
	
	public static void Stop(SFXType sfxType)
	{
		Instance.StopInternal(sfxType);
	}

	void PlayInternal(SFXType sfxType)
	{
		if (soundDictionary.ContainsKey(sfxType))
		{
			soundDictionary[sfxType].Play();
		}
	}
	
	void StopInternal(SFXType sfxType)
	{
		if (soundDictionary.ContainsKey(sfxType))
		{
			soundDictionary[sfxType].Stop();
		}
	}


	public static void PlayHourglassFlip() => Play(SFXType.HourglassFlip);
	public static void PlayHourglassLand() => Play(SFXType.HourglassLand);
}
