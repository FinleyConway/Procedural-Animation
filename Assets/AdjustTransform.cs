using UnityEngine;

public class AdjustTransform : MonoBehaviour
{
    [SerializeField] private float smoothBodyMovementSpeed = 2.5f;
    [SerializeField] private LegMovement[] legMovement;

    [SerializeField] private Transform body;

    private Vector3 currentPosition;
    private Vector3 averageLegPos;
    private Vector3 yRotation;
    private Vector3 zRotation;
    private Vector3 xRotation;

    private float distanceFromFloor;

    private void Update()
    {
        AjustBodyPosition();
        RotateBody();

    }

    // Adjust body position based on the average leg position
    private void AjustBodyPosition()
    {
        // gets the average leg position and normal
        foreach (LegMovement legs in legMovement)
        {
            averageLegPos += legs.CurrentTargetPosition;
            // get the legs normal positon and up direction
            yRotation += legs.transform.up + legs.RaycastNormal;
        }

        // get the current body normal
        RaycastHit hit;
        if (Physics.Raycast(body.position, body.up * -1, out hit, 10))
        {
            yRotation += hit.normal;
            distanceFromFloor = Vector3.Distance(hit.point, transform.position);
        }
        averageLegPos /= legMovement.Length;
        yRotation.Normalize();

        currentPosition = new Vector3(transform.position.x, averageLegPos.y, transform.position.z);
        // interplate movement to give a smooth realstic feeling
        transform.position = Vector3.Lerp(transform.position, currentPosition, smoothBodyMovementSpeed * Time.deltaTime);
    }

    private void RotateBody()
    {
        // get the rotation axis
        xRotation = Vector3.Cross(yRotation, transform.forward);
        zRotation = Vector3.Cross(xRotation, yRotation);
        // rotate body
        Quaternion bodyRotation = Quaternion.LookRotation(zRotation, yRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, bodyRotation, 5 * Time.deltaTime);
    }
}
