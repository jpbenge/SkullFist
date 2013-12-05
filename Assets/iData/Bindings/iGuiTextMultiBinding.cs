using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class iGuiTextMultiBinding : iGuiMultiBinding
{
	public string Format = "";
	
	private readonly List<Dictionary<Type, EZData.Property>> _propertyGroups = new List<Dictionary<Type, EZData.Property>>();
	
	private bool _ignoreChanges = false;
		
	protected abstract void SetControlText(string text);
	
	public override void Awake()
	{
		base.Awake();
		
		for (var i = 0; i < Paths.Count; ++i)
		{
			var properties = new Dictionary<Type, EZData.Property>();
			properties.Add(typeof(string), null);
			properties.Add(typeof(int), null);
			properties.Add(typeof(float), null);
			properties.Add(typeof(double), null);
			properties.Add(typeof(decimal), null);
			properties.Add(typeof(DateTime), null);
			
			_propertyGroups.Add(properties);
		}
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	public override void UpdateBinding()
	{
		foreach(var g in _propertyGroups)
		{
			foreach(var p in g)
			{
				if (p.Value != null)
				{
					p.Value.OnChange -= OnChange;
					g[p.Key] = null;
					break;
				}
			}
		}
			
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("iGuiTextBinding.UpdateBinding - context is null");
			return;
		}
		
		for (var i = 0; i < _propertyGroups.Count && i < Paths.Count; ++i)
		{
			_propertyGroups[i][typeof(string)] = context.FindProperty<string>(Paths[i], this);
			_propertyGroups[i][typeof(int)] = context.FindProperty<int>(Paths[i], this);
			_propertyGroups[i][typeof(float)] = context.FindProperty<float>(Paths[i], this);
			_propertyGroups[i][typeof(double)] = context.FindProperty<double>(Paths[i], this);
			_propertyGroups[i][typeof(decimal)] = context.FindProperty<decimal>(Paths[i], this);
			_propertyGroups[i][typeof(DateTime)] = context.FindProperty<DateTime>(Paths[i], this);
		}
		
		foreach(var g in _propertyGroups)
		{
			foreach(var p in g)
			{
				if (p.Value != null)
				{
					p.Value.OnChange += OnChange;
				}
			}
		}
		OnChange();
	}
		
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		var newValues = new List<object>();
		
		foreach(var g in _propertyGroups)
		{
			if (g[typeof(string)] != null)
				newValues.Add(((EZData.Property<string>)g[typeof(string)]).GetValue());
			else if (g[typeof(int)] != null)
				newValues.Add(((EZData.Property<int>)g[typeof(int)]).GetValue());
			else if (g[typeof(float)] != null)
				newValues.Add(((EZData.Property<float>)g[typeof(float)]).GetValue());
			else if (g[typeof(double)] != null)
				newValues.Add(((EZData.Property<double>)g[typeof(double)]).GetValue());
			else if (g[typeof(decimal)] != null)
				newValues.Add(((EZData.Property<decimal>)g[typeof(decimal)]).GetValue());
			else if (g[typeof(DateTime)] != null)
				newValues.Add(((EZData.Property<DateTime>)g[typeof(DateTime)]).GetValue());
			else
				newValues.Add("");
		}
		
		var newValue = string.Format(Format, newValues.ToArray());
		
		SetControlText(newValue);
	}
}
