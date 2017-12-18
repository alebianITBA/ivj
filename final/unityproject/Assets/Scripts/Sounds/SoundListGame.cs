// Mono Framework
using System;
using System.Collections;

// Unity Framework
using UnityEngine;

public enum SndIdGame : int
{
    BACKGROUND_MUSIC = 0,
    PLAYER_1_SHOOT = 1,
    PLAYER_2_SHOOT = 2,
    DEAD = 3,
    ROCKET_SHOOT = 4,
}

/// <summary>
/// SoundList of the Game
/// </summary>
public class SoundListGame : SoundList
{
    SoundProp[] sounds = {
        new SoundProp((int)SndIdGame.BACKGROUND_MUSIC, "scavengers_music", 1, true, SndType.SND_MUSIC, 100),
        new SoundProp((int)SndIdGame.PLAYER_1_SHOOT, "player1shoot", 1, 100),
        new SoundProp((int)SndIdGame.PLAYER_2_SHOOT, "player2shoot", 1, 100),
        new SoundProp((int)SndIdGame.DEAD, "dead", 1, 100),
        new SoundProp((int)SndIdGame.ROCKET_SHOOT, "rocketshoot", 1, 100),
    };

    new void Start ()
    {
        base.Start();
    }

    protected override SoundProp[] GetSoundProps ()
    {
        return sounds;
    }
}


