/*using UnityEngine;
using System.Collections;

using pumpkin.display;
using pumpkin.events;
using pumpkin.ui;

[RequireComponent (typeof (CharacterController))]
public class skullfist_controller : MonoBehaviour {

	public InteractiveMovieClipBehaviour mc;
	public enum CharacterState {
		Idle = 0,
		Running = 1,
		Jumping = 2,
		Falling = 3,
		Punching = 4,
		Hit = 5,
	};

	public CharacterState characterState;
	public float runSpeed = 7.5f;
	public float jumpHeight = 20.0f;
	public float jumpDir = 0f;
	public float gravity = 20.0f;
	public Vector3 velocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;

	public float maxTime = 300;
	public float curTime = 300;

	public bool reachedApex = false;

	public Vector3 runVector = Vector3.zero;
	public Vector3 gravVector = Vector3.zero;
	public Vector3 jumpVector = Vector3.zero;
	public AudioClip[] jumpSounds;
	public GameObject mcam;

	public GameObject punchBox;
	public AudioClip[] punchSounds;

	Vector3 jumpStartPos;

	private CollisionFlags collisionFlags;

	private bool grounded = true;
	private CharacterController controller;
	// Use this for initialization
	void Start () {
		curTime = maxTime;
		mcam = GameObject.Find("Main Camera");
		grounded = true;
		characterState = CharacterState.Running;
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CheckTime();
		Vector3 movement = runVector + velocity;
		collisionFlags = controller.Move(movement);
		runVector = new Vector3 (
			0*runSpeed*Time.deltaTime,
			0,
			0
		);		
		grounded = IsGrounded();
		if (grounded)
		{
			characterState = CharacterState.Running;
		}
		if (characterState == CharacterState.Running)
		{
			velocity = Vector3.zero;
		}
		if (characterState == CharacterState.Jumping)
		{
			ApplyJump();
		}
		ApplyGravity();

		transform.position = new Vector3(transform.position.x,transform.position.y,0f);
		
			if (mc.swf == "Sprites/skullfist_punch.swf" && mc.currentFrame > 5)
			{
				mc.swf = "Sprites/skullfist_run.swf";
				mc.symbolName = "skullfist_run";
				mc.loop = true;
				characterState = CharacterState.Running;
			}
			if (mc.swf == "Sprites/skullfist_uppercut.swf" && mc.currentFrame > 9)
			{
				mc.swf = "Sprites/skullfist_run.swf";
				mc.symbolName = "skullfist_run";
				mc.loop = true;
				characterState = CharacterState.Running;
			}
		
		if (Input.GetKeyDown("space"))
		{
			Jump(transform.position.x+100, jumpHeight);
		}
	}

	void Jump(float xPos, float jumpY)
	{
		Vector3 p = mcam.camera.ScreenToWorldPoint(new Vector3(xPos,0,0));
		jumpDir = (p.x - transform.position.x)/800;
		jumpDir = Mathf.Clamp(jumpDir,-0.1f,0.1f);
		characterState = CharacterState.Jumping;

		audio.clip = jumpSounds[Random.Range(0,jumpSounds.Length)];
		audio.pitch = Random.Range(0.7f,1.3f);
		audio.Play();
		print(audio.clip);
	}

	void Punch()
	{
		audio.clip = punchSounds[Random.Range(0,punchSounds.Length)];
		audio.pitch = Random.Range(0.7f,1.3f);
		audio.Play();

		characterState = CharacterState.Punching;
		mc.swf = "Sprites/skullfist_punch.swf";
		mc.symbolName = "skullfist_punch";
		mc.currentFrame = 0;
		mc.loop = false;
		GameObject clone = Instantiate(punchBox, new Vector3(transform.position.x+2,transform.position.y,0), transform.rotation) as GameObject;
            clone.GetComponent<punchCollider>().strength = 5;
	}

	void UpperCut() {
		audio.clip = punchSounds[Random.Range(0,punchSounds.Length)];
		audio.pitch = Random.Range(0.4f,0.6f);
		audio.Play();
		print(audio.clip);
		characterState = CharacterState.Punching;
		mc.swf = "Sprites/skullfist_uppercut.swf";
		mc.symbolName = "skullfist_uppercut";
		mc.currentFrame = 0;
		mc.loop = false;
		GameObject clone = Instantiate(punchBox, new Vector3(transform.position.x+2,transform.position.y,0), transform.rotation) as GameObject;
            clone.GetComponent<punchCollider>().strength = 8;
        //velocity = new Vector3(velocity.x,0.8f,0);
	}
	void ApplyJump() {
		if (grounded)
		{
			jumpStartPos = transform.position;
		}
		if (transform.position.y >= jumpStartPos.y+jumpHeight || ((collisionFlags & CollisionFlags.CollidedAbove) != 0))
		{
			jumpVector = Vector3.zero;
			characterState = CharacterState.Falling;
		}
		else
		{
			velocity += new Vector3 (
				jumpDir,
				0.1f*gravity,
				0
			);
		}

	}
	bool IsGrounded () {
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}

	void ApplyGravity() {
		if (!grounded)
		{
			velocity += new Vector3 (
				0,
				-0.075f*gravity,
				0
				);
		}
	}

	public void CheckTime()
	{
		if (Time.time > curTime)
		{
			print("You Lose");
		}
	}


	public void OnHit (float amt)
	{
		curTime -= amt;
	}

		// Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_LongTapStart += On_LongTapStart;
	}
	// Unsubscribe
	void OnDisable(){
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_LongTapStart -= On_LongTapStart;
	}
	// Unsubscribe
	void OnDestroy(){
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_LongTapStart -= On_LongTapStart;
	}

	// Touch start event
	public void On_SimpleTap(Gesture gesture) {
		//Jump(gesture.position.x);
		if (characterState != CharacterState.Punching)
		{
			Punch();
		}
	}

	public void On_TouchStart(Gesture gesture) {
		
	}

	public void On_LongTapStart(Gesture gesture) {
		UpperCut();
	}
}
*/