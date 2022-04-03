using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private void Awake()
    {
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime));

        Vector2 rotateInput = new Vector2(0, Input.GetAxis("Horizontal") * rotateSpeed);

        transform.Rotate(rotateInput * Time.deltaTime);
    }
}
