using UnityEngine;
using UnityEngine.AI;
using FinleyConway.Animation;

namespace FinleyConway
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private GameObject _player = default;

        [SerializeField] private AnimationCurve _movementSpeed;

        private AnimationController _aC;
        private NavMeshAgent _ai;

        private void Awake()
        {
            _aC = GetComponent<AnimationController>();

            _ai = GetComponent<NavMeshAgent>();
            _ai.updateUpAxis = false;
        }

        private void Update()
        {
            _ai.speed = _movementSpeed.Evaluate(_aC.InertiaHandler());

            Vector3 newDest = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);
            _ai.SetDestination(newDest);
        }
    }
}