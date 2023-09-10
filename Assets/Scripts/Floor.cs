using System.Collections.Generic;
using UnityEngine;

//Class for controlling levels and gameObject that moves cubes
public class Floor : MonoBehaviour
{
    public bool IsOutOfBounds => transform.position.x < -LevelLength;
    public float VerticalPosition => transform.position.y;
    public float GameObjectLength => transform.localScale.x;

    public float LevelLength { get; private set; }

    private List<Cube> cubes = new();

    public void SetValues(List<Cube> cubes, float levelLength)
    {
        this.cubes = cubes;
        LevelLength = levelLength;
    }

    //For removing the cube that player took
    public void RemoveCube(Cube cube)
    {
        cubes.Remove(cube);
    }

    //For removing all cubes
    public void ReturnCubesToParents()
    {
        foreach (var cube in cubes)
            cube.ReturnToTrueParent();

        cubes.Clear();
    }

    public void Move(Vector3 moveTo)
    {
        transform.Translate(moveTo);
    }
}
