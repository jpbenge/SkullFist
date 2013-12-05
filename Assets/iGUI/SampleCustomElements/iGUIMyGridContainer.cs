using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using iGUI;
using System;

/// <summary>
/// Only works with iGUI 1.4.1 and up
/// </summary>
public class iGUIMyGridContainer : iGUICustomContainer {
	
	public GridType gridType = GridType.ColumnBased; 
	public int count = 2;
	
	protected override void beforeInitItems (){
		//Can be used to do something before initialization of items
	}
	
	protected override void afterInitItems (){
		//Can be used to do something after initialization of items		
	}
	
	protected override void beforeDrawItems (){
		//Can be used to do something before items drawn (For example a background like panel/window)
	}
	
	protected override void afterDrawItems (){
		//Can be used to do something after items drawn
	}
	
	protected override void initItems (){	
		//Although layouts won't be used, 
		//we force layout as non-free to allow container to be treated as non-free by the editor.
		layout = iGUILayout.Horizontal;
		
		Rect subRect=rect;
		subRect.x+=padding.left;
		subRect.y+=padding.top;
		subRect.width-=padding.left+padding.right;
		subRect.height-=padding.top+padding.bottom;
					
		if(Application.isPlaying)
			injectVariables();
		
		if(reverseOrder)
			Array.Reverse(items);
		
		int columns = 0;
		int rows = 0;
			
		int currentColumn = 0;
		int currentRow = 0;
		
		if(count<1)
			count = 1;
		
		switch(gridType){
		case GridType.ColumnBased:
			columns = count;
			rows = itemCount / columns;
			
			if(itemCount%columns > 0)
				rows++;
						
			if(rows>0){
				subRect.height=(subRect.height-(itemsMargin.top+itemsMargin.bottom)*rows)/rows;
			}
						
			if(columns>0){
				subRect.width=(subRect.width-(itemsMargin.left+itemsMargin.right)*columns)/columns;
			}
			
			for(int i=0;i<itemCount;i++){
				
				currentColumn = i%columns;
				currentRow = i/columns;
				
				if(items[i]!=null){
					subRect.x = rect.x + (currentColumn+1)*itemsMargin.left + currentColumn*(subRect.width+itemsMargin.right);
					subRect.y = rect.y + (currentRow+1)*itemsMargin.top + currentRow*(subRect.height+itemsMargin.bottom);
					items[i].baseRect=rect;
					items[i].isRectStatic=isRectStatic;
					items[i].init(this,subRect);
				}
			}
			
			break;
			
		case GridType.RowBased:
			rows = count;
			columns = itemCount / rows;
			
			if(itemCount%rows > 0)
				columns++;
						
			if(rows>0){
				subRect.height=(subRect.height-(itemsMargin.top+itemsMargin.bottom)*rows)/rows;
			}
						
			if(columns>0){
				subRect.width=(subRect.width-(itemsMargin.left+itemsMargin.right)*columns)/columns;
			}
			
			for(int i=0;i<itemCount;i++){
				
				currentRow = i%rows;
				currentColumn = i/rows;
				
				if(items[i]!=null){
					subRect.x = rect.x + (currentColumn+1)*itemsMargin.left + currentColumn*(subRect.width+itemsMargin.right);
					subRect.y = rect.y + (currentRow+1)*itemsMargin.top + currentRow*(subRect.height+itemsMargin.bottom);
					items[i].baseRect=rect;
					items[i].isRectStatic=isRectStatic;
					items[i].init(this,subRect);
				}
			}
			break;
		}
	}
	
	public override int positionToChildOrder(Vector2 position){
		return itemCount;
	}
	
	protected override bool needsRefresh(){
		return ignoreDisabledElements;
	}
}

public enum GridType{
	RowBased,
	ColumnBased
}
