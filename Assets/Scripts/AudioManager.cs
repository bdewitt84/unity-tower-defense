using UnityEngine;

// Author: Minsu Kim
//
// Created: 05/11/2025
//
// Description:
// Manages playback of game sound effects and background music in response to game events.

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip enemyGoalClip;
    [SerializeField] private AudioClip towerFireClip;
    [SerializeField] private AudioClip placementSuccessClip;
    [SerializeField] private AudioClip placementFailClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip gameClearClip;

    [Header("SFX Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float bgmVolume = 0.3f;

    private AudioSource bgmSource;

    private void Awake()
    {
        // BGM¿ë AudioSource »ý¼º
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.Play();
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
        GameEvents.OnTowerPlacementSuccess += HandleTowerPlacementSuccess;
        GameEvents.OnTowerPlacementInvalid += HandleTowerPlacementFail;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnGameClear += HandleGameClear;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
        GameEvents.OnTowerPlacementSuccess -= HandleTowerPlacementSuccess;
        GameEvents.OnTowerPlacementInvalid -= HandleTowerPlacementFail;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnGameClear -= HandleGameClear;
    }

    private void HandleEnemyKilled(EnemyController enemy)
    {
        PlaySFX(enemyDeathClip);
    }

    private void HandleEnemyReachedGoal(EnemyController enemy)
    {
        PlaySFX(enemyGoalClip);
    }

    private void HandleTowerPlacementSuccess(GameObject towerInstance)
    {
        PlaySFX(placementSuccessClip);
    }

    private void HandleTowerPlacementFail(int x, int y)
    {
        PlaySFX(placementFailClip);
    }

    private void HandleGameOver()
    {
        PlaySFX(gameOverClip);
    }

    private void HandleGameClear()
    {
        PlaySFX(gameClearClip);
    }

    public void PlayTowerFireSound()
    {
        PlaySFX(towerFireClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
