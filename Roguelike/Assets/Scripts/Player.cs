using UnityEngine;
using System.Collections;

public class Player : MovingObject {

	public int wallDamage;
	public int pointPerFood =10;
	public int pointPerSoda = 20;
	private Animator animator;
	private int food;

	protected override void Start ()
	{
		food = GameManager.instance.playerFoodPoints;
		animator = GetComponent<Animator> ();
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playerTurn)
			return;
		int horizontal = 0;
		int vertical = 0;
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");
		if (horizontal != 0)
			vertical = 0;
		if (horizontal != 0 || vertical != 0) {
			AttempMove<Wall> (horizontal,vertical);
		}

	}

	public void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Food") {
			food += pointPerFood;
			target.gameObject.SetActive (false);
		} else if (target.tag == "Soda") {
			food += pointPerSoda;
			target.gameObject.SetActive (false);
		}
	}

	void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	protected override void AttempMove<T> (int xDir, int yDir)
	{
		food--;
		base.AttempMove<T>(xDir, yDir);
		CheckIsOver ();
		GameManager.instance.playerTurn = false;
	}

	protected override void OnCanMove<T> (T component)
	{
		Wall wall = component as Wall;
		wall.DamageWall (wallDamage);
		animator.SetTrigger ("PlayerAttack");
	}

	public void LossFood(int loss)
	{
		food -= loss;
		animator.SetTrigger ("PlayDop");
		CheckIsOver ();
	}

	private void CheckIsOver()
	{
		if (food <= 0) 
		{
			GameManager.instance.GameOver ();
		}
	}
}
