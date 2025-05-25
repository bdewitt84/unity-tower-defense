// TowerPreviewManager.cs
using UnityEngine;

public class TowerPreviewManager : MonoBehaviour
{
    [Tooltip("The material to apply to the instantiated ghost preview.")]
    [SerializeField] private Material _ghostMaterial;
    [SerializeField] private GameBoardController _gameBoardController;

    private GameObject _currentPreviewInstance; // The actual instantiated GameObject
    private GameObject _lastRequestedPrefab;    // To track if the source prefab has changed

    // --- Event Subscriptions ---
    private void OnEnable()
    {
        GameEvents.OnTowerPreviewRequest += HandleTowerPreviewRequest;
        GameEvents.OnTowerPreviewDisable += HandleTowerPreviewDisable;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerPreviewRequest -= HandleTowerPreviewRequest;
        GameEvents.OnTowerPreviewDisable -= HandleTowerPreviewDisable;

        // Clean up any lingering preview instance if the manager is disabled
        if (_currentPreviewInstance != null)
        {
            Destroy(_currentPreviewInstance);
            _currentPreviewInstance = null;
        }
    }

    private void HandleTowerPreviewDisable()
    {
        Destroy(_currentPreviewInstance);
        _currentPreviewInstance = null;
    }

    public void HandleTowerPreviewRequest(Vector3 worldPosition, GameObject towerPrefab)
    {

        if (towerPrefab == null) // Signal to hide the preview
        {
            if (_currentPreviewInstance != null)
            {
                Destroy(_currentPreviewInstance);
                _currentPreviewInstance = null;
                _lastRequestedPrefab = null; // Clear last prefab, as it's no longer relevant

            }
            return; // Nothing more to do if hiding
        }


        // If no instance exists OR the requested prefab is different from the current one
        if (_currentPreviewInstance == null || _lastRequestedPrefab != towerPrefab)
        {
            // Destroy any existing instance before creating a new one
            if (_currentPreviewInstance != null)
            {
                Destroy(_currentPreviewInstance);
                _currentPreviewInstance = null;
            }

            // Instantiate the new preview GameObject
            _currentPreviewInstance = Instantiate(towerPrefab);
            _lastRequestedPrefab = towerPrefab; // Remember which prefab this instance came from
            _currentPreviewInstance.name = "TowerPreviewInstance"; // Give it a clear name in hierarchy


            // Disable all colliders on the instance and its children.
            Collider[] colliders = _currentPreviewInstance.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            // Disable all scripts (MonoBehaviours) on the instance and its children.
            MonoBehaviour[] scripts = _currentPreviewInstance.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }

            // Apply the ghost material to all MeshRenderers in the instance and its children.
            MeshRenderer[] renderers = _currentPreviewInstance.GetComponentsInChildren<MeshRenderer>();
            if (_ghostMaterial == null)
            {
                Debug.LogError("TowerPreviewManager: _ghostMaterial is NULL! Preview will not be visible.", this);
            }
            else
            {
                foreach (MeshRenderer renderer in renderers)
                {
                    renderer.material = _ghostMaterial; // Assign the ghost material
                }
            }
        }

        if (_currentPreviewInstance != null)
        {
            worldPosition = _gameBoardController.SnapToGrid(worldPosition);
            _currentPreviewInstance.transform.position = worldPosition;
        }
    }
}