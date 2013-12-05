using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Texture Binding")]
public abstract class iGuiTextureBinding : iGuiBinding
{
	private EZData.Property<Texture2D> _texture;
	
	public override void Awake()
	{
		base.Awake();
	}
	
	public override void UpdateBinding()
	{
		if (_texture != null)
		{
			_texture.OnChange -= OnChange;
			_texture = null;
		}
		
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("iGuiTexture.UpdateBinding - context is null");
			return;
		}
		
		_texture = context.FindProperty<Texture2D>(Path, this);
		
		if (_texture != null)
		{
			_texture.OnChange += OnChange;
		}
		OnChange();
	}
	
	public void OnChange()
	{
		ApplyNewValue((_texture != null) ? _texture.GetValue() : null);
	}
	
	protected abstract void ApplyNewValue(Texture2D newValue);
}
