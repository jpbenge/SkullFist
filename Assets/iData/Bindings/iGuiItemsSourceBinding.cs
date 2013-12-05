using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class iGuiItemsSourceBinding : iGuiBinding
{
	protected iGuiListItemTemplate _itemTemplate;
	protected EZData.Collection _collection;
	protected bool _isCollectionSelecting = false;
	
	public override void Awake()
	{
		base.Awake();
		_itemTemplate = gameObject.GetComponent<iGuiListItemTemplate>();
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	public override void UpdateBinding()
	{
		if (_collection != null)
		{
			OnItemsClear();
			_collection.OnItemInsert -= OnItemInsert;
			_collection.OnItemRemove -= OnItemRemove;
			_collection.OnItemsClear -= OnItemsClear;
			_collection.OnSelectionChange -= OnDataSelectionChange;
			_collection = null;
		}
			
		var context = GetContext();
		if (context == null)
			return;
		
		_collection = context.FindCollection(Path, this);
		if (_collection == null)
		{
			Debug.LogWarning("iGuiItemsSourceBinding: failed to find collection at " + Path);
			return;
		}
	
		_collection.OnItemInsert += OnItemInsert;
		_collection.OnItemRemove += OnItemRemove;
		_collection.OnItemsClear += OnItemsClear;
		_collection.OnSelectionChange += OnDataSelectionChange;
		
		for(var i = 0; i < _collection.ItemsCount; ++i)
		{
			OnItemInsert(i, _collection.GetBaseItem(i));
		}
	}

	private void OnDataSelectionChange()
	{
		if (_collection != null)
			OnDataSelectionChange(_collection.SelectedIndex);
	}
	
	protected void OnUiSelectionChange(int position)
	{
		_isCollectionSelecting = true;
		if (_collection != null)
			_collection.SelectItem(position);
		_isCollectionSelecting = false;
	}

	private void OnItemInsert(int position, EZData.Context item)
	{
		if (_itemTemplate != null)
		{
			var obj = OnItemInsert(position, item, _itemTemplate.SmartObject);
			_itemTemplate.ApplyContext(obj, item, position);
		}
		else
		{
			OnItemInsert(position, item, string.Empty);
		}
	}
	
	protected abstract GameObject OnItemInsert(int position, EZData.Context item, string smartObject);
		
	protected abstract void OnItemRemove(int position);
	
	protected abstract void OnItemsClear();
		
	protected abstract void OnDataSelectionChange(int position);
}
