// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

public enum SndIdGame : int
{
	BACKGROUND_MUSIC = 0,
	NO_AMMO = 1,
	AMMO_PICK = 2,
	SHOT = 3,
    ZOMBIE_BITE = 4,
    ZOMBIE_GOT_HIT = 5,
    ZOMBIE_SEES_YOU = 6,
    ZOMBIE_SPAWN = 7,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class SoundListGame : SoundList
{
	SoundProp[] sounds = {
		new SoundProp((int) SndIdGame.BACKGROUND_MUSIC, "scavengers_music", 1, true, SndType.SND_MUSIC, 100),
		new SoundProp((int) SndIdGame.NO_AMMO, "no-ammo", 1, 100),
		new SoundProp((int) SndIdGame.AMMO_PICK, "ammo-pick", 1, 100),
		new SoundProp((int) SndIdGame.SHOT, "shot", 1, 80),
        new SoundProp((int) SndIdGame.ZOMBIE_BITE, "zombie-bite", 1, 100),
        new SoundProp((int) SndIdGame.ZOMBIE_SEES_YOU, "zombie-see-you", 1, 100),
        new SoundProp((int) SndIdGame.ZOMBIE_GOT_HIT, "zombie-got-hit", 1, 100),
        new SoundProp((int) SndIdGame.ZOMBIE_SPAWN, "zombie-spawn", 1, 60),
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


