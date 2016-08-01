using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MovingObject {

	public float restartLevelDelay = 1f;
	public int wallDamage;
	public int pointPerFood =10;
	public int pointPerSoda = 20;
	public Text foodText;
	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameoverSound;
	private Animator animator;
	private int food;
	private Vector2 touchOrigin = -Vector2.one;
	protected override void Start ()
	{
		food = GameManager.instance.playerFoodPoints;
		foodText.text = "Food " + food;
		animator = GetComponent<Animator> ();
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playerTurn)
			return;
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		int horizontal = 0;
		int vertical = 0;
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");
		if (horizontal != 0)
			vertical = 0;
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_IPHONE
		if(Input.touchCount>0)
		{
		Touch myTouch = Input.touches[0];
		if(myTouch.phase == TouchPhase.Began)
		{
		touchOrigin = myTouch.position;
		}
		else if(myTouch.phase == TouchPhase.Ended && touchOrigin.x>=0)
		{
		Vector2 touchEnd = myTouch.position;

		//Calculate the difference between the beginning and end of the touch on the x axis.
		float x = touchEnd.x - touchOrigin.x;

		//Calculate the difference between the beginning and end of the touch on the y axis.
		float y = touchEnd.y - touchOrigin.y;

		//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
		touchOrigin.x = -1;

		//Check if the difference along the x axis is greater than the difference along the y axis.
		if (Mathf.Abs(x) > Mathf.Abs(y))
		//If x is greater than zero, set horizontal to 1, otherwise set it to -1
		horizontal = x > 0 ? 1 : -1;
		else
		//If y is greater than zero, set horizontal to 1, otherwise set it to -1
		vertical = y > 0 ? 1 : -1;
		}
		}
		#endif
		if (horizontal != 0 || vertical != 0) {
			AttempMove<Wall> (horizontal,vertical);
		}
	}

	private void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "Food") {
			SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
			food += pointPerFood;
			foodText.text = "+" + pointPerFood + "Food " + food;
			target.gameObject.SetActive (false);
		} else if (target.tag == "Soda") {
			SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
			food += pointPerSoda;
			foodText.text = "+" + pointPerSoda + "Food " + food;
			target.gameObject.SetActive (false);
		} else if (target.tag == "Exit") {
			Invoke ("ResetStart", restartLevelDelay);
		}
	}

	private void ResetStart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	protected override void AttempMove<T> (int xDir, int yDir)
	{
		food--;
		foodText.text = "Food " + food;
		base.AttempMove<T>(xDir, yDir);
		CheckIsOver ();
		GameManager.instance.playerTurn = false;
		SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
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
		foodText.text = "-" + loss + "Food " + food;
		animator.SetTrigger ("PlayDop");
		CheckIsOver ();
	}

	private void CheckIsOver()
	{
		if (food <= 0) 
		{
			foodText.gameObject.SetActive (false);
			SoundManager.instance.PlaySingle (gameoverSound);
			SoundManager.instance.musicSource.Stop ();
			GameManager.instance.GameOver ();
		}
	}
}
