using UnityEditor;
using UnityEngine;

namespace _Workspace.Scripts.Editor
{
    public class ParentSetterEditor : EditorWindow
    {
        [MenuItem("Tools/Set Parent with Center Pivot")]
        static void SetParentWithCenteredPivot()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("No objects selected!");
                return;
            }

            // Calculate center of selected objects
            Vector3 center = Vector3.zero;
            foreach (GameObject obj in selectedObjects)
            {
                center += obj.transform.position;
            }
            center /= selectedObjects.Length;

            // Create a new empty parent object
            GameObject parent = new GameObject("ParentObject");
            parent.transform.position = center;

            // Attach selected objects to new parent
            foreach (GameObject obj in selectedObjects)
            {
                obj.transform.SetParent(parent.transform);
            }

            Debug.Log("Parent created at center of selected objects!");
        }
    }
}