using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Color Picker Binding")]
public class iGuiColorPickerBinding : iGuiColorBinding
{
	private iGUI.iGUIColorPicker [] _colorPickers;
	
	public override void Awake()
	{
		base.Awake();
		
		_colorPickers = GetComponents<iGUI.iGUIColorPicker>();
		foreach(var cp in _colorPickers)
		{
			cp.valueChangeCallback += OnChange;
		}
	}
	
	private void OnChange(iGUI.iGUIElement e)
	{
		var c = ((iGUI.iGUIColorPicker)e).value;
		c.r *= 255;
		c.g *= 255;
		c.b *= 255;
		c.a *= 255;
		OnUIColorChange(c);
	}
	
	protected override void ApplyNewColor(Color newValue)
	{
		foreach(var cp in _colorPickers)
		{
			cp.setValue(newValue);
		}
	}
}
