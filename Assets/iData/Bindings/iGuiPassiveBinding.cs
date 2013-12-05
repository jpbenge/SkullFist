using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Passive Binding")]
public class iGuiPassiveBinding : iGuiBooleanBinding
{
	private iGUI.iGUIElement[] _elements;
	private iGuiPassiveBinding _parentPassivity;
	private bool _selfPassive;
	
	private bool Passive
	{
		get 
		{
			if (_parentPassivity != null)
				return _selfPassive && _parentPassivity.Passive;
			return _selfPassive;
		}	
	}
	
	public override void Awake()
	{
		base.Awake();
		_parentPassivity = iGuiUtils.GetComponentInParents<iGuiPassiveBinding>(gameObject.transform.parent.gameObject);
		
		_elements = GetComponents<iGUI.iGUIElement>();
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		_selfPassive = newValue;
		SetValue(gameObject, Passive);
	}
	
	private void SetValue(GameObject gameObject, bool passive)
	{
		foreach(var e in _elements)
		{
			e.passive = passive;
		}
		
		for (var i = 0; i < gameObject.transform.childCount; ++i)
		{
			var child = gameObject.transform.GetChild(i).gameObject;
			var childPassiveBinding = child.GetComponent<iGuiPassiveBinding>();
			if (childPassiveBinding != null)
			{
				childPassiveBinding.SetValue(child, passive && childPassiveBinding._selfPassive);
			}
			else
			{
				SetValue(child, passive);
			}
		}
	}
}
