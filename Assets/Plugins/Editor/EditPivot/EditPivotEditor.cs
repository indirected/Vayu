using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor( typeof(EditPivotBehaviour) )]
public class EditPivotEditor : Editor  
{
	SerializedObject targetObject;
	SerializedProperty pivotposition;
	SerializedProperty lasttool;
	SerializedProperty reference;	
	SerializedProperty originalPosition;
	SerializedProperty vertexsnap;
	
	Tool backup = Tool.Move;
	Vector3 snappedPos;
	Vector3[] vertices;
	
	void OnEnable()
	{
		targetObject = new SerializedObject(target);
		pivotposition = targetObject.FindProperty( "pivotposition" );
		originalPosition = targetObject.FindProperty( "originalposition" );
		lasttool = targetObject.FindProperty( "lasttool" );
		vertexsnap = targetObject.FindProperty( "vertexsnap" );
		reference = targetObject.FindProperty( "refGO" );
		
		Mesh mesh = ((EditPivotBehaviour)target).gameObject.GetComponent<MeshFilter>().sharedMesh;
		Transform transform = ((EditPivotBehaviour)target).gameObject.GetComponent<Transform>();
		snappedPos = ((EditPivotBehaviour)target).Pivotposition;
		
		vertices = new Vector3[mesh.vertexCount];
		for ( int i=0; i<mesh.vertexCount; i++)
		{
			vertices[i] =  transform.TransformPoint( mesh.vertices[i] );
		}
	}
	
