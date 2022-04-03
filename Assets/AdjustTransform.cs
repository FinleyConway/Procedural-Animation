using UnityEngine;

public class AdjustTransform : MonoBehaviour
{
    [SerializeField] private float smoothBodyMovementSpeed = 2.5f;
    [SerializeField] private LegMovement[] legMovement;

    private float defaultYPosition;

    private void Awake()
    {
        defaultYPosition = transform.position.y;
    }

    private void Update()
    {
        AjustBodyPosition();
    }

    // Adjust body position based on the average leg position
    private void AjustBodyPosition()
    {
        Vector3 averagePos = Vector3.zero;

        // gets the average leg position and normal
        foreach (LegMovement legs in legMovement)
        {
            averagePos += legs.CurrentTargetPosition;
        }
        averagePos /= legMovement.Length;

        // interplate movement to give a smooth realstic feeling
        Vector3 offset = new Vector3(transform.position.x, averagePos.y + defaultYPosition, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, offset, smoothBodyMovementSpeed * Time.deltaTime);
    }
}
