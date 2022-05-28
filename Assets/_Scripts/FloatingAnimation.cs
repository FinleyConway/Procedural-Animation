using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinleyConway
{
    public class FloatingAnimation : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _height;

        [Header("Optional - Random-ness")]
        [SerializeField] private bool _isRandomSpeed, _isRandomHeight;
        [SerializeField] private float _minSpeed, _maxSpeed, _minHeight, _maxHeight;

        private Vector3 _startingPostion;

        private void Awake()
        {
            _startingPostion = transform.position;

            if (_isRandomSpeed)
            {
                _speed = Random.Range(_minSpeed, _maxSpeed);
            }

            if (_isRandomHeight)
            {
                _height = Random.Range(_minHeight, _maxHeight);
            }
        }

        private void Update()
        {
            transform.position = new Vector3(transform.position.x, _startingPostion.y + Mathf.Sin(_speed * Time.time) * _height, transform.position.z);
        }
    }
}
