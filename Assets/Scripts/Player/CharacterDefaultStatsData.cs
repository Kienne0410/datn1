using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStatsData", menuName = "Scriptable Objects/CharacterStatsData")]
public class CharacterDefaultStatsData : ScriptableObject
{
    public CharacterType characterType;
    public CharacterDefaultStats[] characterStats;
}
