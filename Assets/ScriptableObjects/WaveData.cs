using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "TowerDefense/WaveData")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public float spawnDelay;
    public int[] enemyCounts;
    public List<GameObject> enemyTypes;
    public float spawnInterval;
}
