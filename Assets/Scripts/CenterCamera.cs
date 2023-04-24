using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    public PlayingField field;

    private void Start()
    {
        Center();
    }

    public void Center()
    {
        var center = new Vector3(field.playingWidth / 2 + field.xOffset + 0.5f, 0, field.playingHeight / 2 + field.yOffset + 0.5f);
        center += field.transform.position;

        var forward = transform.forward;

        transform.position = center - 10 * forward;
        transform.rotation = Quaternion.Euler(50, -45, 0);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CenterCamera)), CanEditMultipleObjects]
public class CenterCameraInspector : Editor
{
    public override void OnInspectorGUI()
    {
        CenterCamera camera = (CenterCamera)target;

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button("Center camera to playing field"))
        {
            Debug.Log("Centering camera to playing field");
            camera.Center();
        }
    }
}
#endif
