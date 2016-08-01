using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public float turnDelay = 0.1f;
	public int playerFoodPoints = 100;
	public float levelStartDelay = 1f;
	[HideInInspector]public bool playerTurn = true;
	private GameObject levelImage;
	private bool doingSetup = true;
	private Text levelText;
	private bool enemyMoving;
	private int level=1;
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
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		doingSetup = true;
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);
		enemys.Clear ();
		boarderManager.SetupScene (level);
	}

	void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	void Update()
	{
		if (playerTurn || enemyMoving || doingSetup) {
			return;
		}
			
		StartCoroutine (MoveEnemies ());
	}

	void OnLevelWasLoaded()
	{
		level++;
		InitGame ();
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
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive(true);
		enabled = false;
	}
}
