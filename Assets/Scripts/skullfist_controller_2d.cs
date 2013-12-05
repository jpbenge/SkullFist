using UnityEngine;
using System.Collections;
using iGUI;

[RequireComponent (typeof (CharacterController))]
public class skullfist_controller_2d : MonoBehaviour {

//the 2DTK sprite animation object
private tk2dSpriteAnimator anim;

	//state enum
	public enum CharacterState {
		Idle = 0,
		Running = 1,
		Jumping = 2,
		Falling = 3,
		Punching = 4,
		Hit = 5,
		Flipping = 6
	};

	//state enum variable
	public CharacterState characterState;
	//-----------------Punch strength variables------------------------
	public int punch1Strength = 2;
	public int punch2Strength = 5;
	public int punch3Strength = 6;
	public int punch4Strength = 8;
	public int uppercutStrength = 10;

	//-------------punch variables----------------
	public float lastPunchTime = 0f;
	// the last type of punch thrown
	public int lastPunchNum = 0;
	public float punch1min = 0.6f;
	public float punch1max = 2f;
	public float punch2min = 0.6f;
	public float punch2max = 2f;

	public float lastUppercutTime = 0f;
	public int lastUppercutNum = 0;
	public float uppercutMin = 0.2f;
	public float uppercutMax = 0.4f;
	//----------------------------------------

	//---------------------GUI----------------------------
	public float baseWidth = 1280f;
	public float baseHeight = 720f;
	Vector3 scaleVal;
	public Texture2D guiSkull_1;
	public Texture2D skullMeter;
	public Texture2D skullMeterFull;

	//---------------Movement Variables---------------------
	public float runSpeed = 7.5f;
	public float jumpHeight = 2.0f;
	public float jumpDir = 0f;
	public float gravity = 20.0f;
	bool swiping = false;
	bool jumpFrame = false;
	public bool reachedApex = false;
	public Vector3 velocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;
	public Vector3 runVector = Vector3.zero;
	public Vector3 gravVector = Vector3.zero;
	public Vector3 jumpVector = Vector3.zero;
	float jumpTime = 0f;
	float jumpDelay = 0.1f;
	Vector3 jumpStartPos;
	private bool grounded = true;

	//---------Time (health) variables-----------------
	public float maxTime = 200;
	public float curTime = 200;
	public float remainingTime = 200;
	public float lastHit = 0f;
	public float hitCooldown = 1.5f;
	//-------------------------------------------------

	// sounds to play for jumping
	public AudioClip[] jumpSounds;

	// main camera
	public GameObject mcam;

	//camera target
	public GameObject camTarget;
	public Vector3 camTargetInitialPos;

	// level spiral
	public SlowRotate level;

	// the punch collider
	public GameObject punchBox;
	//sounds to play when punching
	public AudioClip[] punchSounds;

	public AudioClip[] hurtSounds;

	// collision flags for testing collision
	private CollisionFlags collisionFlags;

	// controller variable
	private CharacterController controller;
	

	// Use this for initialization
	void Start () {
		swiping = false;
		jumpFrame = false;
		level = GameObject.Find("Level").GetComponent<SlowRotate>();
		camTarget = GameObject.Find("camTarget");
		camTargetInitialPos = camTarget.transform.localPosition;
		anim = GetComponent<tk2dSpriteAnimator>();
		anim.AnimationEventTriggered = StepEvent;
		curTime = maxTime;
		mcam = Camera.main.gameObject;
		grounded = true;
		characterState = CharacterState.Running;
		controller = GetComponent<CharacterController>();
		lastHit = 0f;
	}
	// called when skullfist is running used to play footstep sound
	void StepEvent(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum){
        //audio.clip = jumpSounds[Random.Range(0,jumpSounds.Length)];
		//audio.pitch = Random.Range(0.4f,0.6f);
		//audio.Play();
	}

