using UnityEngine;

public class Platform_OneWayAbove : MonoBehaviour
{
    public LayerMask playerLayer;
    public Color rayColor = Color.red;
    public float raycastOffsetY = 0.2f;

    private Collider2D platformCollider;
    private Collider2D playerCollider;

    private PlayerControls _playerControls;
    private bool ignoreCollision = false;

    public float collisionDisableDuration = 0.25f; // Change the duration as needed
    private float collisionDisableTime = 0.0f;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        // Get the extents of the platform collider
        Vector2 extents = platformCollider.bounds.extents;

        // Calculate the raycast distance based on the maximum dimension of the collider (x or y)
        float raycastDistance = Mathf.Max(extents.x, extents.y) + 0.1f;

        // Find the leftmost and rightmost points of the platform
        Vector2 leftPoint = (Vector2)transform.position - new Vector2(extents.x, 0f);
        Vector2 rightPoint = (Vector2)transform.position + new Vector2(extents.x, 0f);

        // Adjust the raycast origins to be slightly above the platform
        Vector2 raycastOriginLeft = leftPoint + Vector2.up * raycastOffsetY;
        Vector2 raycastOriginRight = rightPoint + Vector2.up * raycastOffsetY;

        // Calculate the center of the platform
        Vector2 platformCenter = (leftPoint + rightPoint) * 0.5f;

        // Calculate the direction from each side to the center of the platform
        Vector2 directionLeft = (platformCenter - leftPoint).normalized;
        Vector2 directionRight = (platformCenter - rightPoint).normalized;

        // Cast rays from the left and right sides towards the center of the platform
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, directionLeft, raycastDistance, playerLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, directionRight, raycastDistance, playerLayer);

        // Check if any of the rays hit the player
        bool playerAbovePlatform = hitLeft.collider != null || hitRight.collider != null;


        if (playerAbovePlatform)
        {
            if (_playerControls.Player.Down.triggered)
            {
                Debug.Log("Collision disabled");
                ignoreCollision = true;
                collisionDisableTime = Time.time + collisionDisableDuration;
            }
        }

        // Enable collision if the disable time has passed
        if (Time.time >= collisionDisableTime)
        {
            ignoreCollision = false;
        }

        // Disable the platform's collision with the player if they are below it or if the button is pressed
        Physics2D.IgnoreCollision(platformCollider, playerCollider, ignoreCollision);

        // Draw the raycast hits as lines in the Scene view
        Debug.DrawRay(raycastOriginLeft, directionLeft * raycastDistance, rayColor);
        Debug.DrawRay(raycastOriginRight, directionRight * raycastDistance, rayColor);

    }
}
