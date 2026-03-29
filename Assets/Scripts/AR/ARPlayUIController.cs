using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ShapeARModule.Tests.PlayMode")]

public class ARPlayUIController : MonoBehaviour
{
    [Header("AR")]
    public ARShapeSpawner spawner;

    [Header("Animation Panels")]
    public GameObject moveAnimationPanel;
    public GameObject spinAnimationPanel;
    public GameObject bounceAnimationPanel;
    public GameObject scaleAnimationPanel;

    [Header("Move Direction Buttons")]
    public SelectableButton horizontalButton;
    public SelectableButton verticalButton;
    public SelectableButton leftDiagonalButton;
    public SelectableButton rightDiagonalButton;

    [Header("Spin Axis Buttons")]
    public SelectableButton xAxisButton;
    public SelectableButton yAxisButton;
    public SelectableButton zAxisButton;

    [Header("Spin Speed")]
    public Button increaseSpeedButton;
    public Button decreaseSpeedButton;

    [Header("Scale Mode Buttons")]
    public SelectableButton pulseButton;
    public SelectableButton growOnlyButton;
    public SelectableButton shrinkOnlyButton;

    [Header("Scale Speed")]
    public Button scaleIncreaseSpeedButton;
    public Button scaleDecreaseSpeedButton;

    [Header("Bounce Sliders")]
    public Slider bounceSpeedSlider;
    public Slider bounceHeightSlider;

    [Header("Navigation")]
    public Button backButton;

    private MoveBehaviour   _moveBehaviour;
    private SpinBehaviour   _spinBehaviour;
    private BounceBehaviour _bounceBehaviour;
    private ScaleBehaviour  _scaleBehaviour;

    private const float SpeedMin = 10f;
    private const float SpeedMax = 360f;
    private const float ScaleSpeedMin = 0.5f;
    private const float ScaleSpeedMax = 10f;
    private const float SpeedStep = 30f;
    private const float ScaleSpeedStep = 0.5f;
    private const float BounceSpeedMin = 0.5f;
    private const float BounceSpeedMax = 10f;
    private const float BounceHeightMin = 0.02f;
    private const float BounceHeightMax = 0.5f;

