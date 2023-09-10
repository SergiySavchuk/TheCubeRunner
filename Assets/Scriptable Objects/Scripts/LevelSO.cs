using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class LevelSO : ScriptableObject
{
    public RowSO[] cubesInLanes;
    public float rowStep = 5f;
}
