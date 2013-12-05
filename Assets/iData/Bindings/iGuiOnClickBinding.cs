using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/OnClick Binding")]
public class iGuiOnClickBinding : iGuiCommandBinding
{
	public override void Awake()
	{
		base.Awake();
		
		foreach(var b in GetComponents<iGUI.iGUIButton>())
		{
			b.clickCallback += OnClick;
		}
	}
	
	void OnClick(iGUI.iGUIElement element)
	{
		if (_command == null)
		{
			return;
		}
		
		_command.DynamicInvoke();
	}
}
