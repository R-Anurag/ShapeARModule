using UnityEngine;

public class SpinBehaviour : MonoBehaviour
{
    public enum SpinAxis { X, Y, Z }

    public SpinAxis axis = SpinAxis.Y;
    public float speed = 90f;

    void Update()
    {
        Vector3 dir = axis switch
        {
            SpinAxis.X => Vector3.right,
            SpinAxis.Z => Vector3.forward,
            _          => Vector3.up
        };
        transform.Rotate(dir, speed * Time.deltaTime, Space.Self);
    }
}
