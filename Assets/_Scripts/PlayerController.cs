using UnityEngine;
using FinleyConway.Animation;

namespace FinleyConway
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        private AnimationController _aC;

        private void Awake()
        {
            _aC = GetComponent<AnimationController>();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical") * _moveSpeed.Evaluate(_aC.InertiaHandler()) * Time.deltaTime));

            Vector2 rotateInput = new Vector2(0, Input.GetAxis("Horizontal") * _rotateSpeed);

            transform.Rotate(rotateInput * Time.deltaTime);
        }
    }
}