using UnityEngine;
using System;
using iGUI;
 
public class iDataEnemyItem : EZData.Context
{
	#region Property Icon
	private readonly EZData.Property<Texture2D> _privateIconProperty = new EZData.Property<Texture2D>();
	public EZData.Property<Texture2D> IconProperty { get { return _privateIconProperty; } }
	public Texture2D Icon
	{
	get    { return IconProperty.GetValue();    }
	set    { IconProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Name
	private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>();
	public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
	public string Name
	{
	get    { return NameProperty.GetValue();    }
	set    { NameProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Sprite
	private readonly EZData.Property<string> _privateSpriteProperty = new EZData.Property<string>();
	public EZData.Property<string> SpriteProperty { get { return _privateSpriteProperty; } }
	public string Sprite
	{
	get    { return SpriteProperty.GetValue();    }
	set    { SpriteProperty.SetValue(value); }
	}
	#endregion

	#region Property Value
	private readonly EZData.Property<int> _privateValueProperty = new EZData.Property<int>();
	public EZData.Property<int> ValueProperty { get { return _privateValueProperty; } }
	public int Value
	{
	get    { return ValueProperty.GetValue();    }
	set    { ValueProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Status
	private readonly EZData.Property<bool> _privateStatusProperty = new EZData.Property<bool>();
	public EZData.Property<bool> StatusProperty { get { return _privateStatusProperty; } }
	public bool Status
	{
	get    { return StatusProperty.GetValue();    }
	set    { StatusProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Position
	private readonly EZData.Property<float> _privatePositionProperty = new EZData.Property<float>();
	public EZData.Property<float> PositionProperty { get { return _privatePositionProperty; } }
	public float Position
	{
	get    { return PositionProperty.GetValue();    }
	set    { PositionProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Position2
	private readonly EZData.Property<float> _privatePosition2Property = new EZData.Property<float>();
	public EZData.Property<float> Position2Property { get { return _privatePosition2Property; } }
	public float Position2
	{
	get    { return Position2Property.GetValue();    }
	set    { Position2Property.SetValue(value); }
	}
	#endregion
}

public class iDataFractalFrameworkUi : EZData.Context
{

	public void PickImageA()
	{
		//SampleImage = ImageA;
	}
	
	public void PickImageB()
	{
		//SampleImage = ImageB;
	}
	
	public void PickImageC()
	{
		//SampleImage = ImageC;
	}
	
	public void PickNullImage()
	{
		//SampleImage = null;
	}
	
	
	#region Property SkullTime
	private readonly EZData.Property<float> _privateSkullTimeProperty = new EZData.Property<float>();
	public EZData.Property<float> SkullTimeProperty { get { return _privateSkullTimeProperty; } }
	public float SkullTime
	{
	get    { return SkullTimeProperty.GetValue();    }
	set    { SkullTimeProperty.SetValue(value); }
	}
	#endregion
	
    #region Property NextEnemy
	private readonly EZData.Property<string> _privateNextEnemyProperty = new EZData.Property<string>();
	public EZData.Property<string> NextEnemyProperty { get { return _privateNextEnemyProperty; } }
	public string NextEnemy
	{
	get    { return NextEnemyProperty.GetValue();    }
	set    { NextEnemyProperty.SetValue(value); }
	}
	#endregion
	
	#region Collection CEnemies
	private readonly EZData.Collection<iDataEnemyItem> _privateCEnemies = new EZData.Collection<iDataEnemyItem>(true);
	public EZData.Collection<iDataEnemyItem> CEnemies { get { return _privateCEnemies; } }
	#endregion
	
	public void RemoveRandomItem()
	{
		var r = new System.Random();
		var i = r.Next(CEnemies.Count);
		Debug.Log("Removing item at " + i);
		CEnemies.Remove(i);
	}
	public void RemoveItem(int i)
	{
		Debug.Log("Removing item at " + i);
		CEnemies.Remove(i);
	}
	
	public void InsertRandomItem()
	{
		var r = new System.Random();
		var i = r.Next(CEnemies.Count + 1);
		switch(r.Next(3))
		{
		case 0:
			Debug.Log("Inserting item A at " + i);
			CEnemies.Insert(i, new iDataEnemyItem { Icon = (Texture2D)Resources.Load("ImageA"), Name = "Item A", Value = 13, Status = true });
			break;
		case 1:
			Debug.Log("Inserting item B at " + i);
			CEnemies.Insert(i, new iDataEnemyItem { Icon = (Texture2D)Resources.Load("ImageB"), Name = "Item B", Value = 13, Status = true });
			break;
		case 2:
			Debug.Log("Inserting item C at " + i);
			CEnemies.Insert(i, new iDataEnemyItem { Icon = (Texture2D)Resources.Load("ImageC"), Name = "Item C", Value = 13, Status = true });
			break;
		}
	}
	
	public void SelectRandomItem()
	{
		var r = new System.Random();
		CEnemies.SelectItem(r.Next(CEnemies.Count));
	}
	
	public void AddItemA()
	{   NextEnemy = "BlukRatchetHop";
		CEnemies.Add(new iDataEnemyItem { Sprite = NextEnemy, Name = "Item A", Value = 13, Status = true });
	}
	
	public void AddItemB()
	{	
		NextEnemy = "BlukRatchetHop";
		CEnemies.Add(new iDataEnemyItem {  Sprite = NextEnemy, Name = "Item B", Value = 13, Status = true });
	}
	
	public void AddItemC()
	{   NextEnemy = "BlukRatchetHop";
		CEnemies.Add(new iDataEnemyItem {  Sprite = NextEnemy, Name = "Item C", Value = 13, Status = true });
	}
}
[System.Serializable]
public class iDataFractalFrameworkViewModel : MonoBehaviour
{
	public iGuiRootContext View;
	public iDataFractalFrameworkUi Context;
	
	void Awake()
	{
		Context = new iDataFractalFrameworkUi();
		Context.SkullTime = 2000.0f;
	
		Context.NextEnemy = "BlukRatchetHop";
		//Context.CEnemies.Add(new iDataEnemyItem { Sprite = Context.NextEnemy,  Name = "Item A", Value = 13, Status = true });	
		//Context.CEnemies.Add(new iDataEnemyItem { Sprite = Context.NextEnemy, Name = "Item B", Value = 17, Status = false });
		Context.NextEnemy = "BlukRatchetHop";
		//Context.CEnemies.Add(new iDataEnemyItem { Sprite = Context.NextEnemy, Name = "Item C", Value = 19, Status = true , Position = 3f});
			
		View.SetContext(Context);
	}
	
	void Update()
	{
		//Context.FlickeringBool = (System.DateTime.Now.Millisecond < 500);
		//Context.Now = DateTime.Now;
		//Context.UtcNow = DateTime.UtcNow;
	}
}
