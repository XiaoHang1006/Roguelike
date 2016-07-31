using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoarderManager : MonoBehaviour {
	[System.Serializable]
	public class Count{
		public int maxNum;
		public int minNum;

		public Count(int maxNum, int minNum)
		{
			this.maxNum = maxNum;
			this.minNum = minNum;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count (5, 9);
	public Count foodCount = new Count (1, 5);
	public GameObject exit;
	public GameObject[] wallTiles;
	public GameObject[] floorTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outwallTiles;
	public GameObject[] foodTiles;

	private Transform boardHolder;
	private List<Vector3> gridPostion = new List<Vector3> ();

	public void InitPositionList()
	{
		for (int i = 1; i < columns - 1; i++) {
			for (int j = 1; j < rows - 1; j++) {
				gridPostion.Add(new Vector3 (i, j, 0));
			}
		}
	}

	public void BoardSetup()
	{
		boardHolder = new GameObject ("BoardHolder").transform;
		for (int i = -1; i < (columns+1); i++) {
			for (int j = -1; j < (rows+1); j++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if(i==-1||i==columns||j==-1||j == rows)
					toInstantiate = outwallTiles[Random.Range(0,outwallTiles.Length)];
					
				GameObject instantiate = Instantiate (toInstantiate, new Vector3 (i, j, 0), Quaternion.identity) as GameObject;
				instantiate.transform.SetParent (boardHolder);
			}
		}
	}

	public Vector3 RandomPosition()
	{
		Vector3  randomPosition = gridPostion[Random.Range (0, gridPostion.Count)];
		gridPostion.Remove (randomPosition);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tilesArry,int min,int max)
	{
		int objectCount = Random.Range (min, max + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject toInstantiate = tilesArry [Random.Range (0, tilesArry.Length)];
			Instantiate (toInstantiate, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level)
	{
		BoardSetup ();
		InitPositionList ();

		LayoutObjectAtRandom (wallTiles, wallCount.minNum, wallCount.maxNum);
		LayoutObjectAtRandom (foodTiles, foodCount.minNum, foodCount.maxNum);

		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0), Quaternion.identity);
	}

}
