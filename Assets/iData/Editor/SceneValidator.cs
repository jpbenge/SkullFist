using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneValidator
{
	private static string GetGameObjectPath(GameObject gameObject)
	{
		if (gameObject == null)
			return null;
		
		string fullName = "";
		while(gameObject != null)
		{
			fullName = gameObject.name + ((fullName.Length > 0) ? "." : "") + fullName;
			gameObject = (gameObject.transform.parent == null) ? null : gameObject.transform.parent.gameObject;
		}
		return fullName;
	}
	
	private static System.Type GetCollectionValueType(System.Type type)
	{
		var genericArguments = type.GetGenericArguments();
		if (genericArguments.Length == 0)
			return null;
		return genericArguments[0];
	}
	
	private static System.Type GetBindingValueType(string path, System.Type type, GameObject gameObject, bool command, ref string tail)
	{
		tail = string.Empty;
		
		if (string.IsNullOrEmpty(path))
		{
			Debug.Log("Binding is empty for object " + GetGameObjectPath(gameObject));
			return null;
		}
		
		var parts = path.Split('.');
		var pathMessage = "";
		for (var i = 0; i < parts.Length; ++i)
		{
			var part = parts[i];
			pathMessage += part;
						
			if (i < parts.Length - 1)
			{
				int index;
				if (int.TryParse(part, out index))
				{
					for (var j = i + 1; j < parts.Length; ++j)
						tail += parts[j] + ".";
					if (tail.Length > 0)
						tail = tail.Substring(0, tail.Length - 1);
					return type;
				}
				else
				{
					var nodeProperty = type.GetProperty(part);
					if (nodeProperty == null)
					{
						Debug.LogError("Failed to resolve node in binding " + path + " in object " + GetGameObjectPath(gameObject) +
						               "\nerror at " + pathMessage);
						return null;
					}
					pathMessage += ".";
					type = nodeProperty.PropertyType;
				}
			}
			else
			{
				if (command)
				{
					var leafCommand = type.GetMethod(part);
					if (leafCommand == null)
					{
						Debug.LogError("Failed to resolve leaf command in binding " + path + " in object " + GetGameObjectPath(gameObject) +
						               "\nerror at " + pathMessage);
						return null;
					}
				}
				else
				{
					var leafProperty = type.GetProperty(part);
					if (leafProperty == null)
					{
						Debug.LogError("Failed to resolve leaf property in binding " + path + " in object " + GetGameObjectPath(gameObject) +
						               "\nerror at " + pathMessage);
						return null;
					}
					type = leafProperty.PropertyType;
				}
			}
		}
		return type;
	}
	
	private static System.Type GetBindingValueType(string path, System.Type type, GameObject gameObject, bool command)
	{
		var tail = path;
		while(tail.Length > 0)
		{
			type = GetBindingValueType(tail, type, gameObject, command, ref tail);
			if (tail.Length == 0)
				break;
			type = GetCollectionValueType(type);
		}
		
		return type;
	}


	private static void ValidateRootObjectBindings(GameObject gameObject, System.Type type)
	{
		if (gameObject == null || type == null)
			return;
	
		foreach(var c in gameObject.GetComponents<MonoBehaviour>())
		{
			if (!(c is EZData.IBinding))
				continue;
			
			if (!c.enabled)
			{
				Debug.LogWarning("Binding in object " + GetGameObjectPath(gameObject) + " is disabled");
			}
			
			var paths = ((EZData.IBinding)c).ReferencedPaths;
			var types = new List<System.Type>();
			
			foreach(var p in paths)
			{
				types.Add(GetBindingValueType(p, type, gameObject, c is iGuiCommandBinding));
			}
			
			foreach(var l in c.gameObject.GetComponents<iGuiListItemTemplate>())
			{
				var allPaths = AssetDatabase.GetAllAssetPaths();
				var pathMarker = "iGUI/Resources/SmartObjects/" + l.SmartObject + "Prefab.prefab";
				var path = "Assets/" + pathMarker;
				foreach(var p in allPaths)
				{
					if (p.EndsWith(pathMarker))
					{
						path = p;
						break;
					}
				}
				
				var prefab = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;
				if (prefab == null)
				{
					Debug.LogError("List in object " + GetGameObjectPath(gameObject) + " doesn't have a valid item template. It has to be a smart object buit for dynamic instantiation.");
				}
				if (types.Count > 0 && c is iGuiItemsSourceBinding)
				{
					ValidateRootObjectBindings(prefab, GetCollectionValueType(types[0]));
				}	
			}
			foreach(var p in c.gameObject.GetComponents<iGuiDropDownListSourceBinding>())
			{
				GetBindingValueType(p.LabelTextPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelImagePath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelTooltipPath, GetCollectionValueType(types[0]), gameObject, false);
			}	
			foreach(var p in c.gameObject.GetComponents<iGuiCheckboxOptionsSourceBinding>())
			{
				GetBindingValueType(p.LabelTextPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelImagePath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelTooltipPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.ValuePath, GetCollectionValueType(types[0]), gameObject, false);
			}
			foreach(var p in c.gameObject.GetComponents<iGuiSlidePanelSourceBinding>())
			{
				GetBindingValueType(p.LabelTextPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelImagePath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelTooltipPath, GetCollectionValueType(types[0]), gameObject, false);
			}
			foreach(var p in c.gameObject.GetComponents<iGuiTabPanelSourceBinding>())
			{
				GetBindingValueType(p.LabelTextPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelImagePath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.LabelTooltipPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.TitleTextPath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.TitleImagePath, GetCollectionValueType(types[0]), gameObject, false);
				GetBindingValueType(p.TitleTooltipPath, GetCollectionValueType(types[0]), gameObject, false);
			}
		}
		for (var i = 0; i < gameObject.transform.childCount; ++i)
		{
			ValidateRootObjectBindings(gameObject.transform.GetChild(i).gameObject, type);
		}
	}
	
	[MenuItem("Tools/iData/Clear Console &c")]
	public static void ClearConsole()
	{
		Assembly assembly = Assembly.GetAssembly(typeof(PrefabType));
        Type type = assembly.GetType("UnityEditorInternal.LogEntries");
        MethodInfo method = type.GetMethod ("Clear");
        method.Invoke (new object (), null);
	}
	
	[MenuItem("Tools/iData/Validate Bindings &v")]
	public static void ValidateBindings()
	{
		foreach (var s in UnityEditor.Selection.gameObjects)
		{
			if (PrefabUtility.GetPrefabType(s) != PrefabType.None)
				continue;
			
			var type = typeof(object); // Use your root data context class type here to speed up validation
			
			if (type == typeof(object))
			{
				var types = typeof(iGuiRootContext).Assembly.GetTypes();
				foreach(var t in types)
				{
					var hasRootContextProperty = false;
					var hasContext = false;
					Type contextType = null;
					foreach(var f in t.GetFields())
					{
						if (f.FieldType == typeof(iGuiRootContext))
							hasRootContextProperty = true;
						if (typeof(EZData.Context).IsAssignableFrom(f.FieldType))
						{
							hasContext = true;
							contextType = f.FieldType;
						}
					}
					foreach(var p in t.GetProperties())
					{
						if (p.PropertyType == typeof(iGuiRootContext))
							hasRootContextProperty = true;
						if (typeof(EZData.Context).IsAssignableFrom(p.PropertyType))
						{
							hasContext = true;
							contextType = p.PropertyType;
						}
					}
					if (hasRootContextProperty && hasContext)
					{
						Debug.Log(string.Format("Root data context class detected: {0}", contextType));
						type = contextType;
					}
				}
			}
			
			ValidateRootObjectBindings(s, type);
		}
	}
}
