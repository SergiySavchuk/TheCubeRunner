using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Transform goodCubesParent;
    [SerializeField] private Transform badCubesParent;

    [SerializeField] private GameObject goodCubePrefab;
    [SerializeField] private GameObject badCubePrefab;

    private Queue<GameObject> goodCubes;
    private Queue<GameObject> badCubes;

    private int goodCubesQuantety, badCubesQuantety;

    private void Awake()
    {
        goodCubes = new Queue<GameObject>();
        badCubes = new Queue<GameObject>();

        for (int i = 0; i < goodCubesParent.childCount; i++)
            goodCubes.Enqueue(goodCubesParent.GetChild(i).gameObject);

        for (int i = 0; i < badCubesParent.childCount; i++)
            badCubes.Enqueue(badCubesParent.GetChild(i).gameObject);

        goodCubesQuantety = goodCubes.Count;
        badCubesQuantety = badCubes.Count;
    }

    public GameObject GetCube(bool goodCube, Floor parentFloor)
    {
        Queue<GameObject> cubesToCheck = goodCube ? goodCubes : badCubes;

        if (!cubesToCheck.TryDequeue(out GameObject returnValue))
        {
            returnValue = Instantiate(goodCube ? goodCubePrefab : badCubePrefab, goodCube ? goodCubesParent : badCubesParent);

            if (goodCube)
            {
                goodCubesQuantety++;
                returnValue.name = $"Good Cube ({goodCubesQuantety}) clone";
            }
            else
            {
                badCubesQuantety++;
                returnValue.name = $"Bad Cube ({badCubesQuantety}) clone";
            }
        }

        returnValue.SetActive(true);
        returnValue.GetComponent<Cube>().SetData(this, goodCube, parentFloor);

        return returnValue;
    }

    public void ReturnToParent(GameObject childToReturn, bool goodChild)
    {
        if (goodChild)
        {
            childToReturn.transform.SetParent(goodCubesParent);
            goodCubes.Enqueue(childToReturn);
        }
        else
        {
            childToReturn.transform.SetParent(badCubesParent);
            badCubes.Enqueue(childToReturn);
        }
    }
}
