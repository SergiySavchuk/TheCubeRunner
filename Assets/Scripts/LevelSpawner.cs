using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private CubeSpawner cubeSpawner;

    [SerializeField] private float levelStep = 5f;

    [Description("One random row that start a floor")]
    [SerializeField] private LevelSO[] startingLevels;
    [SerializeField] private LevelSO[] levels;

    private readonly float gridSize = 2f;
    private readonly float startingZPoint = 4f;
    private readonly float startingYPoint = 1.5f;
    private readonly int maximumRows = 5;

    public void SpawnLevel(Floor floor)
    {
        List<Cube> cubes = new List<Cube>();

        Vector3 spawnPosition = floor.transform.localPosition - new Vector3(floor.Length / 2f, 0, 0);

        float x = levelStep * gridSize;

        float levelsQuantety = floor.Length / (levelStep * gridSize);

        for (int f = 0; f < levelsQuantety; f++)
        {
            LevelSO level = f == 0 ? startingLevels[Random.Range(0, startingLevels.Length)] : levels[Random.Range(0, levels.Length)];

            float z = startingZPoint;

            for (int i = 0; i < level.cubesInLanes.Length && i <= maximumRows; i++)
            {
                float y = startingYPoint;

                for (int j = 1; j <= level.cubesInLanes[i]; j++)
                {
                    GameObject cube = cubeSpawner.GetCube(level.goodCube, floor);

                    cube.transform.position = spawnPosition + new Vector3(x, y, z);
                    cube.transform.SetParent(floor.transform);

                    cubes.Add(cube.GetComponent<Cube>());

                    y += gridSize;
                }

                z -= gridSize;
            }

            x += levelStep * gridSize;
        }

        floor.SetCubes(cubes);
    }
}
