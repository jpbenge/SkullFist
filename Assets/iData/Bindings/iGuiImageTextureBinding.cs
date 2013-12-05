using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Image Texture Binding")]
public class iGuiImageTextureBinding : iGuiTextureBinding
{
	private iGUI.iGUIImage _image;
	
	private float width;
	private float height;
	
	public enum ALIGNMENT
	{
		STRETCH,
		UNIFORM_STRETCH,
	}
	
	public ALIGNMENT alignment = ALIGNMENT.UNIFORM_STRETCH;
	
	public override void Awake()
	{
		base.Awake();
		
		_image = gameObject.GetComponent<iGUI.iGUIImage>();
		if (_image != null)
		{
			width = _image.scale.x;
			height = _image.scale.y;
		}
	}
	
	protected override void ApplyNewValue(Texture2D newValue)
	{
		var aspect = (height == 0) ? 1 : (width / height);
		
		var imageWidth = width;
		var imageHeight = height;
		
		if (newValue != null)
		{
			imageWidth = newValue.width;
			imageHeight = newValue.height;
		}
		
		var imageAspect = (imageHeight == 0) ? 1 : (imageWidth / imageHeight);
		
		var spriteWidth = 0.0f;
		var spriteHeight = 0.0f;
		
		if (newValue != null)
		{
			switch(alignment)
			{
			case ALIGNMENT.STRETCH:
				spriteWidth = width;
				spriteHeight = height;
				break;
			case ALIGNMENT.UNIFORM_STRETCH:
				if (aspect > imageAspect)
				{
					spriteHeight = height;
					spriteWidth = (imageHeight == 0) ? 0 : (imageWidth * spriteHeight / imageHeight);
				}
				else
				{
					spriteWidth = width;
					spriteHeight = (imageWidth == 0) ? 0 : (imageHeight * spriteWidth / imageWidth);
				}
				break;
			}
		}
		
		if (_image != null)
		{
			_image.image = newValue;
			if (alignment == ALIGNMENT.UNIFORM_STRETCH)
				_image.setAspectRatio(imageAspect);
		}
	}
}
