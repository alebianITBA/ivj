// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

public enum SndIdGame : int
{
	BALL_HIT_ANOTHER = 0,
	GAME_START 	     = 1,
	CUE_HIT          = 2,
	IN_POCKET        = 3,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class SoundListGame : SoundList
{
	SoundProp[] sounds = {
		new SoundProp((int) SndIdGame.BALL_HIT_ANOTHER, "ball_hit_another", 1, 100),
		new SoundProp((int) SndIdGame.GAME_START, "game_start", 1, 100),
		new SoundProp((int) SndIdGame.CUE_HIT, "hit", 1, 100),
		new SoundProp((int) SndIdGame.IN_POCKET, "in_pocket", 1, 100),
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


