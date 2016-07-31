using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public float turnDelay = 0.1f;
	public int playerFoodPoints = 100;
	[HideInInspector]public bool playerTurn = true;
	private bool enemyMoving;
	private int level=3;
	private List<Enemy> enemys;
	private BoarderManager boarderManager;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		}else if(instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
		boarderManager = GetComponent<BoarderManager> ();
		enemys = new List<Enemy>();
		InitGame ();
	}

	void InitGame()
	{
		enemys.Clear ();
		boarderManager.SetupScene (level);
	}

	void Update()
	{
		if (playerTurn || enemyMoving) {
			return;
		}
			
		StartCoroutine (MoveEnemies ());
	}

	IEnumerator MoveEnemies()
	{
		enemyMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemys.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}
		for (int i = 0; i < enemys.Count; i++) {
			enemys [i].MoveEnemy ();
			yield return new WaitForSeconds (enemys [i].moveTime);
		}
		playerTurn = true;
		enemyMoving = false;
	}



	public void AddEnemy(Enemy enemy)
	{
		enemys.Add (enemy);
	}

	public void GameOver()
	{
		enabled = false;
	}
}
