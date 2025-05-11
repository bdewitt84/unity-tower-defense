using System;
using System.Collections.Generic;
using UnityEngine;


// Author: Brett DeWitt
//
// Created: 4.27.2025
//
// Description:
//
// Manages enemy wave spawning.
// Spawns enemies at regular intervals during a wave, 
// and controls the timing between successive waves.


public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1.0f;
    private float timeSinceLastSpawn = 0.0f;
    private float timeSinceLastWave = 0.0f;
    [SerializeField] private float waveInterval = 5.0f;
    private bool waveInProgress = false;
    private Queue<GameObject> wave = new();
    [SerializeField] private int waveSize = 5;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform lane;
    private int waveIndex = 0;

    [SerializeField] private List<WaveData> waves;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (WaveInProgress())
        {
            if (TimeForNextSpawn())
            {
                SpawnNextEnemy();
                if (AllEnemiesSpawned())
                {
                    EndWave();
                }
            }
            UpdateSpawnTimer();
        } else {
            UpdateWaveTimer();
            if (TimeForWave())
            {
                if (AllWavesCleared())
                {
                    GameEvents.GameClear();
                } else
                {
                    InitializeWave();
                    ResetWaveTimer();
                    StartNextWave();
                }
            }
        }
    }

    private bool AllWavesCleared()
    {
        return waveIndex >= waves.Count;
    }

    // Returns whether a wave is currently active
    private bool WaveInProgress()
    {
        return waveInProgress;
    }

    // Returns whether enough time has passed to spawn the next enemy
    private bool TimeForNextSpawn()
    {
        return (timeSinceLastSpawn >= spawnInterval);
    }

    // Spawns the next enemy from the wave queue
    private void SpawnNextEnemy()
    {
        GameObject nextEnemy = wave.Dequeue();
        nextEnemy.gameObject.SetActive(true);
        timeSinceLastSpawn = 0.0f;
    }

    // Returns whether all enemies in the current wave have been spawned
    private bool AllEnemiesSpawned()
    {
        return (wave.Count <= 0);
    }

    // Ends the current wave
    private void EndWave()
    {
        waveIndex += 1;
        waveInProgress = false;
    }

    // Updates the spawn timer each frame
    private void UpdateSpawnTimer()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }

    // Updates the wave timer each frame
    private void UpdateWaveTimer()
    {
        timeSinceLastWave += Time.deltaTime;
    }

    // Returns whether enough time has passed to start a new wave
    private bool TimeForWave()
    {
        return timeSinceLastWave >= waveInterval;
    }

    // Creates a new wave of enemy objects and queues them for spawning
    private void InitializeWave()
    {
        wave.Clear();
        WaveData currentWaveData = waves[waveIndex];
        spawnInterval = currentWaveData.spawnInterval;
        waveSize = currentWaveData.enemyCount;
        waveInterval = currentWaveData.spawnDelay;

        for(int i = 0; i < waveSize; i++)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyObject.SetActive(false);
            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
            LanePathFinder lanePathFinder = new(enemyController, lane);
            enemyController.SetPathfindingComponent(lanePathFinder);
            enemyObject.transform.position = spawnPoint.position;
            wave.Enqueue(enemyObject);
        }
    }

    // Resets the timer used for wave intervals
    private void ResetWaveTimer()
    {
        timeSinceLastWave = 0.0f;
    }

    // Starts a new enemy wave
    private void StartNextWave()
    {
        waveInProgress = true;
    }

}
