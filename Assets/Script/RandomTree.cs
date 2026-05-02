using UnityEngine;

public class TreeRandomSpawner : MonoBehaviour
{
    [Header("基础设置")]
    public Terrain targetTerrain;
    public GameObject treePrefab;
    public int treeCount = 3000;

    [Header("检测禁区")]
    public string noTreeTag = "NoTreeZone";
    public float checkRadius = 12f;         // 镜子周围多大范围不长树

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        if (targetTerrain == null || treePrefab == null) return;

        TerrainData td = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;
        Vector3 terrainSize = td.size;

        int spawned = 0;
        int attempts = 0;
        int maxAttempts = treeCount * 5; // 防止死循环

        // 用 while 保证凑够数量，避开禁区
        while (spawned < treeCount && attempts < maxAttempts)
        {
            attempts++;

            // 随机地形位置
            float x = Random.Range(terrainPos.x, terrainPos.x + terrainSize.x);
            float z = Random.Range(terrainPos.z, terrainPos.z + terrainSize.z);
            float y = targetTerrain.SampleHeight(new Vector3(x, 0, z));
            Vector3 spawnPos = new Vector3(x, y, z);

            // 检测当前点 半径内 有没有 NoTreeZone 标签物体
            bool isInForbidden = IsInForbiddenArea(spawnPos);
            if (isInForbidden)
                continue;

            // 不在禁区 → 生成树
            GameObject t = Instantiate(
                treePrefab,
                spawnPos,
                Quaternion.Euler(0, Random.Range(0, 360), 0)
            );
            t.transform.parent = targetTerrain.transform;
            spawned++;
        }
    }

    // 检测该位置是否在镜子/禁区范围内
    bool IsInForbiddenArea(Vector3 pos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, checkRadius);
        foreach (var col in hitColliders)
        {
            if (col.CompareTag(noTreeTag))
            {
                return true;
            }
        }
        return false;
    }
}