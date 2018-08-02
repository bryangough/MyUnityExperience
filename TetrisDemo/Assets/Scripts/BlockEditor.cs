/**
 * Author:    Bryan Gough
 * 
 *
 *
 **/
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Block))]
public class BlockEditor : Editor {
	private static GUIStyle ToggleButtonStyleNormal = null;
	private static GUIStyle ToggleButtonStyleToggled = null;

	Block objectReference;
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (objectReference == null)
			objectReference = (Block)target;	
		if ( ToggleButtonStyleNormal == null )
		{
			ToggleButtonStyleNormal = "Button";
			ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
			ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
		}

		for(int y=0;y<4;y++)
		{
			GUILayout.BeginHorizontal(GUILayout.Width(100));
				for(int x=0;x<4;x++)
				{
					doToggleButton(x,y, objectReference.objectGrid);
				}
			GUILayout.EndHorizontal();
		}
	}

	public void doToggleButton(int x, int y, bool[] boolarray)
	{	
		if ( GUILayout.Button( "", boolarray[y * 4 + x] ? ToggleButtonStyleToggled : ToggleButtonStyleNormal ) )
		{
			boolarray[y * 4 + x] = !boolarray[y * 4 + x];
		}
	}
}
