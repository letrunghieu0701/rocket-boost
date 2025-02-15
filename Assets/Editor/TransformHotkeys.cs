using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class TransformHotkeys
{
    [MenuItem("Custom Tools/Reset Position _F1")]
    public static void ResetPostion()
    {
        ApplyKey(KeyCode.F1);
    }

    [MenuItem("Custom Tools/Reset Rotation _F2")]
    public static void ResetRotation()
    {
        ApplyKey(KeyCode.F2);
    }

    [MenuItem("Custom Tools/Reset Scale _F3")]
    public static void ResetScale()
    {
        ApplyKey(KeyCode.F3);
    }

    private static void ApplyKey(KeyCode keyPressed)
    {
        if (Selection.activeGameObject == null) return;

        Transform selectedTransform = Selection.activeGameObject.transform;
        switch (keyPressed)
        {
            case KeyCode.F1:
                RegisterUndo(selectedTransform, "Reset Transform.localPosition");
                selectedTransform.localPosition = Vector3.zero;
                break;
            case KeyCode.F2:
                RegisterUndo(selectedTransform, "Reset Transform.localRotation");
                selectedTransform.localRotation = Quaternion.identity;
                break;
            case KeyCode.F3:
                RegisterUndo(selectedTransform, "Reset Transform.localScale");
                selectedTransform.localScale = Vector3.one;
                break;
            default:
                return;
        }
    }

    private static void RegisterUndo(Transform t, string action)
    {
        Undo.RecordObject(t, action);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
