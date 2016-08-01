using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	public int playerDamage;
	public AudioClip attackSound1;
	public AudioClip attackSound2;
	private Animator animator;
	private Transform target;
	protected override void Start ()
	{
		animator = GetComponent<Animator> ();
		GameManager.instance.AddEnemy (this);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttempMove<T> (int xDir, int yDir)
	{
		base.AttempMove<T> (xDir, yDir);
	}

	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;
		if (Mathf.Abs (transform.position.x - target.position.x) < float.Epsilon)
			yDir = transform.position.y > target.position.y ? -1 : 1;
		else
			xDir = transform.position.x > target.position.x ? -1 : 1;
	
		AttempMove<Player>(xDir, yDir);
	}

	protected override void OnCanMove<T> (T component)
	{
		Player hitPlayer = component as Player;
		hitPlayer.LossFood (playerDamage);
		animator.SetTrigger ("EnemyAttack");
		SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
	}
}
