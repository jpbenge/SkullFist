using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/List Item Template")]
public class iGuiListItemTemplate : MonoBehaviour
{
	public string SmartObject;
	
	private iGuiDataContext _rootContext = null;
	
	void Awake()
	{
		_rootContext = iGuiUtils.FindRootContext(gameObject);
	}
	
	public void ApplyContext(GameObject instance, EZData.Context itemContext, int index)
	{
		if (instance == null)
			return;
		var itemData = instance.AddComponent<iGuiItemDataContext>();
		itemData.SetContext(itemContext, _rootContext.Root);
		itemData.SetIndex(index);
	}
}
