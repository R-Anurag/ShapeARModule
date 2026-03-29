using UnityEngine;

public class BounceBehaviour : MonoBehaviour
{
    public float height = 0.1f;
    public float speed = 2f;

    private Vector3 _origin;

    void Start() => _origin = transform.localPosition;

    void Update()
    {
        float offset = Mathf.Abs(Mathf.Sin(Time.time * speed)) * height;
        transform.localPosition = _origin + transform.parent.up * offset;
    }
}
