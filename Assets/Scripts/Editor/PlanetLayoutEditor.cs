using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlanetLayout))]
public class PlanetLayoutEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlanetLayout layoutScript = (PlanetLayout)target;

        if (GUILayout.Button("Distance"))
        {
            layoutScript.UpdateLayout();
            layoutScript.Clear();
        }

    }
}
