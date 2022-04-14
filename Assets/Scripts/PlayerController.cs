using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime));

        Vector2 rotateInput = new Vector2(0, Input.GetAxis("Horizontal") * _rotateSpeed);

        transform.Rotate(rotateInput * Time.deltaTime);
    }
}
