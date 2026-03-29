using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ARShapeSpawnerTests
{
    private GameObject _spawnerGO;
    private ARShapeSpawner _spawner;
    private Camera _camera;

    [SetUp]
    public void SetUp()
    {
        // Camera required for ScreenPointToRay inside TryOpenLink
        var cameraGO = new GameObject("MainCamera");
        cameraGO.tag = "MainCamera";
        _camera = cameraGO.AddComponent<Camera>();

        _spawnerGO = new GameObject("ARShapeSpawner");
        _spawner = _spawnerGO.AddComponent<ARShapeSpawner>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_spawnerGO);
        Object.DestroyImmediate(_camera.gameObject);
    }

    // ── Issue 5 — Null collider does not throw ────────────────────────────────

    [Test]
    public void TryOpenLink_NullShape_DoesNotThrow()
    {
        Assert.DoesNotThrow(() =>
            _spawner.internal_TryOpenLink(null, null, Vector2.zero));
    }

    [Test]
    public void TryOpenLink_NullCollider_DoesNotThrow()
    {
        var shape = new GameObject("Shape");
        shape.AddComponent<TappableLink>().url = "https://example.com";

        Assert.DoesNotThrow(() =>
            _spawner.internal_TryOpenLink(shape, null, Vector2.zero));

        Object.DestroyImmediate(shape);
    }

    // ── Issue 6 — Non-tappable shape does not open link ───────────────────────

    [Test]
    public void TryOpenLink_ShapeWithoutTappableLink_DoesNotThrow()
    {
        var shape = new GameObject("Shape");
        var collider = shape.AddComponent<BoxCollider>();

        Assert.DoesNotThrow(() =>
            _spawner.internal_TryOpenLink(shape, collider, Vector2.zero));

        Object.DestroyImmediate(shape);
    }

    [UnityTest]
    public IEnumerator IsTappable_FalseByDefault()
    {
        Assert.IsFalse(_spawner._isTappable);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnedCollider_NullByDefault()
    {
        Assert.IsNull(_spawner._spawnedCollider);
        yield return null;
    }
}
