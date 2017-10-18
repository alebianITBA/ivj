using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviorSingleton<SoundManager> {
	public AudioSource dot;
	public AudioSource intro;
	public AudioSource siren;

	public void PlayDotSound() {
		dot.Play ();
	}

	public void PlayIntroSound() {
		intro.Play ();
	}

	public void PlaySirenSound() {
		siren.Play ();
	}
}
