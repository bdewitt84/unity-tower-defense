using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "TowerDefense/WaveData")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public float spawnDelay;
    public int enemyCount;
    public float spawnInterval;
}
