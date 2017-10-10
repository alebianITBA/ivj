using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicSoundManager : MonoBehaviourSingleton<BasicSoundManager> {
	public AudioSource hit;
	public AudioSource pocket;
	public AudioSource start;
	public AudioSource ballAnother;
	public AudioSource smallHit;

	public void PlayStartSound() {
		start.Play ();
	}

	public void PlayCueHitSound() {
		hit.Play ();
	}

	public void PlayBallHitSound() {
		ballAnother.Play ();
	}

	public void PlayPocketHitSound() {
		pocket.Play ();
	}

	public void PlaySmallBallHit() {
		smallHit.Play ();
	}
}