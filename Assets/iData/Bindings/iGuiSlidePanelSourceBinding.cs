using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/SlidePanel ItemsSource Binding")]
public class iGuiSlidePanelSourceBinding : iGuiItemsSourceBinding
{
	public string LabelTextPath;
	public string LabelImagePath;
	public string LabelTooltipPath;
	
	private iGUI.iGUISlidePanel _slidePanel;
	
	public override void Awake()
	{
		_slidePanel = GetComponent<iGUI.iGUISlidePanel>();
		base.Awake();	
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	protected override GameObject OnItemInsert(int position, EZData.Context item, string smartObject)
	{
		if (_slidePanel == null)
			return null;
				
		for (var i = 0; i < _slidePanel.items.Length; ++i)
		{
			if (_slidePanel.items[i].order >= position)
			{
				_slidePanel.items[i].setOrder(_slidePanel.items[i].order + 1);
				_slidePanel.items[i].name = (_slidePanel.items[i].order + 1).ToString() + "-Panel";
			}
		}
		var r = _slidePanel.newPanel();
		
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
		r.name = (_slidePanel.items[position].order + 1).ToString() + "-Panel";
		
		return r.gameObject;
	}
	
	protected override void OnItemRemove(int position)
	{
		if (_slidePanel == null)
			return;
		
		for (var i = 0; i < _slidePanel.items.Length; ++i)
		{
			if (_slidePanel.items[i].order == position)
				_slidePanel.removePanel(_slidePanel.items[i]);
		}
		for (var i = 0; i < _slidePanel.items.Length; ++i)
		{
			if (_slidePanel.items[i].order > position)
			{
				_slidePanel.items[i].setOrder(_slidePanel.items[i].order - 1);
				_slidePanel.items[i].name = (_slidePanel.items[i].order + 1).ToString() + "-Panel";
			}
		}
	}
	
	protected override void OnItemsClear()
	{
		if (_slidePanel == null)
			return;
		
		while(_slidePanel.items.Length > 0)
			_slidePanel.removePanel(_slidePanel.items[0]);
	}
	
	protected override void OnDataSelectionChange(int position)
	{
	}
}