	public override void OnInspectorGUI ()
	{
		targetObject.Update();		
		
		pivotposition.vector3Value = EditorGUILayout.Vector3Field( "Pivot Position", pivotposition.vector3Value );
		if ( GUILayout.Button( "Apply Pivot" ))
		{
			ApplyPivot();
			return;
		}
		
		GUILayout.Space( 20 );
		// options
		GUILayout.BeginHorizontal();
		GUI.enabled = true;
		if ( ((EditPivotBehaviour)target).GetComponent<MeshFilter>() == null )
		{
			GUI.enabled = false;
		}
		if ( GUILayout.Button( "Center to Mesh" , GUILayout.MaxWidth( Screen.width * 0.5f) ))
		{
			EditPivot.CenterToMesh( ((EditPivotBehaviour)target).gameObject, EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
			pivotposition.vector3Value = ((EditPivotBehaviour)target).transform.position;
		}
		GUI.enabled = true;
		if ( ((EditPivotBehaviour)target).transform.GetChildCount() == 0 )
		{
			GUI.enabled = false;
		}
		if ( GUILayout.Button( "Center to Children" ))
		{
			EditPivot.CenterToChildren( ((EditPivotBehaviour)target).gameObject, EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
			pivotposition.vector3Value = ((EditPivotBehaviour)target).transform.position;
		}
		GUI.enabled = true;
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		if ( GUILayout.Button( "Reset Pivot", GUILayout.MaxWidth( Screen.width * 0.5f) ))
		{
			pivotposition.vector3Value = originalPosition.vector3Value;
			EditPivot.MovePivot( ((EditPivotBehaviour)target).gameObject, pivotposition.vector3Value, EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
		}
		
		if ( GUILayout.Button( "Cancel" ))
		{				
			pivotposition.vector3Value = originalPosition.vector3Value;
			ApplyPivot();
			return;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Space( 20 );
		
		// move pivot to object reference
		GUILayout.BeginHorizontal();
		reference.objectReferenceValue = EditorGUILayout.ObjectField( "Pick Reference Object", reference.objectReferenceValue , typeof(GameObject), true ) as GameObject;		
 		if ( reference.objectReferenceValue == null )
		{
			 GUI.enabled = false;
		}
		if ( GUILayout.Button( "x", EditorStyles.miniButton, GUILayout.MaxWidth(24f) ) )
		{
			reference.objectReferenceValue = null;
		}
		GUILayout.EndHorizontal();
		if ( GUILayout.Button( "Move Pivot to Reference" ) )
		{
			pivotposition.vector3Value = ((GameObject)reference.objectReferenceValue).transform.position;
			EditPivot.MovePivot( ((EditPivotBehaviour)target).gameObject, pivotposition.vector3Value , EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
		}
		
		targetObject.ApplyModifiedProperties();
	}
	
	void ApplyPivot()
	{
		targetObject.ApplyModifiedProperties();
		EditPivot.MovePivot( ((EditPivotBehaviour)target).gameObject, pivotposition.vector3Value, EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
		Tools.current = (Tool)lasttool.intValue;
		DestroyImmediate( (EditPivotBehaviour)target );
	}
	
	void OnSceneGUI ()
	{
		// hide normal move/scale/rotate handle
		if ( Tools.current != Tool.None )
		{
			lasttool.intValue = (int)Tools.current;
			backup = Tools.current;
			targetObject.ApplyModifiedProperties();			
		}		
		Tools.current = Tool.None;
		Tools.pivotMode = PivotMode.Pivot;

		
		Vector3 pos = ((EditPivotBehaviour)target).Pivotposition;
		
		// vertex snapping
		Event current = Event.current;
		switch(current.type)
		{			
			case EventType.MouseDrag:					
				if( !current.alt && current.button == 0 && vertexsnap.boolValue)
				{
					float minDistance = Mathf.Infinity;
					int vertexToSelect = 0;
					for( int i=0; i<vertices.Length; i++ )
					{
						float distance = Vector2.Distance( HandleUtility.WorldToGUIPoint( vertices[i] ), current.mousePosition );
						minDistance = Mathf.Min( minDistance, distance );
						if( distance == minDistance )
						{
							vertexToSelect = i;	
						}
					}					
					snappedPos = vertices[ vertexToSelect ];
					
				}
				break;
		}
		
		if( vertexsnap.boolValue )
		{			
			pos = snappedPos;			
		}	
		
		// cyan dotted is now our pivot handle	
		pos = Handles.PositionHandle(pos, Quaternion.identity );						
		((EditPivotBehaviour)target).Pivotposition = pos;
		targetObject.ApplyModifiedProperties();
		
		Handles.color = Color.cyan;
		Handles.SphereCap( 0, pos, Quaternion.identity, HandleUtility.GetHandleSize(pos)*0.2f );
		
		GUIStyle pivotstyle = new GUIStyle();
		pivotstyle.alignment = TextAnchor.LowerLeft;
		pivotstyle.normal.textColor = Color.cyan;

		Handles.Label( pos, "\nPivot",  pivotstyle);

		// show reference point in view
		if ( reference.objectReferenceValue != null )
		{
			GUIStyle refstyle = new GUIStyle( pivotstyle );
			refstyle.normal.textColor = Color.magenta;
			
			Handles.color = Color.magenta;
			Vector3 refpos = ((GameObject)reference.objectReferenceValue).transform.position;	
			
			Handles.SphereCap( 0, refpos, Quaternion.identity, HandleUtility.GetHandleSize(pos)*0.1f );
			Handles.Label( refpos, "\nReference",  refstyle);
		}		
		
		//in scene view gui (really nice)
		Handles.BeginGUI();
		GUILayout.BeginArea( new Rect(Screen.width - 160, Screen.height - 160, 150,120) );
		
		GUI.enabled = true;
		if ( ((EditPivotBehaviour)target).GetComponent<MeshFilter>() == null )
		{
			GUI.enabled = false;
		}
		
		bool lastVertexsnap = vertexsnap.boolValue;
		vertexsnap.boolValue = GUILayout.Toggle( vertexsnap.boolValue, "Vertex Snap Mode" , GUILayout.MaxWidth( Screen.width * 0.5f));
		if( lastVertexsnap != vertexsnap.boolValue )
		{			
			snappedPos = pivotposition.vector3Value;
			targetObject.ApplyModifiedProperties();
		}		
	
		if ( GUILayout.Button( "Center to Mesh" , GUILayout.MaxWidth( Screen.width * 0.5f) ))
		{			
			EditPivot.CenterToMesh( ((EditPivotBehaviour)target).gameObject, EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
			pivotposition.vector3Value = ((EditPivotBehaviour)target).transform.position;
			snappedPos = pivotposition.vector3Value;
			targetObject.ApplyModifiedProperties();
		}		
		
		if ( ((EditPivotBehaviour)target).transform.GetChildCount() > 0 )
		{		
			if ( GUILayout.Button( "Center to Children" ))
			{			
				EditPivot.CenterToChildren( ((EditPivotBehaviour)target).gameObject, EditPivotMenu.UpdateCollider, EditPivotMenu.UpdatePrefab );
				pivotposition.vector3Value = ((EditPivotBehaviour)target).transform.position;
				snappedPos = pivotposition.vector3Value;
				targetObject.ApplyModifiedProperties();
			}
		}
		
		GUI.enabled = true;
		GUILayout.Space( 20 );
		if(GUILayout.Button("Apply Pivot", EditorStyles.miniButton))
		{
			ApplyPivot();
		}
		if(GUILayout.Button("Cancel", EditorStyles.miniButton))
		{
			pivotposition.vector3Value = originalPosition.vector3Value;
			ApplyPivot();
		}
		GUILayout.EndArea();
		Handles.EndGUI();
		
		if (GUI.changed && target != null)
		{
        	EditorUtility.SetDirty (target);
		}	
	}
	
	// restore last tool
	void OnDestroy()
	{
		if( backup != Tool.None )
		{
			Tools.current = backup;
		}
		else
		{
			Tools.current = Tool.Move;
		}

	}


}