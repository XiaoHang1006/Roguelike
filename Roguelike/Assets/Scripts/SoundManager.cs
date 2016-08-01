using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;
	public AudioSource musicSource;
	public AudioSource efxSource;
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void RandomizeSfx(params AudioClip[] clips)
	{
		AudioClip clip = clips [Random.Range (0, clips.Length)];
		float pitch = Random.Range (lowPitchRange, highPitchRange);
		efxSource.pitch = pitch;
		efxSource.clip = clip;
		efxSource.Play ();
	}
}
