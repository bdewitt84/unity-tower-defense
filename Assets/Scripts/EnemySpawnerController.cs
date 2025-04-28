using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

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
                InitializeWave();
                ResetWaveTimer();
                StartNextWave();
            }
        }
    }

    private bool WaveInProgress()
    {
        return waveInProgress;
    }

    private bool TimeForNextSpawn()
    {
        return (timeSinceLastSpawn >= spawnInterval);
    }

    private void SpawnNextEnemy()
    {
        GameObject nextEnemy = wave.Dequeue();
        nextEnemy.gameObject.SetActive(true);
        timeSinceLastSpawn = 0.0f;
    }

    private bool AllEnemiesSpawned()
    {
        return (wave.Count <= 0);
    }

    private void EndWave()
    {
        waveInProgress = false;
    }

    private void UpdateSpawnTimer()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }

    private void UpdateWaveTimer()
    {
        timeSinceLastWave += Time.deltaTime;
    }

    private bool TimeForWave()
    {
        return timeSinceLastWave >= waveInterval;
    }

    private void InitializeWave()
    {
        wave.Clear();
        for(int i = 0; i < waveSize; i++)
        {
            GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemyObject.SetActive(false);
            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
            enemyController.SetLane(lane);
            enemyObject.transform.position = spawnPoint.position;
            wave.Enqueue(enemyObject);
        }
    }

    private void ResetWaveTimer()
    {
        timeSinceLastWave = 0.0f;
    }

    private void StartNextWave()
    {
        waveInProgress = true;
    }

}
