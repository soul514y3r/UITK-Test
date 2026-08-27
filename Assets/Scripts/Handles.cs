using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(HandleScript))]
[CanEditMultipleObjects]
[Serializable]
public class HandlesTest : Editor
{
    HandleScript box;
    Vector3 NewPos;
    Vector3 Handlepos;
    public Transform transform;


    void OnEnable()
    {
        box = target.GetComponent<HandleScript>();

    }

    void OnSceneGUI()
    {
Handlepos = box.Lastpos + box.transform.position;
NewPos = Handles.FreeMoveHandle(Handlepos, 0.5f, Vector3.zero, Handles.RectangleHandleCap);

if(NewPos != Handlepos)
        {
        Undo.RecordObject(box, "Move Handle");
          Handlepos = NewPos;
          box.Lastpos = Handlepos - box.transform.position;  
        EditorUtility.SetDirty(box);
        }

    }

}
