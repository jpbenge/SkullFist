using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/OnDoubleClick Binding")]
public class iGuiOnDoubleClickBinding : iGuiCommandBinding
{
	public override void Awake()
	{
		base.Awake();
		
		foreach(var b in GetComponents<iGUI.iGUIButton>())
		{
			b.doubleClickCallback += OnDoubleClick;
		}
	}
	
	void OnDoubleClick(iGUI.iGUIElement element)
	{
		if (_command == null)
		{
			return;
		}
		
		_command.DynamicInvoke();
	}
}
