using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class iGuiItemDataContext : iGuiDataContext
{
	public iGuiItemsSourceBinding ItemsSource { get; private set; }
	
	public event System.Action OnSelectedChange;
	
	private bool _selected;
	public bool Selected
	{
		get { return _selected; }
		private set
		{
			bool needUpdate = (value != _selected) && (OnSelectedChange != null);
			_selected = value;
			if (needUpdate)
				OnSelectedChange();
		}
	}
	public int Index { get; private set; }
	
	void Update()
	{
		if (ItemsSource == null)
			ItemsSource = iGuiUtils.GetComponentInParents<iGuiItemsSourceBinding>(gameObject);
	}
	
	public void SetSelected(bool selected)
	{
		Selected = selected;
	}
	
	public void SetIndex(int index)
	{
		Index = index;
	}
	
	public void SetContext(EZData.Context c, iGuiRootContext rootContext)
	{
		_context =  c;
		_root = rootContext;
		
		var bindings = gameObject.GetComponentsInChildren<iGuiBinding>();
		foreach (var binding in bindings)
		{
			binding.SetContext(this);
		}
		
		var multiBindings = gameObject.GetComponentsInChildren<iGuiMultiBinding>();
		foreach (var binding in multiBindings)
		{
			binding.SetContext(this);
		}
	}
}
