using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class iGuiDataContext : MonoBehaviour
{
	protected EZData.Context _context;
	protected iGuiRootContext _root;
	
	public iGuiRootContext Root
	{
		get
		{
			return _root ?? (this as iGuiRootContext);
		}
	}
	
	public EZData.Property<T> FindProperty<T>(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindProperty<T>(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find property " + path + "\n" + ex);
			return null;
		}
	}
	
	public EZData.Property<int> FindEnumProperty(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindEnumProperty(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find enum property " + path + "\n" + ex);
			return null;
		}
	}
	
	public System.Delegate FindCommand(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindCommand(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find command " + path + "\n" + ex);
			return null;
		}
	}
	
	public EZData.Collection FindCollection(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindCollection(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find collection " + path + "\n" + ex);
			return null;
		}
	}
}
