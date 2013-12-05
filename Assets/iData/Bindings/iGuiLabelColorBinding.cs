using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Label Color Binding")]
public class iGuiLabelColorBinding : iGuiColorBinding
{
	public override void Awake()
	{
		base.Awake();
	}
		
	protected override void ApplyNewColor(Color newValue)
	{
		foreach(var e in _elements)
		{
			e.setLabelColor(newValue);
		}
	}
}
