using UnityEngine;
using System.Collections;

public class EnemyFactory : MonoBehaviour {

	public Enemy[] enemyArray;

	// Use this for initialization
	void Start () {
		InitEnemies();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitEnemies()
	{
		GameObject[] enemies;
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		enemyArray = new Enemy[enemies.Length];
		for (int e = 0; e < enemies.Length;e++)
		{
			enemyArray[e] = enemies[e].GetComponent<Enemy>();
		}
	}
}
