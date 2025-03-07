using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public int maxActionPoints = 2;
    public int maxHealth = 100;
    public bool isEnemy;
}
