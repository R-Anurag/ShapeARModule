using UnityEngine;

public class ScaleBehaviour : MonoBehaviour
{
    public enum ScaleMode { Pulse, GrowOnly, ShrinkOnly }

    public ScaleMode mode = ScaleMode.Pulse;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float speed = 1.5f;

    private Vector3 _originalScale;
    private bool _initialised;
    private float _phaseStart;
    private float _lastSpeed;
    private ScaleMode _lastMode;

    void Update()
    {
        if (!_initialised)
        {
            _originalScale = transform.localScale;
            _phaseStart    = Time.time;
            _lastSpeed     = speed;
            _lastMode      = mode;
            _initialised   = true;
        }

        if (speed != _lastSpeed || mode != _lastMode)
        {
            _phaseStart = Time.time;
            _lastSpeed  = speed;
            _lastMode   = mode;
        }

        float elapsed = (Time.time - _phaseStart) * speed;
        float t = mode switch
        {
            ScaleMode.GrowOnly   => Mathf.Repeat(elapsed, 1f),
            ScaleMode.ShrinkOnly => 1f - Mathf.Repeat(elapsed, 1f),
            _                    => (Mathf.Sin(elapsed) + 1f) * 0.5f
        };
        transform.localScale = _originalScale * Mathf.Lerp(minScale, maxScale, t);
    }
}
