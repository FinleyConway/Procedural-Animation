using UnityEngine;

public class IKTargetSolver : MonoBehaviour
{
    [Header("Raycast Variables")]
    [SerializeField] private Transform raycastOrigin = default;
    [SerializeField] private float groundCheckDistance = 10;
    [SerializeField] private LayerMask groundLayer = default;

    [Header("The Objects Parts")]
    [SerializeField] private IKTargetSolver otherTarget = default;
    [SerializeField] private Transform body = default;

    [Header("Leg Settings")]
    [SerializeField] private float targetDistance;
    [SerializeField] private float targetHeight;
    [SerializeField] private float targetSpeed;

    private Vector3 currentTargetPosition;
    private Vector3 oldTargetPosition;
    private Vector3 newTargetPosition;

    private Vector3 currentNormal;
    private Vector3 oldNormal;
    private Vector3 newNormal;

    private Vector3 raycastPosition;
    private Vector3 raycastNormal;

    private float currentDistance;
    private float moveTime;

    private void Awake()
    {
        moveTime = 1;

        currentTargetPosition = transform.position;
        oldTargetPosition = currentTargetPosition;
        newTargetPosition = currentTargetPosition;

        currentNormal = transform.up;
        oldNormal = currentNormal;
        newNormal = currentNormal;
    }

    private void Update()
    {
        // update current and normal positon
        transform.position = currentTargetPosition;
        transform.up = currentNormal;

        GetTargetNewPosition();
        MoveTargetPosition();
    }

    // finds the next target posiiton
    private void GetTargetNewPosition()
    {
        RaycastHit hit;

        // sends a raycast on surface level to find target position 
        if (Physics.Raycast(raycastOrigin.position, body.up.normalized * -1, out hit, groundCheckDistance, groundLayer))
        {
            // get position and rotation of the surface of the ground
            raycastPosition = hit.point;
            raycastNormal = hit.normal;
        }

        // get the distance between the target position and the raycast position
        currentDistance = Vector3.Distance(raycastPosition, transform.position);

        // move leg if distance is too big
        if (currentDistance > targetDistance && moveTime >= 1 && !otherTarget.IsLegMoving)
        {
            moveTime = 0;
            newTargetPosition = raycastPosition;
            newNormal = raycastNormal;
        }
    }

    private void MoveTargetPosition()
    {
        if (moveTime < 1)
        {
            // lerp current postiion to new position
            Vector3 nextPosition = Vector3.Lerp(oldTargetPosition, newTargetPosition, moveTime);
            // curve the y axis for a step movtion
            nextPosition.y += Mathf.Sin(moveTime * Mathf.PI) * targetHeight;

            // update current postiion
            currentTargetPosition = nextPosition;

            // lerp the current normal to the new normal
            currentNormal = Vector3.Lerp(oldNormal, newNormal, moveTime);

            moveTime += Time.deltaTime * targetSpeed;
        }
        else
        {
            oldTargetPosition = newTargetPosition;
            oldNormal = newNormal;
        }
    }

    public bool IsLegMoving => moveTime < 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(raycastPosition, 0.25f);
        if (currentDistance < targetDistance)
            Gizmos.DrawLine(transform.position, raycastPosition);
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, raycastPosition);
        }
    }
}