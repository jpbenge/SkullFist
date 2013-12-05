using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class iGuiNumericBinding : iGuiBinding
{
	private Dictionary<System.Type, EZData.Property> _properties =
		new Dictionary<System.Type, EZData.Property>();
	
	private bool _ignoreChanges;
	
	public override void Awake()
	{
		base.Awake();
	}
	
	public override void UpdateBinding()
	{
		foreach(var p in _properties)
			p.Value.OnChange -= OnChange;
		
		_properties.Clear();
			
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("iGuiNumericBinding.UpdateBinding - context is null");
			return;
		}
		
		_properties.Add(typeof(float), context.FindProperty<float>(Path, this));
		_properties.Add(typeof(double), context.FindProperty<double>(Path, this));
		_properties.Add(typeof(decimal), context.FindProperty<decimal>(Path, this));
		_properties.Add(typeof(long), context.FindProperty<long>(Path, this));
		_properties.Add(typeof(ulong), context.FindProperty<ulong>(Path, this));
		_properties.Add(typeof(int), context.FindProperty<int>(Path, this));
		_properties.Add(typeof(uint), context.FindProperty<uint>(Path, this));
		_properties.Add(typeof(short), context.FindProperty<short>(Path, this));
		_properties.Add(typeof(ushort), context.FindProperty<ushort>(Path, this));
		_properties.Add(typeof(sbyte), context.FindProperty<sbyte>(Path, this));
		_properties.Add(typeof(byte), context.FindProperty<byte>(Path, this));
		
		var nullKeys = new List<System.Type>();
		foreach (var p in _properties)
		{
			if (p.Value == null)
				nullKeys.Add(p.Key);
		}
		foreach(var k in nullKeys)
			_properties.Remove(k);
		
		foreach(var p in _properties)
			p.Value.OnChange += OnChange;
		
		OnChange();
	}
	
	private double GetNumericValue()
	{
		if (_properties.ContainsKey(typeof(double)))
			return ((EZData.Property<double>)(_properties[typeof(double)])).GetValue();
		if (_properties.ContainsKey(typeof(float)))
			return ((EZData.Property<float>)(_properties[typeof(float)])).GetValue();
		if (_properties.ContainsKey(typeof(decimal)))
			return (double)((EZData.Property<decimal>)(_properties[typeof(decimal)])).GetValue();
		if (_properties.ContainsKey(typeof(long)))
			return ((EZData.Property<long>)(_properties[typeof(long)])).GetValue();
		if (_properties.ContainsKey(typeof(UInt64)))
			return ((EZData.Property<ulong>)(_properties[typeof(ulong)])).GetValue();
		if (_properties.ContainsKey(typeof(int)))
			return ((EZData.Property<int>)(_properties[typeof(int)])).GetValue();
		if (_properties.ContainsKey(typeof(uint)))
			return ((EZData.Property<uint>)(_properties[typeof(uint)])).GetValue();
		if (_properties.ContainsKey(typeof(short)))
			return ((EZData.Property<short>)(_properties[typeof(short)])).GetValue();
		if (_properties.ContainsKey(typeof(ushort)))
			return ((EZData.Property<ushort>)(_properties[typeof(ushort)])).GetValue();
		if (_properties.ContainsKey(typeof(sbyte)))
			return ((EZData.Property<sbyte>)(_properties[typeof(sbyte)])).GetValue();
		if (_properties.ContainsKey(typeof(byte)))
			return ((EZData.Property<byte>)(_properties[typeof(byte)])).GetValue();
		return 0;
	}
	
	protected void SetNumericValue(double val)
	{
		if (_properties.ContainsKey(typeof(double)))
			((EZData.Property<double>)(_properties[typeof(double)])).SetValue(val);
		if (_properties.ContainsKey(typeof(float)))
			((EZData.Property<float>)(_properties[typeof(float)])).SetValue((float)val);
		if (_properties.ContainsKey(typeof(decimal)))
			((EZData.Property<decimal>)(_properties[typeof(decimal)])).SetValue((decimal)val);
		if (_properties.ContainsKey(typeof(long)))
			((EZData.Property<long>)(_properties[typeof(long)])).SetValue((long)val);
		if (_properties.ContainsKey(typeof(ulong)))
			((EZData.Property<ulong>)(_properties[typeof(ulong)])).SetValue((ulong)val);
		if (_properties.ContainsKey(typeof(int)))
			((EZData.Property<int>)(_properties[typeof(int)])).SetValue((int)val);
		if (_properties.ContainsKey(typeof(uint)))
			((EZData.Property<uint>)(_properties[typeof(uint)])).SetValue((uint)val);
		if (_properties.ContainsKey(typeof(short)))
			((EZData.Property<short>)(_properties[typeof(short)])).SetValue((short)val);
		if (_properties.ContainsKey(typeof(ushort)))
			((EZData.Property<ushort>)(_properties[typeof(ushort)])).SetValue((ushort)val);
		if (_properties.ContainsKey(typeof(sbyte)))
			((EZData.Property<sbyte>)(_properties[typeof(sbyte)])).SetValue((sbyte)val);
		if (_properties.ContainsKey(typeof(byte)))
			((EZData.Property<byte>)(_properties[typeof(byte)])).SetValue((byte)val);
	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		if (_properties.Count == 0)
			return;
		
		_ignoreChanges = true;
		ApplyNewValue(GetNumericValue());
		_ignoreChanges = false;
	}
	
	protected abstract void ApplyNewValue(double val);
}
