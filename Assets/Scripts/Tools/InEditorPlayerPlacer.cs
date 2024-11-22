#if UNITY_EDITOR
using UnityEngine;

using UnityEditor;

[ExecuteInEditMode]
public class SelectPlayerInEditor : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        // Ensure the scene view processes input
        SceneView.duringSceneGui += OnSceneGUI;
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    private void OnDisable()
    {
        // Unsubscribe when the script is disabled
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        // Only process input when not in play mode
        if (!Application.isPlaying)
        {

            Event eventFromEditor = Event.current;

            // Check for spacebar key press
            if (eventFromEditor != null && eventFromEditor.type == EventType.KeyDown && eventFromEditor.keyCode == KeyCode.Space)
            {
                Selection.activeGameObject = player;
                // Consume the event to prevent propagation
                eventFromEditor.Use();
            }
        }
    }
}
#endif
