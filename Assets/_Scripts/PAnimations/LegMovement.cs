using System.Linq;
using UnityEngine;
using FinleyConway.Utilities;

namespace FinleyConway.Animation
{
    public class LegMovement : MonoBehaviour
    {
        [Header("Raycast Variables")]
        [SerializeField] private float _groundCheckDownDistance = 5;
        [SerializeField] private LayerMask _groundLayer = default;

        [Header("The Objects Parts")]
        [SerializeField] private LegMovement[] _otherTargets = default;
        [SerializeField] private Transform _body = default;

        [Header("Leg Settings")]
        [SerializeField] private float _targetDistance;
        [SerializeField] private float _targetLength;
        [SerializeField] private AnimationCurve _targetHeight;
        [SerializeField] private AnimationCurve _targetSpeed;
        [SerializeField] private float _moveIterations = 1;

        [Header("Camera Shake Settings")]
        [SerializeField] private bool _canCameraShake = false;
        [SerializeField] private float _cameraShakeDuration;
        [SerializeField] private float _cameraShakeIntensity;

        public Vector3 RaycastNormal { get; private set; }
        private Vector3 _currentNormal;
        private Vector3 _newNormal;
        private Vector3 _oldNormal;

        public Vector3 CurrentPosition { get; private set; }
        private Vector3 _raycastPosition;
        private Vector3 _newTargetPosition;
        private Vector3 _oldTargetPosition;

        private Vector3 _targetOffset;
        private float _moveTime;

        private void Awake()
        {
            _targetOffset = transform.position - _body.position;
            _targetOffset = new Vector3(_targetOffset.x, 0, _targetOffset.z);

            CurrentPosition = transform.position;
            _oldTargetPosition = CurrentPosition;
            _newTargetPosition = CurrentPosition;

            _currentNormal = transform.up;
            _oldNormal = _currentNormal;
            _newNormal = _currentNormal;

            _moveTime = _moveIterations;
        }

        private void Update()
        {
            // update current and normal positon
            transform.position = CurrentPosition;
            transform.up = _currentNormal;

            GetTargetNewPosition();
            MoveTargetPosition();
        }

        // finds the next target posiiton
        private void GetTargetNewPosition()
        {
            // sends a raycast on surface level to find target normal and position 
            RaycastHit hit;
            Ray downRay = new Ray(_body.position - -_body.forward.normalized * _targetOffset.z + (_body.right * _targetOffset.x), _body.up.normalized * -1);

            // get position and rotation of the surface of the ground
            if (Physics.Raycast(downRay, out hit, _groundCheckDownDistance, _groundLayer))
            {
                _raycastPosition = hit.point;
                RaycastNormal = hit.normal;

                // move leg if distance is too big
                if ((GetTargetPointDistance(transform.position) > _targetDistance) && _moveTime >= _moveIterations && CanMove)
                {
                    _moveTime = 0;

                    // gets the direction in which the body is moving in
                    int direction = _body.InverseTransformPoint(hit.point).z > _body.InverseTransformPoint(_newTargetPosition).z ? 1 : -1;

                    _newTargetPosition = _raycastPosition + _body.forward * direction * _targetLength;
                    _newNormal = RaycastNormal;
                }
            }

            // checks the distance between the current position and the raycast hit
            float GetTargetPointDistance(Vector3 position)
            {
                #if UNITY_EDITOR
                Debug.DrawLine(position, _raycastPosition, Color.red);
                #endif
                return Vector3.Distance(position, _raycastPosition);
            }
        }

        // interplate current target position to new target position
        private void MoveTargetPosition()
        {
            if (_moveTime < _moveIterations)
            {
                // lerp current postiion to new position
                Vector3 nextPosition = Vector3.MoveTowards(_oldTargetPosition, _newTargetPosition, _moveTime);
                // curve the y axis for a step movtion
                nextPosition.y += _targetHeight.Evaluate(_moveTime);
                
                // update current postiion
                CurrentPosition = nextPosition;

                // lerp the current normal to the new normal
                _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _moveTime);

                _moveTime += Time.deltaTime * _targetSpeed.Evaluate(_moveTime);

                // shake camera when land foot
                if (!(_moveTime < _moveIterations) && _canCameraShake)
                {
                    CameraShake.Instance.ShakeCamera(_cameraShakeIntensity, _cameraShakeDuration);
                }
            }
            else
            {
                _oldTargetPosition = _newTargetPosition;
                _oldNormal = _newNormal;
            }
        }

        // checks if opposite current leg is moving
        public bool CanMove => _otherTargets.All(x => x.IsMoving);

        public bool IsMoving => _moveTime >= _moveIterations;

        // debugging
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_newTargetPosition, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_body.position - -_body.forward.normalized * _targetOffset.z + (_body.right * _targetOffset.x), _body.up.normalized * -1 * _groundCheckDownDistance);
        }
    }
}