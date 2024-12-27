using UnityEditor;
using UnityEngine;

public class SplineEventEditorUtility
{
    public static Transform GetSceneViewCameraTransform()
    {
        return SceneView.lastActiveSceneView.camera.transform;
    }
}