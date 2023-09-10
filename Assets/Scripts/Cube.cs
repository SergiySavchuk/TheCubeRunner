using System;
using UnityEngine;

//Class for controlling cubes
public class Cube : MonoBehaviour
{
    private CubeSpawner spawner;
    private bool goodCube;
    private bool checkPlayerPosiotion = false;
    private float playersX;
    private Action<Transform> callback;
    private Floor parentFloor;

    private LayerMask defaultLayerMask;

    private void Awake()
    {
        defaultLayerMask = gameObject.layer; //saving layer for reseting when needed
    }

    public void SetData(CubeSpawner spawner, bool goodCube, Floor floor)
    {
        this.spawner = spawner;
        this.goodCube = goodCube;
        parentFloor = floor;

        gameObject.layer = defaultLayerMask;
    }

    //For smooth jumping 
    public void StartCheckingPlayerPosiotion(float playersX, Action<Transform> callback)
    {
        checkPlayerPosiotion = true;
        this.playersX = playersX;
        this.callback = callback;
    }

    public void ReturnToTrueParent()
    {
        gameObject.SetActive(false);
        spawner.ReturnToParent(gameObject, goodCube);
    }

    private void Update()
    {
        if (checkPlayerPosiotion && transform.position.x < playersX)
        {
            checkPlayerPosiotion = false;
            callback.Invoke(transform);
            parentFloor.RemoveCube(this);
        }
    }
}
