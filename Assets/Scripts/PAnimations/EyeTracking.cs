using UnityEngine;

namespace FinleyConway.Animation
{
    public class EyeTracking : MonoBehaviour
    {
        [SerializeField] private Transform _player = default;

        [SerializeField] private float _maxAngle = 55;
        [SerializeField] private float lookSpeed = 2;
        private Quaternion _defaultRotation;
        private Quaternion _targetRotation;

        private void Awake()
        {
            _defaultRotation = transform.rotation;
        }

        private void Update()
        {
            Quaternion lookAt = Quaternion.LookRotation(_player.position - transform.position);
            if (Quaternion.Angle(lookAt, _defaultRotation) <= _maxAngle)
            {
                _targetRotation = lookAt;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * lookSpeed);
        }
    }
}
