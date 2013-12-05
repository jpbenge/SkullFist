using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class iGuiBinding : MonoBehaviour, EZData.IBinding
{
	public string Path;
	
	public IList<string> ReferencedPaths { get { return new List<string> { Path }; } }
	
	protected iGuiDataContext _context;

	public virtual void Awake()
	{
		_context = iGuiUtils.FindRootContext(gameObject);
	}
	
	public void OnContextChange()
	{
		UpdateBinding();
	}
	
	public virtual void UpdateBinding()
	{
	}
	
	public void SetContext(iGuiDataContext c)
	{
		_context = c;
	}
	
	public iGuiDataContext GetContext()
	{
		return _context;
	}
	
	public virtual void Start()
	{
		UpdateBinding();
	}
}
