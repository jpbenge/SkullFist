using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/DropDown List Binding")]
public class iGuiDropDownListSourceBinding : iGuiItemsSourceBinding
{
	public string LabelTextPath;
	public string LabelImagePath;
	public string LabelTooltipPath;
	
	private iGUI.iGUIDropDownList _dropDownList;
	
	private bool _ignoreChanges;
	
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _labelTextsCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _labelTooltipsCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	private readonly Dictionary<EZData.Context, EZData.Property<Texture2D>> _labelImagesCache = new Dictionary<EZData.Context, EZData.Property<Texture2D>>();
	
	public override void Awake()
	{
		base.Awake();
		
		_dropDownList = GetComponent<iGUI.iGUIDropDownList>();
		_dropDownList.removeAll();
		_dropDownList.valueChangeCallback += OnChange;
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	public override void UpdateBinding()
	{
		base.UpdateBinding();
		_dropDownList.selectedIndex = _collection.SelectedIndex;
	}
	
	// TODO: Unify with checkbox group, and text binding to have one code for binding text to multiple underlying types
	
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
		
	private void CleanupChangeCallback(EZData.Context key)
	{
		if (_labelTextsCache.ContainsKey(key))
			_labelTextsCache[key].OnChange -= OnOptionDataChange;
		if (_labelTooltipsCache.ContainsKey(key))
			_labelTooltipsCache[key].OnChange -= OnOptionDataChange;
		if (_labelImagesCache.ContainsKey(key))
			_labelImagesCache[key].OnChange -= OnOptionDataChange;
	}
	
	protected override GameObject OnItemInsert(int position, EZData.Context item, string smartObject)
	{
		var option = new GUIContent(
			GetStringValue(item, LabelTextPath, _labelTextsCache),
			(LabelImagePath.Length > 0) ? GetTextureValue(item, LabelImagePath, _labelImagesCache) : null,
			(LabelTooltipPath.Length > 0) ? GetStringValue(item, LabelTooltipPath, _labelTooltipsCache) : string.Empty);
		_dropDownList.insertOption(position, option);
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
		_dropDownList.removeOption(position);
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
		_dropDownList.removeAll();
	}
	
	private void OnOptionDataChange()
	{
		if (_ignoreChanges)
			return;
		
		_ignoreChanges = true;
		for (var i = 0; i < _dropDownList.options.Count; ++i)
		{
			var item = _collection.GetBaseItem(i);
			_dropDownList.options[i].text = GetStringValue(item, LabelTextPath, _labelTextsCache);
			_dropDownList.options[i].image = (LabelImagePath.Length > 0) ? GetTextureValue(item, LabelImagePath, _labelImagesCache) : null;
			_dropDownList.options[i].tooltip = (LabelTooltipPath.Length > 0) ? GetStringValue(item, LabelTooltipPath, _labelTooltipsCache) : string.Empty;			
		}
		_ignoreChanges = false;
	}
	
	private void OnChange(iGUI.iGUIElement e)
	{
		if (_ignoreChanges)
			return;
		
		_ignoreChanges = true;
		_collection.SelectItem(_dropDownList.selectedIndex);
		_ignoreChanges = false;
	}
	
	protected override void OnDataSelectionChange(int position)
	{
		_dropDownList.selectedIndex = position;
	}
}
