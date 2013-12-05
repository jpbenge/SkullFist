using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Label Texture Binding")]
public class iGuiLabelTextureBinding : iGuiTextureBinding
{
	private iGUI.iGUIElement _element;
	
	public override void Awake()
	{
		base.Awake();
		_element = gameObject.GetComponent<iGUI.iGUIElement>();
	}
	
	protected override void ApplyNewValue(Texture2D newValue)
	{
		if (_element != null)
		{
			_element.label.image = newValue;
		}
	}
}
