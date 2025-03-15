using Eldemarkki.VoxelTerrain.World;
using Eldemarkki.VoxelTerrain.Utilities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerTerrainDigger : MonoBehaviour
{
    public static PlayerTerrainDigger Instance { get; private set; }

    [Header("Voxel World Reference")]
    [SerializeField] private VoxelWorld voxelWorld;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Modifies the voxel data in a spherical region around the point.
    /// </summary>
    /// <param name="point">The center of the modification.</param>
    /// <param name="addTerrain">If true, adds terrain; if false, removes terrain.</param>
    /// <param name="deformSpeed">The speed of terrain modification (depth per iteration).</param>
    /// <param name="range">The radius of the area to modify.</param>
    public void EditTerrain(Vector3 point, bool addTerrain, float deformSpeed, float range)
    {
        int buildModifier = addTerrain ? 1 : -1;

        int hitX = Mathf.RoundToInt(point.x);
        int hitY = Mathf.RoundToInt(point.y);
        int hitZ = Mathf.RoundToInt(point.z);
        int3 hitPoint = new int3(hitX, hitY, hitZ);

        int intRange = Mathf.CeilToInt(range);
        int3 rangeInt3 = new int3(intRange, intRange, intRange);

        BoundsInt queryBounds = new BoundsInt((hitPoint - rangeInt3).ToVectorInt(), new int3(intRange * 2).ToVectorInt());

        voxelWorld.VoxelDataStore.SetVoxelDataCustom(queryBounds, (voxelDataWorldPosition, voxelData) =>
        {
            float distance = math.distance(voxelDataWorldPosition, point);
            if (distance <= range)
            {
                float modificationAmount = deformSpeed / math.max(distance, 0.1f) * buildModifier;
                float oldVoxelData = voxelData / 255f;
                return (byte)math.clamp((oldVoxelData - modificationAmount) * 255, 0, 255);
            }
            return voxelData;
        });
    }
}
