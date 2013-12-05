using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using iGUI;

/// <summary>
/// Tiles the given texture. Don't forget to set up your texture correctly
/// Texture Type : Advanced  -  NonPowerOf2 : ToNearest  -  WrapMode : Repeat
/// </summary>
public class iGUIMyTile : iGUICustomElement {
	
	public Texture texture;	
	
	protected override void customDraw (){
		if(texture!=null){
			GUI.DrawTextureWithTexCoords(rect, texture, new Rect(0, 0, rect.width / texture.width, rect.height / texture.height));
		}
	}
}
