using System.Collections.Generic;
using UnityEngine;

//Class for level creation
public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private LevelSO[] levels;

    public readonly static int maximumLanes = 5;

    private readonly float gridSize = 2f;
    private readonly float startingZPoint = 4f;
    private readonly float startingYPoint = 1.5f;

    public Queue<Cube> SpawnLevel(Floor floor)
    {
        List<Cube> cubes = new();
        Queue<Cube> cubesQueue = new();

        LevelSO levelSO = levels[Random.Range(0, levels.Length)];

        float x = 0;

        Vector3 spawnPosition = floor.transform.localPosition - new Vector3(floor.GameObjectLength / 2f, 0, 0);

        foreach (RowSO row in levelSO.cubesInLanes)
        {
            x += levelSO.rowStep * gridSize;

            float z = startingZPoint;

            for (int i = 0; i < row.cubesInLanes.Length && i <= maximumLanes; i++)
            {
                float y = startingYPoint;

                for (int j = 1; j <= row.cubesInLanes[i]; j++)
                {
                    GameObject cube = cubeSpawner.GetCube(row.goodCube, floor);

                    cube.transform.position = spawnPosition + new Vector3(x, y, z);
                    cube.transform.SetParent(floor.transform);

                    cubes.Add(cube.GetComponent<Cube>());
                    cubesQueue.Enqueue(cube.GetComponent<Cube>());

                    y += gridSize;
                }

                z -= gridSize;
            }            
        }

        floor.SetValues(cubes, x);

        return cubesQueue;
    }
}
