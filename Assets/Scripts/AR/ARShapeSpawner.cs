using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ShapeARModule.Tests.PlayMode")]

public class ARShapeSpawner : MonoBehaviour
{
    [Header("AR References")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("Shape Prefabs")]
    public GameObject shapeCubePrefab;
    public GameObject shapeSpherePrefab;
    public GameObject shapeCylinderPrefab;
    public GameObject shapePyramidPrefab;

    [Header("Input")]
    public InputActionReference tapAction;

    public event Action<GameObject> OnShapeSpawned;

    private readonly List<ARRaycastHit> _hits = new();
    private bool _spawned = false;
    private GameObject _spawnedShape;
    internal Collider _spawnedCollider;
    internal bool _isTappable;

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
#endif
    }

    void OnEnable()  { if (tapAction != null) tapAction.action.performed += OnTap; }
    void OnDisable() { if (tapAction != null) tapAction.action.performed -= OnTap; }

    void OnTap(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasPressedThisFrame()) return;

        Vector2 screenPos;

#if UNITY_EDITOR
        if (Mouse.current == null) return;
        screenPos = Mouse.current.position.ReadValue();
#else
        var touch = Touchscreen.current?.primaryTouch;
        if (touch == null) return;
        screenPos = touch.position.ReadValue();
#endif

        if (_spawned)
        {
            TryOpenLink(screenPos);
            return;
        }

        TrySpawnAt(screenPos);
    }

    void TryOpenLink(Vector2 screenPos)
    {
        if (!_isTappable) return;
        internal_TryOpenLink(_spawnedShape, _spawnedCollider, screenPos);
    }

    internal void internal_TryOpenLink(GameObject shape, Collider collider, Vector2 screenPos)
    {
        if (shape == null || collider == null) return;
        var link = shape.GetComponent<TappableLink>();
        if (link == null) return;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (collider.Raycast(ray, out _, 100f))
            link.Open();
    }

    void TrySpawnAt(Vector2 screenPosition)
    {
        if (!raycastManager.Raycast(screenPosition, _hits, TrackableType.PlaneWithinPolygon))
            return;

        ShapeModuleData data = ShapeModuleCache.data;

        if (string.IsNullOrEmpty(data.shapeName))
        {
            Debug.LogError("No shape selected in cache. Cannot spawn.");
            return;
        }

        GameObject prefab = GetPrefab(data.shapeName);

        if (prefab == null)
        {
            Debug.LogError($"No prefab found for shape: {data.shapeName}");
            return;
        }

        Pose hitPose = _hits[0].pose;

        GameObject anchorGO = new GameObject("AR Anchor");
        anchorGO.transform.position = hitPose.position;
        anchorGO.transform.rotation = hitPose.rotation;

        ARAnchor anchor = anchorGO.AddComponent<ARAnchor>();

        if (anchor == null)
        {
            Debug.LogError("Failed to create anchor.");
            return;
        }

        GameObject shape = Instantiate(prefab, anchor.transform);
        _spawnedCollider = shape.GetComponentInChildren<Collider>();
        _isTappable = shape.GetComponent<TappableLink>() != null;

        Camera cam = Camera.main;
        float distance = Vector3.Distance(cam.transform.position, hitPose.position);
        float scaleFactor = Mathf.Clamp(distance * 0.1f, 0.05f, 0.3f);
        shape.transform.localScale = Vector3.one * scaleFactor;

        AttachBehaviour(shape, data.behaviourName);

        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(false);

        planeManager.enabled = false;
        _spawned = true;

        _spawnedShape = shape;
        OnShapeSpawned?.Invoke(shape);
    }

    GameObject GetPrefab(string shapeName) => shapeName switch
    {
        "ShapeCube"     => shapeCubePrefab,
        "ShapeSphere"   => shapeSpherePrefab,
        "ShapeCylinder" => shapeCylinderPrefab,
        "ShapePyramid"  => shapePyramidPrefab,
        _               => null
    };

    void AttachBehaviour(GameObject go, string behaviourName)
    {
        switch (behaviourName)
        {
            case "Spin":   go.AddComponent<SpinBehaviour>();   break;
            case "Move":   go.AddComponent<MoveBehaviour>();   break;
            case "Bounce": go.AddComponent<BounceBehaviour>(); break;
            case "Scale":  go.AddComponent<ScaleBehaviour>();  break;
            default:
                Debug.LogWarning($"Unknown behaviour: {behaviourName}");
                break;
        }
    }
}
