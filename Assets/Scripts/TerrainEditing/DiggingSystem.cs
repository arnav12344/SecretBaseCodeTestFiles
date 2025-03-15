using Eldemarkki.VoxelTerrain.World;
using Eldemarkki.VoxelTerrain.Utilities;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DiggingSystem : MonoBehaviour
{
    [Header("Shovel Settings")]
    [SerializeField] private VoxelWorld voxelWorld;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float digDepth = 3f;      // How much depth is removed per dig iteration
    [SerializeField] private float digRadius = 2f;     // Starting radius of the dig area
    [SerializeField] private KeyCode digKey = KeyCode.Mouse0;
    [SerializeField] private float maxReachDistance = 5f;

    [Header("Dig Timing")]
    [SerializeField] private float digInterval = 0.5f; // Time interval between each dig iteration

    [Header("Radius Adjustment")]
    [SerializeField] private float scrollSensitivity = 0.5f; // How fast the dig radius changes
    [SerializeField] private float minDigRadius = 0.5f;        // Minimum allowed dig radius
    [SerializeField] private float maxDigRadius = 10f;         // Maximum allowed dig radius

    private Coroutine digCoroutine;

    private void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (!Mathf.Approximately(scroll, 0f))
        {
            digRadius = Mathf.Clamp(digRadius + scroll * scrollSensitivity, minDigRadius, maxDigRadius);
            Debug.Log("Current Dig Radius: " + digRadius);
        }

        if (Input.GetKeyDown(digKey))
        {
            if (digCoroutine == null)
            {
                digCoroutine = StartCoroutine(DigRepeatedly());
            }
        }

        if (Input.GetKeyUp(digKey))
        {
            if (digCoroutine != null)
            {
                StopCoroutine(digCoroutine);
                digCoroutine = null;
            }
        }
    }

    private IEnumerator DigRepeatedly()
    {
        while (true)
        {
            DigHole();
            yield return new WaitForSeconds(digInterval);
        }
    }

    /// <summary>
    /// Performs a raycast and digs at the hit point by removing terrain.
    /// </summary>
    private void DigHole()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, maxReachDistance))
        {
            return;
        }
        Vector3 hitPoint = hit.point;
        PlayerTerrainDigger.Instance.EditTerrain(hitPoint, false, digDepth, digRadius);
    }


}
