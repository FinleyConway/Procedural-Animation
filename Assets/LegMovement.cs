using UnityEngine;

public class LegMovement : MonoBehaviour
{
    [Header("Raycast Variables")]
    [SerializeField] private float groundCheckDistance = 10;
    [SerializeField] private LayerMask groundLayer = default;

    [Header("The Objects Parts")]
    [SerializeField] private LegMovement otherTarget = default;
    [SerializeField] private Transform body = default;
    //[SerializeField] private Transform endTip = default;

    [Header("Leg Settings")]
    [SerializeField] private float targetDistance;
    [SerializeField] private float targetHeight;
    [SerializeField] private float targetSpeed;

    public Vector3 CurrentTargetPosition { get; private set; }
    private Vector3 oldTargetPosition;
    private Vector3 newTargetPosition;
    private Vector3 targetOffset;
    private Vector3 currentNormal;
    private Vector3 oldNormal;
    private Vector3 newNormal;
    private Vector3 raycastPosition;
    private Vector3 raycastNormal;

    private float currentDistance;
    private float moveTime;

    private void Awake()
    {
        //transform.position = endTip.position;

        targetOffset.x = transform.localPosition.x;
        targetOffset.z = transform.localPosition.z;

        CurrentTargetPosition = transform.position;
        oldTargetPosition = CurrentTargetPosition;
        newTargetPosition = CurrentTargetPosition;

        currentNormal = transform.up;
        oldNormal = currentNormal;
        newNormal = currentNormal;

        moveTime = 1;
    }

    private void Update()
    {
        // update current and normal positon
        transform.position = CurrentTargetPosition;
        transform.up = currentNormal;

        GetTargetNewPosition();
        MoveTargetPosition();
    }

    // finds the next target posiiton
    private void GetTargetNewPosition()
    {
        // sends a raycast on surface level to find target normal and position 
        RaycastHit hit;
        Ray ray = new Ray(body.position - -body.forward.normalized * targetOffset.z + (body.right * targetOffset.x), body.up.normalized * -1);
        if (Physics.Raycast(ray, out hit, groundCheckDistance, groundLayer))
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

    // interplate current target position to new target position
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
            currentNormal = Vector3.Lerp(oldNormal, newNormal, moveTime);
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
        Gizmos.DrawSphere(newTargetPosition, 0.125f);
        Gizmos.DrawRay(body.position - -body.forward.normalized * targetOffset.z + (body.right * targetOffset.x), body.up.normalized * -1 * groundCheckDistance);
        if (currentDistance < targetDistance)
            Gizmos.DrawLine(transform.position, raycastPosition);
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, raycastPosition);
        }
    }
}