using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Hover Image Texture Binding")]
public class iGuiHoverImageTextureBinding : iGuiTextureBinding
{
	private iGUI.iGUIImage _image;
	
	public override void Awake()
	{
		base.Awake();
		_image = gameObject.GetComponent<iGUI.iGUIImage>();
	}
	
	protected override void ApplyNewValue(Texture2D newValue)
	{
		if (_image != null)
		{
			_image.hoverImage = newValue;
		}
	}
}
