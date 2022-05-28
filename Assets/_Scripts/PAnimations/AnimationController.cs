using UnityEngine;

namespace FinleyConway.Animation
{
    public class AnimationController : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _ground = default;

        [Header("Body Parts")]
        [SerializeField] private LegMovement[] _legs;
        [SerializeField] private Transform _body = default;

        [Header("Body Settings")]
        [SerializeField] private float _smoothBodyMovementSpeed = 1f;
        [SerializeField] private float _smoothBodyRotationSpeed = 2f;
        [SerializeField] private float _bodyOffset = 0.1f;

        public Vector3 CurrentPosition { get; set; }
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
            if (Physics.Raycast(_body.position, _body.up * -1, out hit, _raycastDistance, _ground))
            {
                _yRotation += hit.normal;
            }
            _yRotation.Normalize();

            // lerp movement to give a smooth realistic feeling
            CurrentPosition = averageLegPos + _yRotation.normalized * _bodyOffset;
            //CurrentPosition = new Vector3(transform.position.x, averageLegPos.y + _yRotation.normalized.y * _bodyOffset, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, CurrentPosition, _smoothBodyMovementSpeed * Time.deltaTime);
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

        // gets the normal of the plane and outputs either 1 or -1 depending on the angle of the slope
        // upwards direction is -1
        // downwards direction is 1
        public float InertiaHandler()
        {
            float slope = 0;

            RaycastHit hit;
            if (Physics.Raycast(_body.position, Vector3.down, out hit, _raycastDistance, _ground))
            {
                slope = Vector3.Dot(_body.right, (Vector3.Cross(Vector3.up, hit.normal)));
            }

            return slope;
        }
    }
}