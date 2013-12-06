using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int health = 0;
	public int maxHealth = 100;
	// Use this for initialization
	void Start () {
		health = maxHealth;	
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			OnDeath();
	}

	void OnDeath()
	{
		Destroy(gameObject);
	}

	void OnHit(int dmg)
	{
		health -= dmg;
	}
}
