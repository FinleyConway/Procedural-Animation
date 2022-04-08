using UnityEngine;

public class AdjustTransform : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField] private LegMovement[] legMovement = default;
    [SerializeField] private Transform body = default;

    [Header("Body Settings")]
    [SerializeField] private float smoothBodyMovementSpeed = 2.5f;
    [SerializeField] private float smoothBodyRotationSpeed = 5;
    [SerializeField] private float bodyOffset = 0.1f;

    private Vector3 currentPosition;
    private Vector3 yRotation;
    private Vector3 zRotation;
    private Vector3 xRotation;

    private void Update()
    {
        AjustBodyPosition();
        RotateBody();
    }

    // Adjust body position based on the average leg position
    private void AjustBodyPosition()
    {
        Vector3 averageLegPos = Vector3.zero;
        yRotation = Vector3.zero;

        // gets the average leg position and normal
        foreach (LegMovement legs in legMovement)
        {
            averageLegPos += legs.CurrentTargetPosition;
            // get the legs normal positon and up direction
            yRotation += legs.transform.up + legs.RaycastNormal;
        }
        averageLegPos /= legMovement.Length;

        // lerp movement to give a smooth realstic feeling
        currentPosition = averageLegPos + yRotation.normalized * bodyOffset;
        transform.position = Vector3.Lerp(transform.position, currentPosition, smoothBodyMovementSpeed * Time.deltaTime);
    }

    private void RotateBody()
    {
        // get the current body normal
        RaycastHit hit;
        if (Physics.Raycast(body.position, body.up * -1, out hit, 10))
        {
            yRotation += hit.normal;
        }
        yRotation.Normalize();

        // get the rotation axis
        xRotation = Vector3.Cross(yRotation, transform.forward);
        zRotation = Vector3.Cross(xRotation, yRotation);
        // rotate body
        Quaternion bodyRotation = Quaternion.LookRotation(zRotation, yRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, bodyRotation, smoothBodyRotationSpeed * Time.deltaTime);
    }
}
