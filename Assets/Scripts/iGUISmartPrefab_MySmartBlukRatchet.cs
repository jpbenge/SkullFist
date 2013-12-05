using UnityEngine;
using System.Collections;
using iGUI;


public class iGUISmartPrefab_MySmartBlukRatchet : MonoBehaviour {
	public float jumpDelay = 2f;
	float lastJump = 0;
	int health = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}


	void Move()
	{	
		if (Time.time >= lastJump+jumpDelay)
		{
			rigidbody.AddForce( new Vector3(-100,300,0));
			lastJump = Time.time;
		}
		else
			rigidbody.AddForce( new Vector3(-5,0,0));
	}

	void OnHit(int dmg)
	{
		health -= dmg;
		if (health <= 0)
		{
		
			
			//int ord = gameObject.GetComponent<iGUIContainer>().order;
			Destroy(gameObject);
		}
	}
	
}
