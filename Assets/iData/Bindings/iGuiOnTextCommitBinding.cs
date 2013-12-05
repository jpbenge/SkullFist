using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("iGUI/iData/OnTextCommit Binding")]
public class iGuiOnTextCommitBinding : iGuiCommandBinding
{
	public override void Awake()
	{
		base.Awake();
		
		foreach(var e in GetComponents<iGUI.iGUITextfield>())
			e.enterKeyCallback += OnTextCommit;
		
		foreach(var e in GetComponents<iGUI.iGUIPasswordField>())
			e.enterKeyCallback += OnTextCommit;

		foreach(var e in GetComponents<iGUI.iGUINumberField>())
			e.enterKeyCallback += OnTextCommit;
	}
		
	void OnTextCommit(iGUI.iGUIElement e)
	{
		if (_command != null)
		{
			_command.DynamicInvoke();
		}
	}
}
