// TowerPreviewManager.cs
using UnityEngine; // Always needed for Unity basics

public class TowerPreviewManager : MonoBehaviour
{
    [Tooltip("The material to apply to the instantiated ghost preview.")]
    [SerializeField] private Material _ghostMaterial;

    [Tooltip("Reference to the GameBoardController for grid snapping and validation.")]
    [SerializeField] private GameBoardController _gameBoardController; // Added this field

    private readonly Color _validColor = new Color(0f, 1f, 0f, 0.25f); // Green, semi-transparent
    private readonly Color _invalidColor = new Color(1f, 0f, 0f, 0.25f); // Red, semi-transparent

    private GameObject _currentPreviewInstance; // The actual instantiated GameObject
    private GameObject _lastRequestedPrefab;    // To track if the source prefab has changed

    private void OnEnable()
    {
        Debug.Log("TowerPreviewManager: OnEnable called.");
        GameEvents.OnTowerPreviewRequest += HandleTowerPreviewRequest;
        GameEvents.OnTowerPreviewDisable += HandleTowerPreviewDisable;
    }

    private void OnDisable()
    {
        Debug.Log("TowerPreviewManager: OnDisable called.");
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
        if (_currentPreviewInstance != null)
        {
            Destroy(_currentPreviewInstance);
            _currentPreviewInstance = null;
            _lastRequestedPrefab = null; // Clear last prefab as we're no longer previewing it
        }
    }

    public void HandleTowerPreviewRequest(Vector3 worldPosition, GameObject towerPrefab)
    {

        // If no instance exists OR the requested prefab is different from the current one
        if (_currentPreviewInstance == null || _lastRequestedPrefab != towerPrefab)
        {
            // Destroy any existing instance before creating a new one
            if (_currentPreviewInstance != null)
            {
                DestroyCurrentInstance();
            }

            InstantiatePreview(towerPrefab);
            DisablePreviewScripts();
            ApplyPreviewMaterial();
        }

        // Update Position and Visual Validation (always for the currently active instance)
        if (_currentPreviewInstance != null)
        {
            Vector3 snappedPosition = SnapPreviewToGrid(worldPosition);
            Color finalPreviewColor = DecidePreviewColor(snappedPosition);
            ApplyPreviewColor(finalPreviewColor);
        }
    }

    private void ApplyPreviewColor(Color finalPreviewColor)
    {
        MeshRenderer[] renderers = _currentPreviewInstance.GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = finalPreviewColor;
            renderer.material.SetColor("_BaseColor", finalPreviewColor); // <-- Use this for URP Lit

        }
    }

    private Color DecidePreviewColor(Vector3 snappedPosition)
    {
        return _gameBoardController.CanPlaceTower(snappedPosition) ? _validColor : _invalidColor;
    }

    private Vector3 SnapPreviewToGrid(Vector3 worldPosition)
    {
        Vector3 snappedPosition = _gameBoardController.SnapToGrid(worldPosition);
        _currentPreviewInstance.transform.position = snappedPosition;
        return snappedPosition;
    }

    private void ApplyPreviewMaterial()
    {
        MeshRenderer[] renderers = _currentPreviewInstance.GetComponentsInChildren<MeshRenderer>(true); // Include inactive renderers
        {
            foreach (MeshRenderer renderer in renderers)
            {
                // Assign the ghost material to the renderer's first material slot.
                // This creates a unique material instance for this renderer.
                renderer.material = _ghostMaterial;
            }
        }
    }

    private void DisablePreviewScripts()
    {
        MonoBehaviour[] scripts = _currentPreviewInstance.GetComponentsInChildren<MonoBehaviour>(true); // Include inactive scripts
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    private void InstantiatePreview(GameObject towerPrefab)
    {
        _currentPreviewInstance = Instantiate(towerPrefab);
        _lastRequestedPrefab = towerPrefab; // Remember which prefab this instance came from
        _currentPreviewInstance.name = "TowerPreviewInstance"; // Give it a clear name in hierarchy
    }

    private void DestroyCurrentInstance()
    {
        Destroy(_currentPreviewInstance);
        _currentPreviewInstance = null;
    }
}