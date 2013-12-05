using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class iGuiMultiBinding : MonoBehaviour, EZData.IBinding
{
	public List<string> Paths;
	
	public IList<string> ReferencedPaths { get { return Paths; } }
	
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
