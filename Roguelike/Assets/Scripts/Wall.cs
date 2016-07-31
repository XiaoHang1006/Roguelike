using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public int hp = 3;
	public Sprite dmgSprite;

	private SpriteRenderer spriteRender;

	void Awake()
	{
		spriteRender = GetComponent<SpriteRenderer> ();
	}

	public void DamageWall(int loss)
	{
		hp-=loss;
		spriteRender.sprite = dmgSprite;
		if (hp < 0) {
			gameObject.SetActive (false);
		}
	}

}
