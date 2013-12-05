using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Clock Time Binding")]
public class iGuiClockTimeBinding : iGuiBinding
{
	private EZData.Property<DateTime> _property;
	
	private iGUI.iGUIClock _clock;
	
	public override void Awake()
	{
		base.Awake();
		
		_clock = GetComponent<iGUI.iGUIClock>();
	}
		
	public override void Start()
	{
		base.Start();
	}
	
	public override void UpdateBinding()
	{
		if (_property != null)
			_property.OnChange -= OnChange;
			
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("iGuiClockTimeBinding.UpdateBinding - context is null");
			return;
		}
		
		_property = context.FindProperty<DateTime>(Path, this);
		
		if (_property != null)
		{
			_property.OnChange += OnChange;
		}
		OnChange();
	}
	
	private void OnChange()
	{
		if (_clock == null || _property == null)
			return;
		
		_clock.useSystemTimeAsInitialTime = false;
		_clock.initialHour = 0;
		_clock.initialMinute = 0;
		_clock.initialSecond = 0;
		
		var val = _property.GetValue();
		
		_clock.time = TimeSpan.FromSeconds(val.Second + val.Minute * 60 + val.Hour * 3600);
	}
}
