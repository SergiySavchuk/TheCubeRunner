using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Floor : MonoBehaviour
{
    public float Position => transform.position.x;
    public float Length => transform.localScale.x;

    private List<Cube> cubes = new();

    public void SetCubes(List<Cube> cubes)
    {
        this.cubes = cubes;
    }

    public void RemoveCube(Cube cube)
    {
        cubes.Remove(cube);
    }

    public void ReturnCubesToParents()
    {
        foreach (var cube in cubes)
            cube.ReturnToTrueParent();

        cubes = new();
    }

    public void Move(Vector3 moveTo)
    {
        transform.Translate(moveTo);
    }
}
