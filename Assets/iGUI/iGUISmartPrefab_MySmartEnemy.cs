using UnityEngine;
using System.Collections;
using iGUI;

public class iGUISmartPrefab_MySmartEnemy : MonoBehaviour{
	
	public float jumpDelay = 2f;
  	float lastJump = 0;
  	int health = 10;
  	bool destroy = false;

	[HideInInspector]
	public iGUIContainer enemy;
	
	private tk2dAnimatedSprite anim;



		// Texture for runtime sprite collection demo
	public Texture2D runtimeTexture;

	// Texture packer textures
	public Texture2D texturePackerTexture;
	public TextAsset texturePackerExportFile;

	// This object will be destroyed on startup
	public GameObject destroyOnStart;
	//
	
	public tk2dAnimatedSprite spriteInstance = null;
	public tk2dSpriteCollectionData spriteCollectionInstance = null;
	tk2dSpriteCollectionSize spriteCollectionSize = tk2dSpriteCollectionSize.Explicit(5, 640);
	
	static iGUISmartPrefab_MySmartEnemy instance;
	public iDataFractalFrameworkViewModel ViewModel;
	
	void Awake(){
		instance=this;

	
	}
	void Start(){
		anim = GetComponent<tk2dAnimatedSprite>();
		Debug.Log(enemy.order);
		lastJump = 0;
		health = 10;
		destroy = false;
	}

	void Update(){
		Move();
		//Debug.Log(ViewModel.Context.CEnemies.SelectedIndex);
		if(destroy)
			iGUICode_FractalFramework.getInstance().ViewModel.Context.CEnemies.Remove(enemy.order);
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
		rigidbody.AddForce(300,200,0);
		if (health <= 0)
		{
			destroy = true;
			//ViewModel.RemoveItem(enemy.order);
			//ViewModel.Context.CEnemies.Remove(enemy.order);
			//Destroy(gameObject);
		}
		anim.color = Color.red;
		Hashtable hash = new Hashtable();
		hash.Add("from", anim.color);
		hash.Add("to", Color.white);
		hash.Add("onupdate", "ColorUpdate");
		hash.Add("time", 0.5f);
		iTween.ValueTo(gameObject,hash);
	}

	void ColorUpdate(Color c)
	{
		anim.color = c;
	}

	public static iGUISmartPrefab_MySmartEnemy getInstance(){
		return instance;
	}
    //
	
	
	public void image1_Init(iGUIImage caller){
		
	}

	void OnCollisionEnter(Collision collision) 
	{
        if (collision.gameObject.tag == "Skullfist")
        {
        	collision.gameObject.BroadcastMessage("OnHit", 5.0f,SendMessageOptions.DontRequireReceiver);
        }
    }

			public void button2_Click(iGUIButton caller){
		ViewModel.Context.AddItemB();
	}
		public void button3_Click(iGUIButton caller){
		ViewModel.Context.AddItemC();
	}
		public void button4_Click(iGUIButton caller){
		ViewModel.Context.RemoveRandomItem();
	}
	
}
