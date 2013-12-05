using UnityEngine;
using System.Collections;
using iGUI;

public class iGUISmartPrefab_MySmartObject : MonoBehaviour{
	[HideInInspector]
	public iGUILabel label1;
	[HideInInspector]
	public iGUIContainer container1;
	
	
	
	//
	static iGUISmartPrefab_MySmartObject instance;
	void Awake(){
		instance=this;
	}
	public static iGUISmartPrefab_MySmartObject getInstance(){
		return instance;
	}
    //
	
	
	public void image1_Init(iGUIImage caller){
		
	}

}
