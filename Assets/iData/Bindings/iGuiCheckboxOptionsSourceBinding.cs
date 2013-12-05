using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Checkbox Options Binding")]
public class iGuiCheckboxOptionsSourceBinding : iGuiItemsSourceBinding
{
	public string LabelTextPath;
	public string LabelImagePath;
	public string LabelTooltipPath;
	public string ValuePath;
	
	private iGUI.iGUICheckboxGroup _checkboxGroup;
	
	private bool _ignoreChanges;
	
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _labelTextsCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _labelTooltipsCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	private readonly Dictionary<EZData.Context, EZData.Property<Texture2D>> _labelImagesCache = new Dictionary<EZData.Context, EZData.Property<Texture2D>>();
	private readonly Dictionary<EZData.Context, EZData.Property<bool>> _valuesCache = new Dictionary<EZData.Context, EZData.Property<bool>>();
	
	public override void Awake()
	{
		base.Awake();
		
		_checkboxGroup = GetComponent<iGUI.iGUICheckboxGroup>();
		_checkboxGroup.valueChangeCallback += OnChange;
	}
	
	public override void Start()
	{
		_checkboxGroup.removeAll();
		base.Start();
	}
	
	public override void UpdateBinding()
	{
		base.UpdateBinding();
	}
		
	// TODO: Unify with drop down list, and text binding to have one code for binding text to multiple underlying types
	
	private string GetStringValue(EZData.Context item, string path, Dictionary<EZData.Context, EZData.Property<string>> cache)
	{
		if (item == null)
			return null;		
		EZData.Property<string> property = null;
		if (cache.TryGetValue(item, out property))
			return property.GetValue();
		property = item.FindProperty<string>(path, this);
		if (property == null)
			return string.Empty;
		property.OnChange += OnOptionDataChange;
		cache.Add(item, property);
		return property.GetValue();
	}
	
	private Texture2D GetTextureValue(EZData.Context item, string path, Dictionary<EZData.Context, EZData.Property<Texture2D>> cache)
	{
		if (item == null)
			return null;		
		EZData.Property<Texture2D> property = null;
		if (cache.TryGetValue(item, out property))
			return property.GetValue();
		property = item.FindProperty<Texture2D>(path, this);
		if (property == null)
			return null;
		property.OnChange += OnOptionDataChange;
		cache.Add(item, property);
		return property.GetValue();
	}
	
	private bool GetBoolValue(EZData.Context item, string path, Dictionary<EZData.Context, EZData.Property<bool>> cache)
	{
		if (item == null)
			return false;		
		EZData.Property<bool> property = null;
		if (cache.TryGetValue(item, out property))
			return property.GetValue();
		property = item.FindProperty<bool>(path, this);
		if (property == null)
			return false;
		property.OnChange += OnOptionDataChange;
		cache.Add(item, property);
		return property.GetValue();
	}
	
	private void CleanupChangeCallback(EZData.Context key)
	{
		if (_labelTextsCache.ContainsKey(key))
			_labelTextsCache[key].OnChange -= OnOptionDataChange;
		if (_labelTooltipsCache.ContainsKey(key))
			_labelTooltipsCache[key].OnChange -= OnOptionDataChange;
		if (_labelImagesCache.ContainsKey(key))
			_labelImagesCache[key].OnChange -= OnOptionDataChange;
		if (_valuesCache.ContainsKey(key))
			_valuesCache[key].OnChange -= OnOptionDataChange;
	}
	
	protected override GameObject OnItemInsert(int position, EZData.Context item, string smartObject)
	{
		var option = new GUIContent(
			GetStringValue(item, LabelTextPath, _labelTextsCache),
			(LabelImagePath.Length > 0) ? GetTextureValue(item, LabelImagePath, _labelImagesCache) : null,
			(LabelTooltipPath.Length > 0) ? GetStringValue(item, LabelTooltipPath, _labelTooltipsCache) : string.Empty);
		_checkboxGroup.insertOption(position, option);
		_checkboxGroup.values[position] = GetBoolValue(item, ValuePath, _valuesCache);
		return null;
	}
	
	protected override void OnItemRemove(int position)
	{
		var key = _collection.GetBaseItem(position);
		CleanupChangeCallback(key);
		_labelTextsCache.Remove(key);
		if (LabelTooltipPath.Length > 0)
			_labelTooltipsCache.Remove(key);
		if (LabelImagePath.Length > 0)
			_labelImagesCache.Remove(key);
		_valuesCache.Remove(key);
		_checkboxGroup.removeOption(position);
	}
	
	protected override void OnItemsClear()
	{
		for (var i = 0; i < _collection.ItemsCount; ++i)
		{
			var key = _collection.GetBaseItem(i);
			CleanupChangeCallback(key);
		}
		
		_labelTextsCache.Clear();
		_labelTooltipsCache.Clear();
		_labelImagesCache.Clear();
		_valuesCache.Clear();
		_checkboxGroup.removeAll();
	}
	
	private void OnOptionDataChange()
	{
		if (_ignoreChanges)
			return;
		
		_ignoreChanges = true;
		for (var i = 0; i < _checkboxGroup.values.Count; ++i)
		{
			var item = _collection.GetBaseItem(i);
			_checkboxGroup.optionList[i].text = GetStringValue(item, LabelTextPath, _labelTextsCache);
			_checkboxGroup.optionList[i].image = (LabelImagePath.Length > 0) ? GetTextureValue(item, LabelImagePath, _labelImagesCache) : null;
			_checkboxGroup.optionList[i].tooltip = (LabelTooltipPath.Length > 0) ? GetStringValue(item, LabelTooltipPath, _labelTooltipsCache) : string.Empty;
			_checkboxGroup.values[i] = GetBoolValue(item, ValuePath, _valuesCache);
		}
		_ignoreChanges = false;
	}
	
	private void OnChange(iGUI.iGUIElement e)
	{
		if (_ignoreChanges)
			return;
		
		_ignoreChanges = true;
		for (var i = 0; i < _checkboxGroup.values.Count; ++i)
		{
			var key = _collection.GetBaseItem(i);
			var valueProperty = _valuesCache[key];
			if (valueProperty.GetValue() != _checkboxGroup.values[i])
			{
				valueProperty.SetValue(_checkboxGroup.values[i]);
			}
		}
		_ignoreChanges = false;
	}
	
	protected override void OnDataSelectionChange(int position)
	{
		
	}
}
