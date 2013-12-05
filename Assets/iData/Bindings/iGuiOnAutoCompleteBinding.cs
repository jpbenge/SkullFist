using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/OnAutoComplete Binding")]
public class iGuiOnAutoCompleteBinding : iGuiCommandBinding
{
	public float interval = 1f;
	public bool suggestForEmptyInput = false;
	
	private float _time;
	private string _lastText;
	
	public override void Awake()
	{
		base.Awake();
		
		foreach(var e in GetComponents<iGUI.iGUITextarea>())
			e.valueChangeCallback += OnTextareaChange;
		
		foreach(var e in GetComponents<iGUI.iGUITextfield>())
			e.valueChangeCallback += OnTextfieldChange;
		
		foreach(var e in GetComponents<iGUI.iGUIPasswordField>())
			e.valueChangeCallback += OnPasswordFieldChange;

		foreach(var e in GetComponents<iGUI.iGUINumberField>())
			e.valueChangeCallback += OnNumberFieldChange;
	}
	
	void OnTextareaChange(iGUI.iGUIElement e)
	{
		OnChange(((iGUI.iGUITextarea)e).value);
	}
	
	void OnTextfieldChange(iGUI.iGUIElement e)
	{
		OnChange(((iGUI.iGUITextfield)e).value);
	}
	
	void OnPasswordFieldChange(iGUI.iGUIElement e)
	{
		OnChange(((iGUI.iGUIPasswordField)e).value);
	}
	
	void OnNumberFieldChange(iGUI.iGUIElement e)
	{
		OnChange(((iGUI.iGUINumberField)e).value.ToString());
	}
	
	void OnChange(string text)
	{
		_lastText = text;
		if (_command != null && (!string.IsNullOrEmpty(text) || suggestForEmptyInput))
		{
			_time = interval;
		}		
	}
	
	void Update()
	{
		if (_time > 0f)
		{
			_time -= Time.deltaTime;
			if (_time < 0f)
			{
				_time = 0f;
				if (_command != null && (!string.IsNullOrEmpty(_lastText) || suggestForEmptyInput))
				{
					_command.DynamicInvoke();
				}
			}
		}
	}
}