    void Start()
    {
        ShowPanelForBehaviour(ShapeModuleCache.data.behaviourName);
        HideAllPanels();
        WireMoveButtons();
        WireSpinButtons();
        WireScaleButtons();
        WireBounceSliders();
        backButton.onClick.AddListener(GoBack);
        spawner.OnShapeSpawned += OnShapeSpawned;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[Key.Escape].wasPressedThisFrame)
            GoBack();
    }

    void OnDestroy() { if (spawner != null) spawner.OnShapeSpawned -= OnShapeSpawned; }

    void OnShapeSpawned(GameObject shape)
    {
        _moveBehaviour   = shape.GetComponent<MoveBehaviour>();
        _spinBehaviour   = shape.GetComponent<SpinBehaviour>();
        _bounceBehaviour = shape.GetComponent<BounceBehaviour>();
        _scaleBehaviour  = shape.GetComponent<ScaleBehaviour>();

        if (_bounceBehaviour != null)
        {
            _bounceBehaviour.speed  = bounceSpeedSlider.value;
            _bounceBehaviour.height = bounceHeightSlider.value;
        }

        if (_moveBehaviour  != null) RefreshMoveButtons(_moveBehaviour.direction);
        if (_spinBehaviour  != null) RefreshSpinButtons(_spinBehaviour.axis);
        if (_scaleBehaviour != null) RefreshScaleButtons(_scaleBehaviour.mode);

        ShowPanelForBehaviour(ShapeModuleCache.data.behaviourName);
    }

    // ── Panel visibility ──────────────────────────────────────────────────────

    void HideAllPanels()
    {
        moveAnimationPanel.SetActive(false);
        spinAnimationPanel.SetActive(false);
        bounceAnimationPanel.SetActive(false);
        scaleAnimationPanel.SetActive(false);
    }

    void ShowPanelForBehaviour(string behaviourName)
    {
        moveAnimationPanel.SetActive(behaviourName   == "Move");
        spinAnimationPanel.SetActive(behaviourName   == "Spin");
        bounceAnimationPanel.SetActive(behaviourName == "Bounce");
        scaleAnimationPanel.SetActive(behaviourName  == "Scale");
    }

    // ── Move ──────────────────────────────────────────────────────────────────

    void WireMoveButtons()
    {
        horizontalButton.GetComponent<Button>().onClick.AddListener(()    => SetMoveDirection(MoveBehaviour.MoveDirection.Horizontal));
        verticalButton.GetComponent<Button>().onClick.AddListener(()      => SetMoveDirection(MoveBehaviour.MoveDirection.Vertical));
        leftDiagonalButton.GetComponent<Button>().onClick.AddListener(()  => SetMoveDirection(MoveBehaviour.MoveDirection.LeftDiagonal));
        rightDiagonalButton.GetComponent<Button>().onClick.AddListener(() => SetMoveDirection(MoveBehaviour.MoveDirection.RightDiagonal));
        RefreshMoveButtons(MoveBehaviour.MoveDirection.Horizontal);
    }

    void SetMoveDirection(MoveBehaviour.MoveDirection direction)
    {
        if (_moveBehaviour != null) _moveBehaviour.direction = direction;
        RefreshMoveButtons(direction);
    }

    void RefreshMoveButtons(MoveBehaviour.MoveDirection active)
    {
        horizontalButton.SetSelected(active   == MoveBehaviour.MoveDirection.Horizontal);
        verticalButton.SetSelected(active     == MoveBehaviour.MoveDirection.Vertical);
        leftDiagonalButton.SetSelected(active == MoveBehaviour.MoveDirection.LeftDiagonal);
        rightDiagonalButton.SetSelected(active == MoveBehaviour.MoveDirection.RightDiagonal);
    }

    // ── Spin ──────────────────────────────────────────────────────────────────

    void WireSpinButtons()
    {
        xAxisButton.GetComponent<Button>().onClick.AddListener(() => SetSpinAxis(SpinBehaviour.SpinAxis.X));
        yAxisButton.GetComponent<Button>().onClick.AddListener(() => SetSpinAxis(SpinBehaviour.SpinAxis.Y));
        zAxisButton.GetComponent<Button>().onClick.AddListener(() => SetSpinAxis(SpinBehaviour.SpinAxis.Z));
        increaseSpeedButton.onClick.AddListener(() => AdjustSpinSpeed(SpeedStep));
        decreaseSpeedButton.onClick.AddListener(() => AdjustSpinSpeed(-SpeedStep));
        RefreshSpinButtons(SpinBehaviour.SpinAxis.Y);
    }

    void SetSpinAxis(SpinBehaviour.SpinAxis axis)
    {
        if (_spinBehaviour != null) _spinBehaviour.axis = axis;
        RefreshSpinButtons(axis);
    }

    void RefreshSpinButtons(SpinBehaviour.SpinAxis active)
    {
        xAxisButton.SetSelected(active == SpinBehaviour.SpinAxis.X);
        yAxisButton.SetSelected(active == SpinBehaviour.SpinAxis.Y);
        zAxisButton.SetSelected(active == SpinBehaviour.SpinAxis.Z);
    }

    void AdjustSpinSpeed(float delta) => internal_AdjustSpinSpeed(delta);

    internal void internal_AdjustSpinSpeed(float delta)
    {
        if (_spinBehaviour == null) return;
        _spinBehaviour.speed = Mathf.Clamp(_spinBehaviour.speed + delta, SpeedMin, SpeedMax);
        decreaseSpeedButton.interactable = _spinBehaviour.speed > SpeedMin;
        increaseSpeedButton.interactable = _spinBehaviour.speed < SpeedMax;
    }

    // ── Scale ─────────────────────────────────────────────────────────────────

    void WireScaleButtons()
    {
        pulseButton.GetComponent<Button>().onClick.AddListener(()      => SetScaleMode(ScaleBehaviour.ScaleMode.Pulse));
        growOnlyButton.GetComponent<Button>().onClick.AddListener(()   => SetScaleMode(ScaleBehaviour.ScaleMode.GrowOnly));
        shrinkOnlyButton.GetComponent<Button>().onClick.AddListener(() => SetScaleMode(ScaleBehaviour.ScaleMode.ShrinkOnly));
        scaleIncreaseSpeedButton.onClick.AddListener(() => AdjustScaleSpeed(ScaleSpeedStep));
        scaleDecreaseSpeedButton.onClick.AddListener(() => AdjustScaleSpeed(-ScaleSpeedStep));
        RefreshScaleButtons(ScaleBehaviour.ScaleMode.Pulse);
    }

    void SetScaleMode(ScaleBehaviour.ScaleMode mode)
    {
        if (_scaleBehaviour != null) _scaleBehaviour.mode = mode;
        RefreshScaleButtons(mode);
    }

    void RefreshScaleButtons(ScaleBehaviour.ScaleMode active)
    {
        pulseButton.SetSelected(active      == ScaleBehaviour.ScaleMode.Pulse);
        growOnlyButton.SetSelected(active   == ScaleBehaviour.ScaleMode.GrowOnly);
        shrinkOnlyButton.SetSelected(active == ScaleBehaviour.ScaleMode.ShrinkOnly);
    }

    void AdjustScaleSpeed(float delta) => internal_AdjustScaleSpeed(delta);

    internal void internal_AdjustScaleSpeed(float delta)
    {
        if (_scaleBehaviour == null) return;
        _scaleBehaviour.speed = Mathf.Clamp(_scaleBehaviour.speed + delta, ScaleSpeedMin, ScaleSpeedMax);
        scaleDecreaseSpeedButton.interactable = _scaleBehaviour.speed > ScaleSpeedMin;
        scaleIncreaseSpeedButton.interactable = _scaleBehaviour.speed < ScaleSpeedMax;
    }

    // ── Bounce ────────────────────────────────────────────────────────────────

    void WireBounceSliders()
    {
        bounceSpeedSlider.minValue  = BounceSpeedMin;
        bounceSpeedSlider.maxValue  = BounceSpeedMax;
        bounceSpeedSlider.value     = 2f;
        bounceHeightSlider.minValue = BounceHeightMin;
        bounceHeightSlider.maxValue = BounceHeightMax;
        bounceHeightSlider.value    = 0.1f;

        bounceSpeedSlider.onValueChanged.AddListener(v  => { if (_bounceBehaviour != null) _bounceBehaviour.speed  = v; });
        bounceHeightSlider.onValueChanged.AddListener(v => { if (_bounceBehaviour != null) _bounceBehaviour.height = v; });
    }

    internal void internal_SetSpinBehaviour(SpinBehaviour b)   => _spinBehaviour  = b;
    internal void internal_SetScaleBehaviour(ScaleBehaviour b) => _scaleBehaviour = b;

    // ── Navigation ────────────────────────────────────────────────────────────

    void GoBack()
    {
        ShapeModuleCache.ResetBehaviour();
        SceneManager.LoadScene("BehaviorSelectScene");
    }
}