	//called on the impact frame of a punch
	void PunchEvent(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
	{
		// create the punch hit box
		GameObject clone = Instantiate(punchBox, new Vector3(transform.position.x+2,transform.position.y,0), transform.rotation) as GameObject;
        clone.transform.parent = transform;
        // set strength based on which punch it is
        // by getting the frame info from the anim trigger
            if (clip.GetFrame(frameNum).eventInfo == "punch1")
            {
            	clone.GetComponent<punchCollider>().strength = punch1Strength;
            }
            if (clip.GetFrame(frameNum).eventInfo == "punch2")
            {
            	clone.GetComponent<punchCollider>().strength = punch2Strength;
            	clone.transform.position = new Vector3(transform.position.x+3.5f,transform.position.y,0);
            }
            if (clip.GetFrame(frameNum).eventInfo == "airPunch1")
            {
            	clone.GetComponent<punchCollider>().strength = punch3Strength;
            	clone.transform.position = new Vector3(transform.position.x+1.5f,transform.position.y-1f,0);
            }
            if (clip.GetFrame(frameNum).eventInfo == "uppercut1")
            {
            	clone.GetComponent<punchCollider>().strength = punch4Strength;
            	clone.transform.position = new Vector3(transform.position.x+1.5f,transform.position.y+1f,0);
            }
            if (clip.GetFrame(frameNum).eventInfo == "uppercut2")
            {
            	clone.GetComponent<punchCollider>().strength = uppercutStrength;
            	clone.transform.position = new Vector3(transform.position.x+1.5f,transform.position.y+1f,0);
            	//second uppercut gets a little vertical boost
            	velocity = new Vector3(0.8f,1.25f,0);
            }
            //print(frame.eventInfo);
	}	

	// Update is called once per frame
	void FixedUpdate () {
		remainingTime = curTime - Time.time;
		//see if we've lost
		//print(velocity);
		//print(grounded);
		CheckTime();
		//calculate the forward run vector
		runVector = new Vector3 (
			0*runSpeed*Time.deltaTime,
			0,
			0
		);
		grounded = IsGrounded();
		if (grounded && characterState != CharacterState.Punching)
		{
			characterState = CharacterState.Running;
		}
		//print("velocity = "+velocity);
		// apply downward force for gravity
		ApplyGravity();
		
		
		if (jumpFrame && !swiping && grounded)
		{
			Jump(transform.position.x+100, jumpHeight);
			jumpFrame = false;
		}
		if (characterState == CharacterState.Jumping)
		{
			print("jumping");
			print(grounded);
			ApplyJump();
		}
		// calculate the movemnt vector 
		Vector3 movement = runVector + velocity;
		velocity = Vector3.zero;
		//move the character
		collisionFlags = controller.Move(movement);

		transform.position = new Vector3(transform.position.x,transform.position.y,0f);
	}

	void OnGUI()
	{
		Rect skullRect = new Rect(50f,20f,480f*(remainingTime/curTime),102f);
		scaleVal = Vector3.zero;
	 	scaleVal.x = Screen.width / baseWidth;
	 	scaleVal.y = Screen.height / baseHeight;
	 	scaleVal.z = 1f;

	 	//Matrix4x4 svMat = GUI.matrix;
	 	GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scaleVal);
		GUI.Label(new Rect(20,100,100,100),remainingTime.ToString());
		GUI.DrawTexture(new Rect(1020f,550f,203f,155f),guiSkull_1);

		GUI.DrawTexture(new Rect(50f,20f,512f,102f),skullMeter);
		GUILayout.BeginArea(skullRect);
		print(skullRect.width);
			GUI.DrawTexture(new Rect(0,0,512f,102f),skullMeterFull);
		GUILayout.EndArea();
	}

	void Jump(float xPos, float jumpY)
	{
		// apply a slight force in the direction of the tap
		// move a bit right if the tap is to the right, left if the tap is to the left
		// of skullfist
		Vector3 p = mcam.camera.ScreenToWorldPoint(new Vector3(xPos,0,0));
		jumpDir = (p.x - transform.position.x)/800;
		jumpDir = Mathf.Clamp(jumpDir,-0.1f,0.1f);
		// character state to jumping
		characterState = CharacterState.Jumping;
		//set the jump time to now
		jumpTime = Time.time;
		jumpStartPos = transform.position;
		// play jump sound
		audio.clip = jumpSounds[Random.Range(0,jumpSounds.Length)];
		audio.pitch = Random.Range(0.7f,1.3f);
		audio.Play();
	}

	void Punch(int punchNum)
	{
		// play one of the punching noises
		audio.clip = punchSounds[Random.Range(0,punchSounds.Length)];
		audio.pitch = Random.Range(0.7f,1.3f);
		audio.Play();
		// set last punch time to now
		lastPunchTime = Time.time;
		//set last punch num to this one
		lastPunchNum = punchNum;
		// character state to punching
		characterState = CharacterState.Punching;
		// play the appropriate punching animation
		anim.Play("punch"+punchNum);
		// set event and complete delagates
		anim.AnimationEventTriggered = PunchEvent;
		anim.AnimationCompleted = PunchCompleteDelegate;
		//print(punchNum);
		
	}


