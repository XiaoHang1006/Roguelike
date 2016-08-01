using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public int hp = 3;
	public Sprite dmgSprite;
	public AudioClip chopSound1;
	public AudioClip chopSound2;

	private SpriteRenderer spriteRender;

	void Awake()
	{
		spriteRender = GetComponent<SpriteRenderer> ();
	}

	public void DamageWall(int loss)
	{
		SoundManager.instance.RandomizeSfx (chopSound1, chopSound2);
		hp-=loss;
		spriteRender.sprite = dmgSprite;
		if (hp < 0) {
			gameObject.SetActive (false);
		}
	}

}
