using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/List ItemsSource Binding")]
public class iGuiListItemsSourceBinding : iGuiItemsSourceBinding
{
	private iGUI.iGUIListBox _listBox;
	
	public override void Awake()
	{
		_listBox = GetComponent<iGUI.iGUIListBox>();
		base.Awake();	
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	protected override GameObject OnItemInsert(int position, EZData.Context item, string smartObject)
	{
		if (_listBox == null)
			return null;
		
		for (var i = 0; i < _listBox.items.Length; ++i)
		{
			if (_listBox.items[i].order >= position)
			{
				_listBox.items[i].setOrder(_listBox.items[i].order + 1);
				_listBox.items[i].name = (_listBox.items[i].order + 1).ToString() + "-Item";
			}
		}
		var r = _listBox.addSmartObject(smartObject);
		if (r == null)
			return null;
		r.setOrder(position);
		r.name = (_listBox.items[position].order + 1).ToString() + "-Item";
		
		return r.gameObject;
	}
	
	protected override void OnItemRemove(int position)
	{
		if (_listBox == null)
			return;
		
		for (var i = 0; i < _listBox.items.Length; ++i)
		{
			if (_listBox.items[i].order == position)
				_listBox.removeElement(_listBox.items[i]);
		}
		for (var i = 0; i < _listBox.items.Length; ++i)
		{
			if (_listBox.items[i].order > position)
			{
				_listBox.items[i].setOrder(_listBox.items[i].order - 1);
				_listBox.items[i].name = (_listBox.items[i].order + 1).ToString() + "-Item";
			}
		}
	}
	
	protected override void OnItemsClear()
	{
		if (_listBox == null)
			return;
				
		_listBox.removeAll();
	}
	
	protected override void OnDataSelectionChange(int position)
	{
	}
}
