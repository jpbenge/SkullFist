using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/Root Context")]
public class iGuiRootContext : iGuiDataContext
{
	public void SetContext(EZData.Context context)
	{
		_context = context;
	}
}
