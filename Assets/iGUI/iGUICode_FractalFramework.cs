using UnityEngine;
using System.Collections;
using iGUI;

public class iGUICode_FractalFramework : MonoBehaviour{
	[HideInInspector]
	public iGUIButton button5;
	[HideInInspector]
	public iGUIButton button1;
	[HideInInspector]
	public iGUIButton button3;
	[HideInInspector]
	public iGUIMyTile myTile1;
	[HideInInspector]
	public iGUIContainer SpriteHolder;
	[HideInInspector]
	public iGUILabel label1;
	[HideInInspector]
	public iGUIContainer enemy;
	[HideInInspector]
	public iGUIListBox listBox1;
	[HideInInspector]
	public iGUIImage image1;
	[HideInInspector]
	public iGUIRoot root1;
	


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
	
	static iGUICode_FractalFramework instance;
	public iDataFractalFrameworkViewModel ViewModel;
	
	void Awake(){
		instance=this;
	
	}
	public static iGUICode_FractalFramework getInstance(){
		return instance;
	}
    //
	
	
	public void image1_Init(iGUIImage caller){
		
	}

	public void SpriteHolder_Init(iGUIContainer caller){	
		spriteCollectionInstance = iGUICode_FractalFramework.getInstance().spriteCollectionInstance;
		spriteInstance = caller.GetComponent<tk2dAnimatedSprite>();
		spriteInstance.SetSprite(spriteCollectionInstance, ViewModel.Context.NextEnemy);
	}

	public void button1_Click(iGUIButton caller){
		ViewModel.Context.AddItemA();
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
	

	public void button5_Init(iGUIButton caller){
		
	}

	public void button5_Enable(iGUIButton caller){
		
	}

	public void button5_ClickDown(iGUIButton caller){
		
	}

}
