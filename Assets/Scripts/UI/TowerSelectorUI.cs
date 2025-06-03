// Assets/Scripts/UI/TowerSelectorUI.cs

using UnityEngine;
using UnityEngine.UI; // Required for the Button component

/// Author: Brett DeWitt
/// 
/// Date: 5.23.25
/// 
/// <summary>
/// Component attached to a UI Button that represents a specific tower type for selection.
/// </summary>

[RequireComponent(typeof(Button))]

public class TowerSelectorUI : MonoBehaviour
{
    [Tooltip("Basic Tower")]
    [SerializeField] private GameObject _towerPrefab;
    private Button _button;

    private TowerController _towerControllerOnPrefab;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        GameEvents.TowerSelected(GetTowerPrefab());
    }

    /// <summary>
    /// Gets the tower prefab associated with this UI element.
    /// </summary>
    public GameObject GetTowerPrefab()
    {
        return _towerPrefab;
    }

    /// <summary>
    /// Gets the cost of the tower associated with this UI element.
    /// </summary>
    public int GetTowerCost()
    {
        if (_towerControllerOnPrefab != null)
        {
            return _towerControllerOnPrefab.GetCost();
        }
        Debug.LogWarning($"TowerSelectorUI on {gameObject.name}: Could not get cost from prefab's TowerController. Returning 0.", this);
        return 0;
    }
}