using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical, LeftDiagonal, RightDiagonal }

    public MoveDirection direction = MoveDirection.Horizontal;
    public float distance = 0.3f;
    public float speed = 1f;

    private Vector3 _origin;
    private MoveDirection _lastDirection;
    private float _phaseStart;

    void Start()
    {
        _origin        = transform.localPosition;
        _lastDirection = direction;
        _phaseStart    = Time.time;
    }

    void Update()
    {
        if (direction != _lastDirection)
        {
            _origin        = transform.localPosition;
            _lastDirection = direction;
            _phaseStart    = Time.time;
        }

        Vector3 axis = direction switch
        {
            MoveDirection.Vertical      => transform.up,
            MoveDirection.LeftDiagonal  => (-transform.right + transform.up).normalized,
            MoveDirection.RightDiagonal => ( transform.right + transform.up).normalized,
            _                           => transform.right
        };
        float offset = Mathf.Sin((Time.time - _phaseStart) * speed) * distance;
        transform.localPosition = _origin + axis * offset;
    }
}
