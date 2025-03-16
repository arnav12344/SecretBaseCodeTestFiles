using UnityEngine;

public class DiggingIndicator : MonoBehaviour
{
    public static DiggingIndicator instance;
    [SerializeField] private LineRenderer digCircle; // Assign a LineRenderer in Inspector
    [SerializeField] private int segments = 50; // More segments = smoother circle

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void DrawCircle(Vector3 center, float radius)
    {
        if (digCircle == null) return;

        digCircle.positionCount = segments + 1; // Ensure smooth closure

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 point = new Vector3(x, 0, z);
            digCircle.SetPosition(i, center + point + Vector3.up * 0.02f); // Slightly above ground
        }
    }
}
