using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Value Text Binding")]
public class iGuiValueTextBinding : iGuiTextBinding
{
	protected Dictionary<iGUI.iGUITextarea, string> _textAreas = new Dictionary<iGUI.iGUITextarea, string>();
	protected Dictionary<iGUI.iGUITextfield, string> _textFields = new Dictionary<iGUI.iGUITextfield, string>();
	protected Dictionary<iGUI.iGUIPasswordField, string> _passwordFields = new Dictionary<iGUI.iGUIPasswordField, string>();
	protected Dictionary<iGUI.iGUINumberField, float> _numberFields = new Dictionary<iGUI.iGUINumberField, float>();
	
	private readonly List<iGUI.iGUINumberField> _invalidKeys = new List<iGUI.iGUINumberField>();
	
	public override void Awake()
	{
		base.Awake();
		
		foreach(var e in GetComponents<iGUI.iGUITextarea>())
			_textAreas.Add(e, e.value);
		
		foreach(var e in GetComponents<iGUI.iGUITextfield>())
			_textFields.Add(e, e.value);
		
		foreach(var e in GetComponents<iGUI.iGUIPasswordField>())
			_passwordFields.Add(e, e.value);

		foreach(var e in GetComponents<iGUI.iGUINumberField>())
			_numberFields.Add(e, e.value);
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	protected new void Update()
	{
		foreach(var e in _textAreas)
		{
			if (e.Key.value != e.Value)
			{
				OnTextChangeByControl(e.Key.value);
				_textAreas[e.Key] = e.Key.value;
				break;
			}
		}
		foreach(var e in _textFields)
		{
			if (e.Key.value != e.Value)
			{
				OnTextChangeByControl(e.Key.value);
				_textFields[e.Key] = e.Key.value;
				break;
			}
		}
		foreach(var e in _passwordFields)
		{
			if (e.Key.value != e.Value)
			{
				OnTextChangeByControl(e.Key.value);
				_passwordFields[e.Key] = e.Key.value;
				break;
			}
		}
		foreach(var e in _numberFields)
		{
			if (e.Key.value != e.Value)
			{
				OnTextChangeByControl(e.Key.value.ToString());
				_numberFields[e.Key] = e.Key.value;
				break;
			}
		}
		
		base.Update();
	}
	
	protected override void SetControlText (string text)
	{
		foreach(var e in _textAreas)
		{
			e.Key.value = text;
		}
		foreach(var e in _textFields)
		{
			e.Key.value = text;
		}
		foreach(var e in _passwordFields)
		{
			e.Key.value = text;
		}
		_invalidKeys.Clear();
		foreach(var e in _numberFields)
		{
			float f = 0;
			if (!float.TryParse(text, out f))
			{
				f = 0;
				_invalidKeys.Add(e.Key);
			}
			e.Key.value = f;
		}
		foreach(var k in _invalidKeys)
		{
			_numberFields[k] = 0;
		}
	}
}
