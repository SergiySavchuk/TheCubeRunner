using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelSpawner levelSpawner;

    [SerializeField] private float floorSpeed = 1f;

    [Description("How many chanks of levels have to be spawned at the begining")]
    [SerializeField] private Floor[] floors;

    [SerializeField] private GameObject gameOverUI;

    private Queue<Floor> floorsQueue;

    private Floor floorToCheck;

    private bool pause = true;
    public bool Pause => pause;

    private void Start()
    {
        LoadStartingLevls();
    }

    private void LoadStartingLevls()
    {
        floorsQueue = new Queue<Floor>();

        floorToCheck = floors[0];

        floorToCheck.transform.position = Vector3.zero + new Vector3(floorToCheck.Length / 2f, 0, 0);
        levelSpawner.SpawnLevel(floorToCheck);
        floorsQueue.Enqueue(floorToCheck);

        for (int i = 1; i < floors.Length; i++)
        {
            floors[i].transform.position = new Vector3(floorToCheck.Length * i + floorToCheck.Length / 2f, 0);
            levelSpawner.SpawnLevel(floors[i]);
            floorsQueue.Enqueue(floors[i]);
        }       
    }

    public void StartGame() // for UI button
    {
        pause = false;
    }

    private void Update()
    {
        if (pause) return;

        foreach (var item in floorsQueue)
        {
            item.Move(floorSpeed * Time.deltaTime * Vector3.left);
        }

        if (floorToCheck.Position < -floorToCheck.Length)
        {
            Floor oldFloor = floorsQueue.Dequeue();

            oldFloor.ReturnCubesToParents();

            floorToCheck = floorsQueue.Peek();

            oldFloor.transform.position = new Vector3(floorToCheck.Length * (floors.Length - 1), 0);

            levelSpawner.SpawnLevel(oldFloor);

            floorsQueue.Enqueue(oldFloor);
        }
    }

    public void GameOver()
    {
        pause = true;
        gameOverUI.SetActive(true);
    }

    public void PlayAgain()
    {
        gameOverUI.SetActive(false);

        foreach (Floor floor in floors)
            floor.ReturnCubesToParents();            

        LoadStartingLevls();

        pause = false;
    }
}
