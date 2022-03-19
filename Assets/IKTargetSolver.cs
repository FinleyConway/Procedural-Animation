using UnityEngine;

public class IKTargetSolver : MonoBehaviour
{
    [SerializeField] private Transform targetPoint = default;
    [SerializeField] private float groundCheckDistance = 10;
    [SerializeField] private LayerMask groundLayer = default;
    [SerializeField] private IKTargetSolver otherTarget = default;

    [SerializeField] private float stepDistance;
    [SerializeField] private float stepHeight;
    [SerializeField] private float limbSpeed;
    private Vector3 currentPosition;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private float moveTime;

    private void Awake()
    {
        currentPosition = transform.position;
        oldPosition = currentPosition;
        newPosition = currentPosition;
        moveTime = 1;
    }

    private void Update()
    {
        // fixes legs to the floor and updates when the current position updates
        transform.position = currentPosition;

        GetNextTargetPosition();
        MoveTarget();
    }

    // uses a raycast to get the next location 
    private void GetNextTargetPosition()
    {
        RaycastHit hit;
        // Get position from raycast point.
        if (Physics.Raycast(targetPoint.position, Vector3.down, out hit, 10, groundLayer))
        {
            #if UNITY_EDITOR
            Debug.DrawRay(targetPoint.position, Vector3.down * groundCheckDistance);
            #endif

            // Gets the distance between the current location and the raycast pos and sets the new pos depning on the max step dist.
            if (Vector3.Distance(transform.position, hit.point) > stepDistance && moveTime >= 1 && !otherTarget.IsTargetMoving)
            {
                #if UNITY_EDITOR
                DebugExtension.DebugPoint(hit.point, 1);
                Debug.DrawLine(transform.position, hit.point);
                #endif
                moveTime = 0;

                // New position.
                newPosition = hit.point;
            }
        }
    }

    // lerps target position to the new position
    private void MoveTarget()
    {
        if (moveTime < 1)
        {
            // lerp position to the new position
            Vector3 nextPosition = Vector3.Lerp(oldPosition, newPosition, moveTime);
            // give the y axis an arch movement
            nextPosition.y += Mathf.Sin(moveTime * Mathf.PI) * stepHeight;

            // update the current position
            currentPosition = nextPosition;

            moveTime += Time.deltaTime * limbSpeed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    // check if the current leg is moving
    public bool IsTargetMoving => moveTime < 1;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(newPosition, 0.5f);
    }
}