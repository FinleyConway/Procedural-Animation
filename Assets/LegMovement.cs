using UnityEngine;

public class LegMovement : MonoBehaviour
{
    [Header("Raycast Variables")]
    [SerializeField] private Transform raycastOrigin = default;
    [SerializeField] private float groundCheckDistance = 10;
    [SerializeField] private LayerMask groundLayer = default;

    [Header("The Objects Parts")]
    [SerializeField] private LegMovement otherTarget = default;
    [SerializeField] private Transform body = default;

    [Header("Leg Settings")]
    [SerializeField] private float targetDistance;
    [SerializeField] private float targetHeight;
    [SerializeField] private float targetSpeed;

    public Vector3 CurrentTargetPosition { get; private set; }
    private Vector3 oldTargetPosition;
    private Vector3 newTargetPosition;

    public Vector3 CurrentNormal { get; private set; }
    private Vector3 oldNormal;
    private Vector3 newNormal;

    private Vector3 raycastPosition;
    private Vector3 raycastNormal;

    private float currentDistance;
    private float moveTime;

    private void Awake()
    {
        moveTime = 1;

        CurrentTargetPosition = transform.position;
        oldTargetPosition = CurrentTargetPosition;
        newTargetPosition = CurrentTargetPosition;

        CurrentNormal = transform.up;
        oldNormal = CurrentNormal;
        newNormal = CurrentNormal;
    }

    private void Update()
    {
        // update current and normal positon
        transform.position = CurrentTargetPosition;
        transform.up = CurrentNormal;

        GetTargetNewPosition();
        MoveTargetPosition();
    }

    // finds the next target posiiton
    private void GetTargetNewPosition()
    {
        RaycastHit hit;

        // sends a raycast on surface level to find target normal and position 
        if (Physics.Raycast(raycastOrigin.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
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
            nextPosition.y += Mathf.Sin(moveTime * Mathf.PI) * targetHeight; // might change later with animation curve

            // update current postiion
            CurrentTargetPosition = nextPosition;

            // lerp the current normal to the new normal
            CurrentNormal = Vector3.Lerp(oldNormal, newNormal, moveTime);
            moveTime += Time.deltaTime * targetSpeed; // might change later with animation curve
        }
        else
        {
            oldTargetPosition = newTargetPosition;
            oldNormal = newNormal;
        }
    }

    // function to check if the current leg is moving
    public bool IsLegMoving => moveTime < 1;

    // debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(raycastPosition, 0.25f);
        Gizmos.DrawRay(raycastOrigin.position, Vector3.down * groundCheckDistance);
        if (currentDistance < targetDistance)
            Gizmos.DrawLine(transform.position, raycastPosition);
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, raycastPosition);
        }
    }
}