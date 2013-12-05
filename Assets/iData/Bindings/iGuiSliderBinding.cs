using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Slider Binding")]
public class iGuiSliderBinding : iGuiNumericBinding
{
	private iGUI.iGUIFloatVerticalSlider [] _floatVerticals;
	private iGUI.iGUIFloatHorizontalSlider [] _floatHorizontals;
	private iGUI.iGUIIntVerticalSlider [] _intVerticals;
	private iGUI.iGUIIntHorizontalSlider [] _intHorizontals;
	private iGUI.iGUIKnob [] _knobs;
	private iGUI.iGUIProgressBar [] _progressBars;
	private iGUI.iGUIGauge [] _gauges;
	
	public override void Awake()
	{
		base.Awake();
		
		_floatVerticals = gameObject.GetComponents<iGUI.iGUIFloatVerticalSlider>();
		foreach(var s in _floatVerticals)
			s.valueChangeCallback += OnFloatVerticalChange;
		
		_floatHorizontals = gameObject.GetComponents<iGUI.iGUIFloatHorizontalSlider>();
		foreach(var s in _floatHorizontals)
			s.valueChangeCallback += OnFloatHorizontalChange;

		_intVerticals = gameObject.GetComponents<iGUI.iGUIIntVerticalSlider>();
		foreach(var s in _intVerticals)
			s.valueChangeCallback += OnIntVerticalChange;
		
		_intHorizontals = gameObject.GetComponents<iGUI.iGUIIntHorizontalSlider>();
		foreach(var s in _intHorizontals)
			s.valueChangeCallback += OnIntHorizontalChange;

		_knobs = gameObject.GetComponents<iGUI.iGUIKnob>();
		foreach(var s in _knobs)
			s.valueChangeCallback += OnKnobChange;

		_progressBars = gameObject.GetComponents<iGUI.iGUIProgressBar>();

		_gauges = gameObject.GetComponents<iGUI.iGUIGauge>();
	}
	
	private void OnFloatVerticalChange(iGUI.iGUIElement e)
	{
		SetNumericValue(((iGUI.iGUIFloatVerticalSlider)e).value);
	}
	
	private void OnFloatHorizontalChange(iGUI.iGUIElement e)
	{
		SetNumericValue(((iGUI.iGUIFloatHorizontalSlider)e).value);
	}
	
	private void OnIntVerticalChange(iGUI.iGUIElement e)
	{
		SetNumericValue(((iGUI.iGUIIntVerticalSlider)e).value);
	}
	
	private void OnIntHorizontalChange(iGUI.iGUIElement e)
	{
		SetNumericValue(((iGUI.iGUIIntHorizontalSlider)e).value);
	}
	
	private void OnKnobChange(iGUI.iGUIElement e)
	{
		SetNumericValue(((iGUI.iGUIKnob)e).value);
	}
	
	protected override void ApplyNewValue(double val)
	{
		foreach(var s in _floatVerticals)
			s.setValue((float)val);
		foreach(var s in _floatHorizontals)
			s.setValue((float)val);
		foreach(var s in _intVerticals)
			s.setValue(Mathf.RoundToInt((float)val));
		foreach(var s in _intHorizontals)
			s.setValue(Mathf.RoundToInt((float)val));
		foreach(var s in _knobs)
			s.setValue((float)val);
		foreach(var s in _progressBars)
			s.value = ((float)val);
		foreach(var s in _gauges)
			s.value = ((float)val);
	}
}
