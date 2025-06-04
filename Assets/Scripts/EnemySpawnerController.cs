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
        if (GameIsOver()) return;

        if (waveIndex != -1)
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
            }
            else
            {
                UpdateWaveTimer();
                if (TimeForWave())
                {
                    if (AllWavesCleared())
                    {
                        GameEvents.StageClear();
                        waveIndex = -1;
                    }
                    else
                    {
                        InitializeWave();
                        ResetWaveTimer();
                        StartNextWave();
                    }
                }
            }
        }
    }

    private bool GameIsOver()
    {
        return Time.timeScale == 0f;
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
        waveInterval = currentWaveData.spawnDelay;
        int numEnemyTypes = currentWaveData.enemyTypes.Count;
        for (int typeIndex = 0; typeIndex < numEnemyTypes; typeIndex++)
        {
            waveSize = currentWaveData.enemyCounts[typeIndex];
            for (int enemyIndex = 0; enemyIndex < waveSize; enemyIndex++)
            {
                GameObject enemyObject = Instantiate(currentWaveData.enemyTypes[typeIndex], spawnPoint.position, Quaternion.identity);
                enemyObject.SetActive(false);
                EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
                LanePathFinder lanePathFinder = new(enemyController, lane);
                enemyController.SetPathfindingComponent(lanePathFinder);
                enemyObject.transform.position = spawnPoint.position;
                wave.Enqueue(enemyObject);
            }
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
