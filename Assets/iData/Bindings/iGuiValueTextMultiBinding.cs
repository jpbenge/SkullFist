using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Value Text Multi-Binding")]
public class iGuiValueTextMultiBinding : iGuiTextMultiBinding
{
	private iGUI.iGUITextarea [] _textAreas;
	private iGUI.iGUITextfield [] _textFields;
	private iGUI.iGUIPasswordField [] _passwordFields;
	private iGUI.iGUINumberField [] _numberFields;
	
	public override void Awake()
	{
		base.Awake();
		
		_textAreas = GetComponents<iGUI.iGUITextarea>();
		_textFields = GetComponents<iGUI.iGUITextfield>();
		_passwordFields = GetComponents<iGUI.iGUIPasswordField>();
		_numberFields = GetComponents<iGUI.iGUINumberField>();
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	protected override void SetControlText (string text)
	{
		foreach(var e in _textAreas)
		{
			e.value = text;
		}
		foreach(var e in _textFields)
		{
			e.value = text;
		}
		foreach(var e in _passwordFields)
		{
			e.value = text;
		}
		foreach(var e in _numberFields)
		{
			float f = 0;
			if (!float.TryParse(text, out f))
				f = 0;
			e.value = f;
		}
	}
}