	// happens once the punch is complete sets the state back to running
	// and plays the running animation
	void PunchCompleteDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip){
		if (anim.DefaultClip != clip)
		{
			//print("derp");
			characterState = CharacterState.Running;
			anim.Play("run");
			//print("punched");
			anim.AnimationEventTriggered = StepEvent;
			anim.AnimationCompleted = null;
			swiping = false;
			jumpFrame = false;
		}
	}

	// uppercut method accets paramiter for which uppercut to do
	void UpperCut(int punchNum) {
		// play punching sound
		audio.clip = punchSounds[Random.Range(0,punchSounds.Length)];
		audio.pitch = Random.Range(0.4f,0.6f);
		audio.Play();
		// last uppercut time to now
		lastUppercutTime = Time.time;
		lastUppercutNum = punchNum;
		//print(audio.clip);
		// character state to punching
		characterState = CharacterState.Punching;
		// play appropriate uppercut animation based on peramiter
		anim.Play("uppercut"+punchNum);
		// set event and complete delagates
		anim.AnimationEventTriggered = PunchEvent;
		anim.AnimationCompleted = PunchCompleteDelegate;
        
	}

	void AirPunch(int punchNum) {
		// play punching sound
		audio.clip = punchSounds[Random.Range(0,punchSounds.Length)];
		audio.pitch = Random.Range(0.4f,0.6f);
		audio.Play();
		// last uppercut time to now
		//lastUppercutTime = Time.time;
		//lastUppercutNum = punchNum;
		//print(audio.clip);
		// character state to punching
		characterState = CharacterState.Punching;
		// play appropriate uppercut animation based on peramiter
		anim.Play("airPunch"+punchNum);
		// set event and complete delagates
		anim.AnimationEventTriggered = PunchEvent;
		anim.AnimationCompleted = PunchCompleteDelegate;
        
	}

	void ApplyJump() {
		if (characterState == CharacterState.Jumping)
		{
			jumpVector = new Vector3 (
				jumpDir,
				gravity*0.2f,
				0
			);
			velocity += jumpVector;
		
			if (transform.position.y >= jumpStartPos.y+jumpHeight || ((collisionFlags & CollisionFlags.CollidedAbove) != 0))
			{
				jumpVector = Vector3.zero;
				//velocity = Vector3.zero;
				characterState = CharacterState.Falling;
			}
		}

	}
	bool IsGrounded () {
		//print(controller.collisionFlags == CollisionFlags.Below);
		return (controller.collisionFlags == CollisionFlags.Below);
	}

	void ApplyGravity() {
		velocity += new Vector3 (
		0,
		-gravity*0.1f,
		0
		);
	}

	public void CheckTime()
	{
		if (Time.time > remainingTime)
		{
			print("You Lose");
		}
	}


	public void OnHit (float amt)
	{
		if (Time.time >= lastHit + hitCooldown)
		{
			lastHit = Time.time;
			print("ouch");
			characterState = CharacterState.Hit;
			anim.Play("hit");
			anim.AnimationCompleted = PunchCompleteDelegate;
			anim.Sprite.color = Color.red;
			Hashtable hash = new Hashtable();
			hash.Add("from", anim.Sprite.color);
			hash.Add("to", Color.white);
			hash.Add("onupdate", "ColorUpdate");
			hash.Add("time", 0.5f);
			iTween.ValueTo(gameObject,hash);
			curTime -= amt;

			audio.clip = hurtSounds[Random.Range(0,hurtSounds.Length)];
			audio.Play();
		}
	}

	void ColorUpdate(Color c)
	{
		anim.Sprite.color = c;
	}

		// Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_SimpleTap += On_SimpleTap;
		EasyTouch.On_LongTapStart += On_LongTapStart;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;
		EasyTouch.On_Swipe += On_Swipe;
	}
	// Unsubscribe
	void OnDisable(){
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_LongTapStart -= On_LongTapStart;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;
		EasyTouch.On_Swipe -= On_Swipe;
	}
	// Unsubscribe
	void OnDestroy(){
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_SimpleTap -= On_SimpleTap;
		EasyTouch.On_LongTapStart -= On_LongTapStart;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;
		EasyTouch.On_Swipe -= On_Swipe;
	}

	// Touch start event
	public void On_SimpleTap(Gesture gesture) {
		//Jump(gesture.position.x);
		//print(Time.time-lastPunchTime);
		
	}

	public void On_TouchStart(Gesture gesture) {
		if ((gesture.position.x >= (1000f*scaleVal.x) && gesture.position.x <= (1220f*scaleVal.x)) && ((Screen.height-gesture.position.y) >= (530f*scaleVal.y) && (Screen.height-gesture.position.y) <= (700f*scaleVal.y)))
		{
			if (characterState == CharacterState.Jumping || characterState == CharacterState.Falling)
			{
				AirPunch(1);
			}
		}
	}

	public void On_TouchUp(Gesture gesture) {
		print(gesture.position);
		if ((gesture.position.x >= (1000f*scaleVal.x) && gesture.position.x <= (1220f*scaleVal.x)) && ((Screen.height-gesture.position.y) >= (530f*scaleVal.y) && (Screen.height-gesture.position.y) <= (700f*scaleVal.y)))
		{
			if ((lastPunchNum == 1) && (Time.time - lastPunchTime >= punch1min) && (Time.time - lastPunchTime <= punch1max))
			{	
				print("2");
				Punch(2);
			
			}
			else if (characterState != CharacterState.Punching && Time.time >= lastPunchTime + punch1max)
			{
				print(characterState);
				Punch(1);
			}

		}
		else
		{
			jumpFrame = true;
		}
	}

	public void On_LongTapStart(Gesture gesture) {
		if ((gesture.position.x >= (1000f*scaleVal.x) && gesture.position.x <= (1220f*scaleVal.x)) && ((Screen.height-gesture.position.y) >= (530f*scaleVal.y) && (Screen.height-gesture.position.y) <= (700f*scaleVal.y)))
		{
			if (lastUppercutNum == 1 && (Time.time - lastUppercutTime >= uppercutMin) && (Time.time - lastUppercutTime <= uppercutMax))
			{
				UpperCut(2);
				print("upper 2");
			}
			else if (characterState != CharacterState.Punching)
			{
				UpperCut(1);
			}
		}
	}
	public void On_Swipe(Gesture gesture) {
		swiping = true;
	}

	public void On_SwipeEnd(Gesture gesture) {
		//print(gesture.swipe);
		jumpFrame = false;
		//swiping = false;
		if (gesture.swipe == EasyTouch.SwipeType.Right)
		{
			characterState = CharacterState.Flipping;
			anim.Play("forward_charge");
			anim.AnimationCompleted = FlipCompleteDelegate;
			Hashtable hash = new Hashtable();
			hash.Add("from", level.rotateSpeed);
			hash.Add("to", 5f);
			hash.Add("onupdate","SpeedUpdate");
			hash.Add("oncomplete","SpeedUpEnd");
			hash.Add("time",0.5f);
			iTween.ValueTo(gameObject,hash);

			Hashtable hash2 = new Hashtable();
			hash2.Add("position",new Vector3(camTargetInitialPos.x-2,camTargetInitialPos.y,camTargetInitialPos.z));
			hash2.Add("islocal",true);
			hash2.Add("time",0.5f);
			iTween.MoveTo(camTarget,hash2);
		}
		else if (gesture.swipe == EasyTouch.SwipeType.Left)
		{
			characterState = CharacterState.Flipping;
			anim.Play("flip_backward");
			anim.AnimationCompleted = FlipCompleteDelegate;
			Hashtable hash = new Hashtable();
			hash.Add("from", level.rotateSpeed);
			hash.Add("to", 1f);
			hash.Add("onupdate","SpeedUpdate");
			hash.Add("oncomplete","SlowDownEnd");
			hash.Add("time",0.5f);
			iTween.ValueTo(gameObject,hash);

			Hashtable hash2 = new Hashtable();
			hash2.Add("position",new Vector3(camTargetInitialPos.x+2,camTargetInitialPos.y,camTargetInitialPos.z));
			hash2.Add("islocal",true);
			hash2.Add("time",0.5f);
			iTween.MoveTo(camTarget,hash2);
		}
	}

	public void FlipCompleteDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip){
		if (anim.DefaultClip != clip)
		{
			//print("derp");
			characterState = CharacterState.Running;
			anim.Play("run");
			//print("punched");
			anim.AnimationEventTriggered = StepEvent;
			anim.AnimationCompleted = null;
			swiping = false;
			jumpFrame = false;
		}
	}

	public void SpeedUpdate(float speedVal)
	{
		level.rotateSpeed = speedVal;
	}

	public void SpeedUpEnd()
	{
		Hashtable hash = new Hashtable();
		hash.Add("from", level.rotateSpeed);
		hash.Add("to", 3f);
		hash.Add("onupdate","SpeedUpdate");
		hash.Add("time",3f);
		iTween.ValueTo(gameObject,hash);

		Hashtable hash2 = new Hashtable();
		hash2.Add("position",camTargetInitialPos);
		hash2.Add("islocal",true);
		hash2.Add("time",3f);
		iTween.MoveTo(camTarget,hash2);
	}

	public void SlowDownEnd()
	{
		Hashtable hash = new Hashtable();
		hash.Add("from", level.rotateSpeed);
		hash.Add("to", 3f);
		hash.Add("onupdate","SpeedUpdate");
		hash.Add("time",3f);
		iTween.ValueTo(gameObject,hash);

		Hashtable hash2 = new Hashtable();
		hash2.Add("position",camTargetInitialPos);
		hash2.Add("islocal",true);
		hash2.Add("time",3f);
		iTween.MoveTo(camTarget,hash2);
	}
}
