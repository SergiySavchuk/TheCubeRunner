using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//Main script
public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelSpawner levelSpawner;

    [SerializeField] private float floorSpeed = 1f;

    [Description("How many chanks of levels have to be spawned at the begining")]
    [SerializeField] private Floor[] floors;

    [SerializeField] private GameObject gameOverUI;

    private Queue<Floor> floorsQueue = new();

    private Floor floorToCheck;

    private bool pause = true;
    public bool Pause => pause;

    //For reseting player position at the restarting of the game
    public Action OnGameStart;

    private void Start()
    {
        LoadStartingLevls();
    }

    private void LoadStartingLevls()
    {
        floorsQueue.Clear();

        floorToCheck = floors[0];

        float levelsLenth = 0;

        for (int i = 0; i < floors.Length - 1; i++)
        {
            levelSpawner.SpawnLevel(floors[i]);
            floors[i].transform.position = new Vector3(levelsLenth + floors[i].GameObjectLength / 2f, 0);
            floorsQueue.Enqueue(floors[i]);

            if (i < floors.Length - 2)
                levelsLenth += floors[i].LevelLength;
        }

        //Last level is moving up to hide how it's appearing 
        levelSpawner.SpawnLevel(floors[^1]);
        floors[^1].transform.position = new Vector3(levelsLenth + floors[^1].GameObjectLength / 2f, -floors[^2].LevelLength);
        floorsQueue.Enqueue(floors[^1]);
    }

    //For UI button "Tap to play"
    public void StartGame() 
    {
        pause = false;
    }

    private void Update()
    {
        if (pause) return;

        //Moving floors. The last one moves up
        foreach (var item in floorsQueue)
            item.Move(floorSpeed * Time.deltaTime * (item.VerticalPosition < 0 ? Vector3.up : Vector3.left));

        //Checking if position of the first floor is beyond camera. If it is true than creating new level
        if (floorToCheck.IsOutOfBounds)
            CreateNewLevel();
    }

    private void CreateNewLevel()
    {
        Floor oldFloor = floorsQueue.Dequeue();

        oldFloor.ReturnCubesToParents();

        floorToCheck = floorsQueue.Peek();

        float levelsLenth = 0f;
        float lastLevelLenth = 0f;

        foreach (var floor in floorsQueue)
        {
            levelsLenth += floor.LevelLength;
            lastLevelLenth = floor.LevelLength;
        }

        levelsLenth -= lastLevelLenth;

        oldFloor.transform.position = new Vector3(levelsLenth, -lastLevelLenth);

        levelSpawner.SpawnLevel(oldFloor);

        floorsQueue.Enqueue(oldFloor);
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

        //For reseting player position at the restarting of the game
        OnGameStart?.Invoke();
    }
}
