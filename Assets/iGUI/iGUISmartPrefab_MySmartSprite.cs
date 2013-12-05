/*
using UnityEngine;
using System.Collections;
using iGUI;

public class iGUISmartPrefab_MySmartSprite : MonoBehaviour{
	public iGUIContainer SpriteHolder;
	[HideInInspector]
	public iGUIMyTile myTile1;
	[HideInInspector]
	public iGUIContainer container1;
	
	
    tk2dBaseSprite spriteInstance = null;
    tk2dSpriteCollectionData spriteCollectionInstance = null;
	
	//
	static iGUISmartPrefab_MySmartSprite instance;
	void Awake(){
		instance=this;
		int ord = SpriteHolder.order;
			Debug.Log(ord);
	}
	public static iGUISmartPrefab_MySmartSprite getInstance(){
		return instance;
	}
    //
	
	
	public void image1_Init(iGUIImage caller){
					
		spriteCollectionInstance = iGUICode_FractalFramework.getInstance().spriteCollectionInstance;
		    spriteInstance = caller.GetComponentInChildren<tk2dSprite>();
			spriteInstance.SetSprite(spriteCollectionInstance, "sun");
		print ("000000000000000000");
	}

}
*/

using UnityEngine;
using System.Collections;
using iGUI;

public class iGUISmartPrefab_MySmartSprite : MonoBehaviour{

 [HideInInspector]
 public iGUIContainer SpriteHolder;
  
  
 // Link to the animated sprite
  private tk2dAnimatedSprite anim;

  // State variable to see if the character is walking.
  private bool walking = false;
  
 //
 static iGUISmartPrefab_MySmartSprite instance;
 void Awake(){
 	instance=this;
 	 
 	 
 }
 public static iGUISmartPrefab_MySmartSprite getInstance(){
 	return instance;
 }
  
 public void SpriteHolder_Init(){
 	 
 	Debug.Log (SpriteHolder.order);
 	 
 	 
 }


  // Use this for initialization
  void Start () {
  // This script must be attached to the sprite to work.
  anim = GetComponent<tk2dAnimatedSprite>();
  }

  // This is called once the hit animation has compelted playing
  // It returns to playing whatever animation was active before hit
  // was playing.
  void HitCompleteDelegate(tk2dAnimatedSprite sprite, int clipId) {
  if (walking) {
  anim.Play("walk");
  } 
  else {
  anim.Play("idle");
  }
  }

  // Update is called once per frame
  void Update () {
  if (Input.GetKey(KeyCode.A)) {
  // Only play the clip if it is not already playing.
  // Calling play will restart the clip if it is already playing.
  if (!anim.IsPlaying("hit")) {
  anim.Play("hit");

  // The delegate is used here to return to the previously
  // playing clip after the "hit" animation is done playing.
  anim.animationCompleteDelegate = HitCompleteDelegate;
  }
  }

  if (Input.GetKey(KeyCode.D)) {
  if (!anim.IsPlaying("walk")) {

  // Walk is a looping animation
  // A looping animation never completes...
  anim.Play("walk");

  // We dont have any reason for detecting when it completes
  anim.animationCompleteDelegate = null;
  walking = true;
  }
  }

  if (Input.GetKey(KeyCode.W)) {
  if (!anim.IsPlaying("idle")) {
  anim.Play("idle");
  anim.animationCompleteDelegate = null;
  walking = false;
  }
  }
  }
}