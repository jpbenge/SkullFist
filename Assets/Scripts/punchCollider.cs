using UnityEngine;
using System.Collections;

public class punchCollider : MonoBehaviour {

	public int strength = 5;
	public float destroySecs = 1f;
	float spawnTime = 0f;
    
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Skullfist")
            other.gameObject.BroadcastMessage("OnHit", strength, SendMessageOptions.DontRequireReceiver);
    }
    void Start()
    {
    	spawnTime = Time.time;
    }
    void Update()
    {
    	if (Time.time >= spawnTime+destroySecs)
    	{
    		Destroy(gameObject);
    	}
    }
}