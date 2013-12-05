using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Enabled Binding")]
public class iGuiEnabledBinding : iGuiBooleanBinding
{
	private iGUI.iGUIElement[] _elements;
	private iGuiEnabledBinding _parentVisibility;
	private bool _selfVisible;
	
	private bool Visible
	{
		get 
		{
			if (_parentVisibility != null)
				return _selfVisible && _parentVisibility.Visible;
			return _selfVisible;
		}	
	}
	
	public override void Awake()
	{
		base.Awake();
		_parentVisibility = iGuiUtils.GetComponentInParents<iGuiEnabledBinding>(gameObject.transform.parent.gameObject);
		
		_elements = GetComponents<iGUI.iGUIElement>();
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		_selfVisible = newValue;
		SetValue(gameObject, Visible);
	}
		
	private void SetValue(GameObject gameObject, bool enabled)
	{
		foreach(var e in _elements)
		{
			e.setEnabled(enabled);
		}
		
		for (var i = 0; i < gameObject.transform.childCount; ++i)
		{
			var child = gameObject.transform.GetChild(i).gameObject;
			var childVisibilityBinding = child.GetComponent<iGuiEnabledBinding>();
			if (childVisibilityBinding != null)
			{
				childVisibilityBinding.SetValue(child, enabled && childVisibilityBinding._selfVisible);
			}
			else
			{
				SetValue(child, enabled);
			}
		}
	}
}
