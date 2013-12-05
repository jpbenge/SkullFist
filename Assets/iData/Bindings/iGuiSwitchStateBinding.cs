using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Switch State Binding")]
public class iGuiSwitchStateBinding : iGuiBooleanBinding
{
	private iGUI.iGUISwitch _switch;
	private bool _ignoreChanges;
	
	public override void Awake()
	{
		base.Awake();
		_switch = GetComponent<iGUI.iGUISwitch>();
		if (_switch != null)
			_switch.valueChangeCallback += OnChange;
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	private void OnChange(iGUI.iGUIElement e)
	{
		if (_ignoreChanges)
			return;
		
		if (_switch != null)
		{
			_ignoreChanges = true;
			ApplyInputValue(_switch.value);
			_ignoreChanges = false;
		}
	}
	
	protected override void ApplyNewValue(bool newValue)
	{		
		if (_ignoreChanges)
			return;
		
		if (_switch != null)
		{
			_ignoreChanges = true;
			_switch.setValue(newValue);
			_switch.value = newValue;
			_ignoreChanges = false;
		}
	}
}
