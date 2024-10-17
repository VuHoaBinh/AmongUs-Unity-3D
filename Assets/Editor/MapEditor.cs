using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor 
{
    [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override void OnInspectorGUI ()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
		base.OnInspectorGUI ();

		MapGenerator map = target as MapGenerator;

		map.GenerateMap ();
	}
	
}
