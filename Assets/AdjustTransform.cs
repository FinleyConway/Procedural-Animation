using UnityEngine;

public class AdjustTransform : MonoBehaviour
{
    [SerializeField] private float smoothBodyMovementSpeed = 2.5f;
    [SerializeField] private LegMovement[] legMovement;

    private void Update()
    {
        AjustBodyPosition();
    }

    // Adjust body position based on the average leg position
    private void AjustBodyPosition()
    {
        Vector3 currentYPosition = Vector3.zero;
        Vector3 averagePos = Vector3.zero;

        // gets the average leg position and normal
        foreach (LegMovement legs in legMovement)
        {
            averagePos += legs.CurrentTargetPosition;
        }
        averagePos /= legMovement.Length;

        // interplate movement to give a smooth realstic feeling
        currentYPosition = new Vector3(transform.position.x, averagePos.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, currentYPosition, smoothBodyMovementSpeed * Time.deltaTime);
    }
}
