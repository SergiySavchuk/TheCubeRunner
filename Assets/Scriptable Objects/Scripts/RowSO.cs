using UnityEngine;

[CreateAssetMenu(fileName = "Row", menuName = "ScriptableObjects/Row")]
public class RowSO : ScriptableObject
{
    public bool goodCube;
    public int[] cubesInLanes;
}
