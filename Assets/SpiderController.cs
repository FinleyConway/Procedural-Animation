using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Awake()
    {
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);

        transform.position += transform.TransformDirection(input) * Time.deltaTime;
    }
}
