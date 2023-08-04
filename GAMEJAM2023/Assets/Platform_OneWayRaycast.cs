using UnityEngine;

public class Platform_OneWayRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Color aboveRayColor = Color.red;
    [SerializeField] private Color belowRayColor = Color.yellow;
    [SerializeField] private float aboveRaycastOffsetY = 0.2f;
    [SerializeField] private float belowRaycastOffsetY = -0.1f;

    private Collider2D _platformCollider;
    private Collider2D _playerCollider;
    private PlayerControls _playerControls;

    [SerializeField] private float _collisionDisableDuration = 0.25f; // Change the duration as needed
    private float _collisionDisableTime = 0.0f;
    private bool _ignoreCollision = false;

    private Vector2 extents;

    private Vector2 aboveRaycastOriginLeft;
    private Vector2 aboveRaycastOriginRight;
    private Vector2 belowRaycastOriginLeft;
    private Vector2 belowRaycastOriginRight;
    private Vector2 directionLeft;
    private Vector2 directionRight;

    private float raycastDistance;

    private bool _playerAbovePlatform;
    private bool _playerBelowPlatform;

    private void Awake()
    {
        _platformCollider = GetComponent<Collider2D>();
        _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        extents = _platformCollider.bounds.extents;
        raycastDistance = Mathf.Max(extents.x, extents.y) + 0.1f;
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
        // Find the leftmost and rightmost points of the platform
        Vector2 leftPoint = (Vector2)transform.position - new Vector2(extents.x, 0f);
        Vector2 rightPoint = (Vector2)transform.position + new Vector2(extents.x, 0f);

        // Adjust the raycast origins for above and below detection
        aboveRaycastOriginLeft = leftPoint + Vector2.up * aboveRaycastOffsetY;
        aboveRaycastOriginRight = rightPoint + Vector2.up * aboveRaycastOffsetY;
        belowRaycastOriginLeft = leftPoint + Vector2.up * belowRaycastOffsetY;
        belowRaycastOriginRight = rightPoint + Vector2.up * belowRaycastOffsetY;

        // Calculate the center of the platform
        Vector2 platformCenter = (leftPoint + rightPoint) * 0.5f;

        // Calculate the direction from each side to the center of the platform
        directionLeft = (platformCenter - leftPoint).normalized;
        directionRight = (platformCenter - rightPoint).normalized;

        // Check for collisions above the platform using raycasts
        RaycastHit2D hitLeftAbove = Physics2D.Raycast(aboveRaycastOriginLeft, directionLeft, raycastDistance, playerLayer);
        RaycastHit2D hitRightAbove = Physics2D.Raycast(aboveRaycastOriginRight, directionRight, raycastDistance, playerLayer);

        _playerAbovePlatform = hitLeftAbove.collider != null || hitRightAbove.collider != null;

        // Check for collisions below the platform using raycasts
        RaycastHit2D hitLeftBelow = Physics2D.Raycast(belowRaycastOriginLeft, directionLeft, raycastDistance, playerLayer);
        RaycastHit2D hitRightBelow = Physics2D.Raycast(belowRaycastOriginRight, directionRight, raycastDistance, playerLayer);

        _playerBelowPlatform = hitLeftBelow.collider != null || hitRightBelow.collider != null;

        bool disableCollisions = (_playerAbovePlatform && _playerControls.Player.Down.triggered) || _playerBelowPlatform;

        if (_playerAbovePlatform && _playerControls.Player.Down.triggered)
        {
            Debug.Log("Collision disabled");
            _ignoreCollision = true;
            _collisionDisableTime = Time.time + _collisionDisableDuration;
        }

        if (Time.time >= _collisionDisableTime)
        {
            _ignoreCollision = false;
        }

        Physics2D.IgnoreCollision(_platformCollider, _playerCollider, disableCollisions);

        // Optionally, you can draw the rays for both cases
        Debug.DrawRay(aboveRaycastOriginLeft, directionLeft * raycastDistance, aboveRayColor);
        Debug.DrawRay(aboveRaycastOriginRight, directionRight * raycastDistance, aboveRayColor);
        Debug.DrawRay(belowRaycastOriginLeft, directionLeft * raycastDistance, belowRayColor);
        Debug.DrawRay(belowRaycastOriginRight, directionRight * raycastDistance, belowRayColor);
    }
}
