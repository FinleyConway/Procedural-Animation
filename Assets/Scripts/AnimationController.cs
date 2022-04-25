using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField] private LegMovement[] _legs = default;
    [SerializeField] private Transform _body = default;

    [Header("Body Settings")]
    [SerializeField] private float _smoothBodyMovementSpeed = 1f;
    [SerializeField] private float _smoothBodyRotationSpeed = 2f;
    [SerializeField] private float _bodyOffset = 0.1f;

    private Vector3 _currentPosition;
    private Vector3 _yRotation;
    private Vector3 _zRotation;
    private Vector3 _xRotation;

    private void Update()
    {
        AdjustBodyPosition();
        AdjustBodyRotation();
    }

    // Adjust body position based on the average leg position
    private void AdjustBodyPosition()
    {
        Vector3 averageLegPos = Vector3.zero;
        _yRotation = Vector3.zero;

        // gets the average leg position and normal
        foreach (LegMovement legs in _legs)
        {
            averageLegPos += legs.CurrentPosition;
            // get the legs normal positon and up direction
            _yRotation += legs.transform.up + legs.RaycastNormal;
        }
        averageLegPos /= _legs.Length;

        // get the current body normal
        RaycastHit hit;
        if (Physics.Raycast(_body.position, _body.up * -1, out hit, 30))
        {
            _yRotation += hit.normal;
        }
        _yRotation.Normalize();

        // lerp movement to give a smooth realistic feeling
        _currentPosition = averageLegPos + _yRotation.normalized * _bodyOffset;
        transform.position = Vector3.MoveTowards(transform.position, _currentPosition, _smoothBodyMovementSpeed * Time.deltaTime);
    }

    private void AdjustBodyRotation()
    {
        // get the rotation axis
        _xRotation = Vector3.Cross(_yRotation, transform.forward);
        _zRotation = Vector3.Cross(_xRotation, _yRotation);
        // rotate body
        Quaternion bodyRotation = Quaternion.LookRotation(_zRotation, _yRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, bodyRotation, _smoothBodyRotationSpeed * Time.deltaTime);
    }
}
