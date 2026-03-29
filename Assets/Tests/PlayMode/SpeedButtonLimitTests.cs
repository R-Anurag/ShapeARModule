using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class SpeedButtonLimitTests
{
    private GameObject _controllerGO;
    private ARPlayUIController _controller;
    private SpinBehaviour _spin;
    private ScaleBehaviour _scale;

    [SetUp]
    public void SetUp()
    {
        _controllerGO = new GameObject("ARPlayUIController");
        _controller = _controllerGO.AddComponent<ARPlayUIController>();

        // Spin speed buttons
        _controller.increaseSpeedButton = new GameObject("IncSpin").AddComponent<Button>();
        _controller.decreaseSpeedButton = new GameObject("DecSpin").AddComponent<Button>();

        // Scale speed buttons
        _controller.scaleIncreaseSpeedButton = new GameObject("IncScale").AddComponent<Button>();
        _controller.scaleDecreaseSpeedButton = new GameObject("DecScale").AddComponent<Button>();

        // Behaviour components the controller adjusts
        var shapeGO = new GameObject("Shape");
        _spin  = shapeGO.AddComponent<SpinBehaviour>();
        _scale = shapeGO.AddComponent<ScaleBehaviour>();

        _controller.internal_SetSpinBehaviour(_spin);
        _controller.internal_SetScaleBehaviour(_scale);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_controller.increaseSpeedButton.gameObject);
        Object.DestroyImmediate(_controller.decreaseSpeedButton.gameObject);
        Object.DestroyImmediate(_controller.scaleIncreaseSpeedButton.gameObject);
        Object.DestroyImmediate(_controller.scaleDecreaseSpeedButton.gameObject);
        Object.DestroyImmediate(_spin.gameObject);
        Object.DestroyImmediate(_controllerGO);
    }

    // ── Spin speed ────────────────────────────────────────────────────────────

    [Test]
    public void SpinSpeed_AtMinimum_DecreaseButtonIsNotInteractable()
    {
        _spin.speed = 40f; // one step above min (10f)
        _controller.internal_AdjustSpinSpeed(-30f); // brings it to 10f
        Assert.IsFalse(_controller.decreaseSpeedButton.interactable);
    }

    [Test]
    public void SpinSpeed_AtMinimum_IncreaseButtonRemainsInteractable()
    {
        _spin.speed = 40f;
        _controller.internal_AdjustSpinSpeed(-30f);
        Assert.IsTrue(_controller.increaseSpeedButton.interactable);
    }

    [Test]
    public void SpinSpeed_AtMaximum_IncreaseButtonIsNotInteractable()
    {
        _spin.speed = 330f; // one step below max (360f)
        _controller.internal_AdjustSpinSpeed(30f);
        Assert.IsFalse(_controller.increaseSpeedButton.interactable);
    }

    [Test]
    public void SpinSpeed_AtMaximum_DecreaseButtonRemainsInteractable()
    {
        _spin.speed = 330f;
        _controller.internal_AdjustSpinSpeed(30f);
        Assert.IsTrue(_controller.decreaseSpeedButton.interactable);
    }

    [Test]
    public void SpinSpeed_InMiddle_BothButtonsInteractable()
    {
        _spin.speed = 180f;
        _controller.internal_AdjustSpinSpeed(30f);
        Assert.IsTrue(_controller.increaseSpeedButton.interactable);
        Assert.IsTrue(_controller.decreaseSpeedButton.interactable);
    }

    // ── Scale speed ───────────────────────────────────────────────────────────

    [Test]
    public void ScaleSpeed_AtMinimum_DecreaseButtonIsNotInteractable()
    {
        _scale.speed = 1f; // one step above min (0.5f)
        _controller.internal_AdjustScaleSpeed(-0.5f);
        Assert.IsFalse(_controller.scaleDecreaseSpeedButton.interactable);
    }

    [Test]
    public void ScaleSpeed_AtMinimum_IncreaseButtonRemainsInteractable()
    {
        _scale.speed = 1f;
        _controller.internal_AdjustScaleSpeed(-0.5f);
        Assert.IsTrue(_controller.scaleIncreaseSpeedButton.interactable);
    }

    [Test]
    public void ScaleSpeed_AtMaximum_IncreaseButtonIsNotInteractable()
    {
        _scale.speed = 9.5f; // one step below max (10f)
        _controller.internal_AdjustScaleSpeed(0.5f);
        Assert.IsFalse(_controller.scaleIncreaseSpeedButton.interactable);
    }

    [Test]
    public void ScaleSpeed_AtMaximum_DecreaseButtonRemainsInteractable()
    {
        _scale.speed = 9.5f;
        _controller.internal_AdjustScaleSpeed(0.5f);
        Assert.IsTrue(_controller.scaleDecreaseSpeedButton.interactable);
    }

    [Test]
    public void ScaleSpeed_InMiddle_BothButtonsInteractable()
    {
        _scale.speed = 5f;
        _controller.internal_AdjustScaleSpeed(0.5f);
        Assert.IsTrue(_controller.scaleIncreaseSpeedButton.interactable);
        Assert.IsTrue(_controller.scaleDecreaseSpeedButton.interactable);
    }
}
