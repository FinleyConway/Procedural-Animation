using UnityEngine;
using UnityEngine.AI;

namespace FinleyConway
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private GameObject _player = default;

        private NavMeshAgent _ai;

        private void Awake()
        {
            _ai = GetComponent<NavMeshAgent>();
            _ai.updateUpAxis = false;
        }

        private void Update()
        {
            Vector3 newDest = new Vector3(_player.transform.position.x, 0, _player.transform.position.z);

            _ai.SetDestination(newDest);
        }
    }
}