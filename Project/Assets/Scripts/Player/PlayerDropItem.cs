
public class PlayerDropItem
{
    // [SerializeField] private GameObject item;
    // [SerializeField] private Transform dropPoint;

    // [Header("Drop Item Settings")]
    // [SerializeField] private Transform dropPointOffset;
    // [SerializeField] private LayerMask collisionLayer;

    // private float _forwardForce = 2f;
    // private float _upwardForce = 1f;

    // private void OnEnable()
    // {
    //     if (InputManager.Instance != null)
    //         InputManager.Instance.OnDropItem += HandleDropItem;

    //     item.transform.position = dropPoint.position;
    // }

    // private void OnDisable()
    // {
    //     if (InputManager.Instance != null)
    //         InputManager.Instance.OnDropItem -= HandleDropItem;
    // }

    // private void HandleDropItem()
    // {
    //     if (item != null && dropPoint != null)
    //     {
    //         Rigidbody itemRigidbody = item.GetComponent<Rigidbody>();

    //         itemRigidbody.isKinematic = false;
    //         itemRigidbody.AddForce(Vector3.up * _upwardForce + Vector3.forward * _forwardForce, ForceMode.Impulse);



    //         // Vector3 newDropPoint = FindClearDropPosition(dropItemPoint, dropItemPoint.position);

    //         // // Instantiate the item at the valid drop position
    //         // GameObject itemObject = Instantiate(
    //         //     EquippedItem.GetItem().prefab,
    //         //     newDropPoint,
    //         //     Quaternion.identity
    //         // );

    //         // EquippedItem.RemoveItem();

    //         // // Ignore collision between the player and the dropped item
    //         // Collider itemCollider = itemObject.GetComponent<Collider>();
    //         // if (itemCollider != null)
    //         // {
    //         //     Physics.IgnoreCollision(GetComponent<CharacterController>(), itemCollider, true);
    //         // }
    //     }
    // }

    // private Vector3 FindClearDropPosition(Transform dropPoint, Vector3 initialPosition)
    // {
    //     const int maxAttempts = 50; // Limit the number of adjustments
    //     float offsetIncrement = 0.05f; // Distance to move the item on each attempt
    //     Vector3 currentPosition = initialPosition;

    //     for (int i = 0; i < maxAttempts; i++)
    //     {
    //         // If still not clear, try moving it backward slightly (away from obstacles)
    //         if (!IsPositionClear(currentPosition))
    //         {
    //             currentPosition -= dropPoint.forward * offsetIncrement;
    //         }

    //         // Return the cleared position found
    //         else if (IsPositionClear(currentPosition))
    //         {
    //             return currentPosition;
    //         }
    //     }

    //     // If no clear position is found, return the initial position
    //     return initialPosition;
    // }

    // private bool IsPositionClear(Vector3 position)
    // {
    //     // Check for overlapping colliders at the position
    //     Collider[] colliders = Physics.OverlapSphere(position, 0.2f, collisionLayer);
    //     return colliders.Length == 0;
    // }
}
