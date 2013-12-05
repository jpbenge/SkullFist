using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Readonly Binding")]
public class iGuiReadonlyBinding : iGuiBooleanBinding
{
	private iGUI.iGUITextarea [] _textAreas;
	private iGUI.iGUITextfield [] _textFields;
	private iGUI.iGUIPasswordField [] _passwordFields;
	private iGUI.iGUINumberField [] _numberFields;
	private iGUI.iGUISwitch [] _switches;
	private iGUI.iGUIDropDownList [] _dropDownLists;
	private iGUI.iGUIFloatVerticalSlider [] _floatVerticals;
	private iGUI.iGUIFloatHorizontalSlider [] _floatHorizontals;
	private iGUI.iGUIIntVerticalSlider [] _intVerticals;
	private iGUI.iGUIIntHorizontalSlider [] _intHorizontals;
	
	public override void Awake()
	{
		base.Awake();
		
		_textAreas = GetComponents<iGUI.iGUITextarea>();
		_textFields = GetComponents<iGUI.iGUITextfield>();
		_passwordFields = GetComponents<iGUI.iGUIPasswordField>();
		_numberFields = GetComponents<iGUI.iGUINumberField>();
		_switches = GetComponents<iGUI.iGUISwitch>();
		_dropDownLists = GetComponents<iGUI.iGUIDropDownList>();
		_floatVerticals = GetComponents<iGUI.iGUIFloatVerticalSlider>();
		_floatHorizontals = GetComponents<iGUI.iGUIFloatHorizontalSlider>();
		_intVerticals = GetComponents<iGUI.iGUIIntVerticalSlider>();
		_intHorizontals = GetComponents<iGUI.iGUIIntHorizontalSlider>();
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		foreach(var e in _textAreas)			e.readOnly = newValue;
		foreach(var e in _textFields)			e.readOnly = newValue;
		foreach(var e in _passwordFields)		e.readOnly = newValue;
		foreach(var e in _numberFields)			e.readOnly = newValue;
		foreach(var e in _switches)				e.readOnly = newValue;
		foreach(var e in _dropDownLists)		e.readOnly = newValue;
		foreach(var e in _floatVerticals)		e.readOnly = newValue;
		foreach(var e in _floatHorizontals)		e.readOnly = newValue;
		foreach(var e in _intVerticals)			e.readOnly = newValue;
		foreach(var e in _intHorizontals)		e.readOnly = newValue;
	}
}
