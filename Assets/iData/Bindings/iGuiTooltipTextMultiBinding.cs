using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Tooltip Text Multi-Binding")]
public class iGuiTooltipTextMultiBinding : iGuiTextMultiBinding
{
	private iGUI.iGUIElement[] _elements;
	
	public override void Awake()
	{
		base.Awake();
		_elements = GetComponents<iGUI.iGUIElement>();
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	protected override void SetControlText (string text)
	{
		foreach(var e in _elements)
		{
			e.label.tooltip = text;
		}
	}
}
