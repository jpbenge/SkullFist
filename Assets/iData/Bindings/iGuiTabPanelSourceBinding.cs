using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/TabPanel ItemsSource Binding")]
public class iGuiTabPanelSourceBinding : iGuiItemsSourceBinding
{
	public string LabelTextPath;
	public string LabelImagePath;
	public string LabelTooltipPath;
	public string TitleTextPath;
	public string TitleImagePath;
	public string TitleTooltipPath;
	
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _titleTextsCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	private readonly Dictionary<EZData.Context, EZData.Property<string>> _titleTooltipsCache = new Dictionary<EZData.Context, EZData.Property<string>>();
	private readonly Dictionary<EZData.Context, EZData.Property<Texture2D>> _titleImagesCache = new Dictionary<EZData.Context, EZData.Property<Texture2D>>();
	
	private iGUI.iGUITabPanel _tabPanel;
	
	public override void Awake()
	{
		_tabPanel = GetComponent<iGUI.iGUITabPanel>();
		base.Awake();	
	}
	
	public override void Start()
	{
		while(_tabPanel.titles.Count > 0)
			_tabPanel.titles.RemoveAt(0);
		while(_tabPanel.items.Length > 0)
			_tabPanel.removePanel(_tabPanel.items[0]);
		base.Start();
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
		if (_titleTextsCache.ContainsKey(key))
			_titleTextsCache[key].OnChange -= OnOptionDataChange;
		if (_titleTooltipsCache.ContainsKey(key))
			_titleTooltipsCache[key].OnChange -= OnOptionDataChange;
		if (_titleImagesCache.ContainsKey(key))
			_titleImagesCache[key].OnChange -= OnOptionDataChange;
	}
	
	protected override GameObject OnItemInsert(int position, EZData.Context item, string smartObject)
	{
		if (_tabPanel == null)
			return null;
				
		for (var i = 0; i < _tabPanel.items.Length; ++i)
		{
			if (_tabPanel.items[i].order >= position)
			{
				_tabPanel.items[i].setOrder(_tabPanel.items[i].order + 1);
				_tabPanel.items[i].name = (_tabPanel.items[i].order + 1).ToString() + "-Panel";
			}
		}
		
		var r = _tabPanel.newPanel(GetStringValue(item, TitleTextPath, _titleTextsCache));
		
		if (LabelTextPath.Length > 0)
			r.gameObject.AddComponent<iGuiLabelTextBinding>().Path = LabelTextPath;
		if (LabelImagePath.Length > 0)
			r.gameObject.AddComponent<iGuiLabelTextureBinding>().Path = LabelImagePath;
		if (LabelTooltipPath.Length > 0)
			r.gameObject.AddComponent<iGuiTooltipTextBinding>().Path = LabelTooltipPath;
		
		r.addSmartObject(smartObject);
		
		if (r == null)
			return null;
		r.setOrder(position);
		r.name = (_tabPanel.items[position].order + 1).ToString() + "-Panel";
		OnOptionDataChange(position, item);
		
		return r.gameObject;
	}
	
	protected override void OnItemRemove(int position)
	{
		if (_tabPanel == null)
			return;
		
		var key = _collection.GetBaseItem(position);
		CleanupChangeCallback(key);
		_titleTextsCache.Remove(key);
		if (TitleTooltipPath.Length > 0)
			_titleTooltipsCache.Remove(key);
		if (TitleImagePath.Length > 0)
			_titleImagesCache.Remove(key);
		
		for (var i = 0; i < _tabPanel.items.Length; ++i)
		{
			if (_tabPanel.items[i].order == position)
				_tabPanel.removePanel(_tabPanel.items[i]);
		}
		for (var i = 0; i < _tabPanel.items.Length; ++i)
		{
			if (_tabPanel.items[i].order > position)
			{
				_tabPanel.items[i].setOrder(_tabPanel.items[i].order - 1);
				_tabPanel.items[i].name = (_tabPanel.items[i].order + 1).ToString() + "-Panel";
			}
		}
	}
	
	protected override void OnItemsClear()
	{
		if (_tabPanel == null)
			return;
		
		for (var i = 0; i < _collection.ItemsCount; ++i)
		{
			var key = _collection.GetBaseItem(i);
			CleanupChangeCallback(key);
		}
		
		_titleTextsCache.Clear();
		_titleTooltipsCache.Clear();
		_titleImagesCache.Clear();
		
		while(_tabPanel.items.Length > 0)
			_tabPanel.removePanel(_tabPanel.items[0]);
	}
	
	private void OnOptionDataChange(int fakeItemIndex, EZData.Context fakeItem)
	{
		for (var i = 0; i < _tabPanel.titles.Count; ++i)
		{
			var item = (i == fakeItemIndex) ? fakeItem : _collection.GetBaseItem((i > fakeItemIndex) ? i - 1 : i);
			Debug.Log("Setting item " + i + " to " + GetStringValue(item, TitleTextPath, _titleTextsCache));
			
			_tabPanel.titles[i].text = GetStringValue(item, TitleTextPath, _titleTextsCache);
			_tabPanel.titles[i].image = (TitleImagePath.Length > 0) ? GetTextureValue(item, TitleImagePath, _titleImagesCache) : null;
			_tabPanel.titles[i].tooltip = (TitleTooltipPath.Length > 0) ? GetStringValue(item, TitleTooltipPath, _titleTooltipsCache) : string.Empty;
		}
	}
	
	private void OnOptionDataChange()
	{
		OnOptionDataChange(_tabPanel.titles.Count + 10, null);
	}
	
	protected override void OnDataSelectionChange(int position)
	{
	}
}
