using UnityEngine;

public class ScaleBehaviour : MonoBehaviour
{
    public enum ScaleMode { Pulse, GrowOnly, ShrinkOnly }

    public ScaleMode mode = ScaleMode.Pulse;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float speed = 2f;

    private Vector3 _originalScale;

    void Start() => _originalScale = transform.localScale;

    void Update()
    {
        float t = mode switch
        {
            ScaleMode.GrowOnly   => Mathf.Abs(Mathf.Sin(Time.time * speed)),
            ScaleMode.ShrinkOnly => 1f - Mathf.Abs(Mathf.Sin(Time.time * speed)),
            _                    => (Mathf.Sin(Time.time * speed) + 1f) * 0.5f
        };
        transform.localScale = _originalScale * Mathf.Lerp(minScale, maxScale, t);
    }
}
