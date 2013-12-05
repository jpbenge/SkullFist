using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Color Binding")]
public class iGuiColorBinding : iGuiBinding
{
	private bool _ignoreChanges = false;
	
	private EZData.Property<string> _hexProperty = null;
	private EZData.Property<Color> _colorProperty = null;
	
	protected iGUI.iGUIElement [] _elements;
	
    private static int ToColorComponent(float c)
    {
        return (int) (Math.Max(0, Math.Min(c, 255)));
    }
	
	private static string ColorToHex(Color c)
	{
		if (c.a > 254)
    		return string.Format("#{0:x2}{1:x2}{2:x2}", ToColorComponent(c.r), ToColorComponent(c.g), ToColorComponent(c.b));
		else
			return string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", ToColorComponent(c.r), ToColorComponent(c.g), ToColorComponent(c.b), ToColorComponent(c.a));
	}
	
	private static bool HexToColor(string h, out Color c)
	{
		c = Color.white;
		
		if (h == null)
			h = "0";
		if (h.Length > 0 && h[0] == '#')
			h = h.Substring(1);
		if (h.Length > 1 && h[0] == '[' && h[h.Length - 1] == ']')
			h = h.Substring(1, h.Length - 2);
		if (h.Length > 8)
			h = h.Substring(0, 8);
		while(h.Length < 6)
			h += "0";
		if (h.Length > 6)
			while(h.Length < 8)
				h += "0";
        int color;
		if (!int.TryParse(h, System.Globalization.NumberStyles.HexNumber, null, out color))
            return false;
        
		var b0 = (color >> 24) & 0xff;
        var b1 = (color >> 16) & 0xff;
        var b2 = (color >> 8) & 0xff;
        var b3 = (color) & 0xff;
		
		if (h.Length == 6)
			c = new Color(b1 / 255.0f, b2 / 255.0f, b3 / 255.0f, 1.0f);
		else
			c = new Color(b0 / 255.0f, b1 / 255.0f, b2 / 255.0f, b3 / 255.0f);
		return true;
    }
    
	public override void Awake()
	{
		base.Awake();
		_elements = GetComponents<iGUI.iGUIElement>();
	}
		
	public override void UpdateBinding()
	{
		if (_hexProperty != null)
		{
			_hexProperty.OnChange -= OnChange;
			_hexProperty = null;
		}
		if (_colorProperty != null)
		{
			_colorProperty.OnChange -= OnChange;
			_colorProperty = null;
		}
		
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("iGuiColorBinding.UpdateBinding - context is null");
			return;
		}
		
		_colorProperty = context.FindProperty<Color>(Path, this);
		if (_colorProperty == null)
			_hexProperty = context.FindProperty<string>(Path, this);
		
		if (_colorProperty != null)
			_colorProperty.OnChange += OnChange;
		if (_hexProperty != null)
			_hexProperty.OnChange += OnChange;

		OnChange();
	}
	
	public void OnUIColorChange(Color color)
	{
		_ignoreChanges = true;
		if (_colorProperty != null)
			_colorProperty.SetValue(color);
		if (_hexProperty != null)
			_hexProperty.SetValue(ColorToHex(color));
		_ignoreChanges = false;
	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		Color newColor = Color.white;
		if (_colorProperty != null)
			newColor = _colorProperty.GetValue();
		else if (_hexProperty != null && !HexToColor(_hexProperty.GetValue(), out newColor))
			return;
		
		if (_ignoreChanges)
			return;
		
		ApplyNewColor(newColor);
	}
	
	protected virtual void ApplyNewColor(Color newValue)
	{
		foreach(var e in _elements)
		{
			e.setColor(newValue);
		}
	}
}
