// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

public enum SndIdGame : int
{
	BACKGROUND_MUSIC = 0,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class SoundListGame : SoundList
{
	SoundProp[] sounds = {
		new SoundProp((int) SndIdGame.BACKGROUND_MUSIC, "scavengers_music", 1, true, SndType.SND_MUSIC, 100),
	};

	new void Start()
	{
		base.Start();
	}

	protected override SoundProp[] GetSoundProps()
	{
		return sounds;
	}
}


