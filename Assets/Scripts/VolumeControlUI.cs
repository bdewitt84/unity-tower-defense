using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Author: Minsu Kim
//
// Created: 05/11/2025
//
// Description:
//   Handles real-time master volume control using a UI slider.
//   Designed to be always visible in-game for user convenience.
//   (Can be changed later)

public class VolumeControlUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;          // Reference to the game's audio mixer
    [SerializeField] private Slider volumeSlider;       // UI slider for controlling volume

    private void Start()
    {
        // Initialize slider value to current mixer volume
        if (mixer.GetFloat("MasterVolume", out float currentVolume))
        {
            volumeSlider.value = Mathf.Pow(10, currentVolume / 20f); // Convert dB to 0~1 scale
        }

        // Add listener to handle volume changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Called whenever the slider value changes
    private void SetVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20f; // Convert 0~1 scale to dB
        mixer.SetFloat("MasterVolume", dB);
    }
}
