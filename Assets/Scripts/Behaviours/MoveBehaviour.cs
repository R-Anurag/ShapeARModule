using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical, LeftDiagonal, RightDiagonal }

    public MoveDirection direction = MoveDirection.Horizontal;
    public float distance = 0.3f;
    public float speed = 1f;

    private Vector3 _origin;

    static readonly Vector3[] _axes =
    {
        Vector3.right,
        Vector3.up,
        new Vector3(-1, 1, 0).normalized,
        new Vector3( 1, 1, 0).normalized
    };

    void Start() => _origin = transform.localPosition;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * distance;
        transform.localPosition = _origin + _axes[(int)direction] * offset;
    }
}
