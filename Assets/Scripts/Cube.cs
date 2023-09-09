using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;

    [SerializeField] private bool goodCube;

    private bool checkPlayerPosiotion = false;
    private float playersX;
    private Action<Transform> callback;
    private Floor parentFloor;

    public void SetData(CubeSpawner spawner, bool goodCube, Floor floor)
    {
        this.spawner = spawner;
        this.goodCube = goodCube;
        parentFloor = floor;
    }

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
