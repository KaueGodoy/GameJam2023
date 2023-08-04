using UnityEngine;

public class Platform_OneWayAboveRaycast : MonoBehaviour
{
    private PlayerControls _playerControls;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Color rayColor = Color.red;
    [SerializeField] private float raycastOffsetY = 0.2f;

    private Collider2D _platformCollider;
    private Collider2D _playerCollider;

    [SerializeField] private float _collisionDisableDuration = 0.25f; // Change the duration as needed
    private float _collisionDisableTime = 0.0f;
    private bool _ignoreCollision = false;

    private Vector2 _extents;

    private Vector2 _raycastOriginLeft;
    private Vector2 _raycastOriginRight;
    private Vector2 _directionLeft;
    private Vector2 _directionRight;

    private float _raycastDistance;

    private bool _playerAbovePlatform;

    private void Awake()
    {
        _platformCollider = GetComponent<Collider2D>();
        _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        _extents = _platformCollider.bounds.extents;
        _raycastDistance = Mathf.Max(_extents.x, _extents.y) + 0.1f;
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
        Vector2 leftPoint = (Vector2)transform.position - new Vector2(_extents.x, 0f);
        Vector2 rightPoint = (Vector2)transform.position + new Vector2(_extents.x, 0f);

        // Adjust the raycast origins to be slightly above the platform
        _raycastOriginLeft = leftPoint + Vector2.up * raycastOffsetY;
        _raycastOriginRight = rightPoint + Vector2.up * raycastOffsetY;

        // Calculate the center of the platform
        Vector2 platformCenter = (leftPoint + rightPoint) * 0.5f;

        // Calculate the direction from each side to the center of the platform
        _directionLeft = (platformCenter - leftPoint).normalized;
        _directionRight = (platformCenter - rightPoint).normalized;

        RaycastHit2D hitLeft = Physics2D.Raycast(_raycastOriginLeft, _directionLeft, _raycastDistance, playerLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(_raycastOriginRight, _directionRight, _raycastDistance, playerLayer);

        _playerAbovePlatform = hitLeft.collider != null || hitRight.collider != null;

        if (_playerAbovePlatform)
        {
            if (_playerControls.Player.Down.triggered)
            {
                Debug.Log("Collision disabled");
                _ignoreCollision = true;
                _collisionDisableTime = Time.time + _collisionDisableDuration;
            }
        }

        if (Time.time >= _collisionDisableTime)
        {
            _ignoreCollision = false;
        }

        Physics2D.IgnoreCollision(_platformCollider, _playerCollider, _ignoreCollision);

        Debug.DrawRay(_raycastOriginLeft, _directionLeft * _raycastDistance, rayColor);
        Debug.DrawRay(_raycastOriginRight, _directionRight * _raycastDistance, rayColor);
    }
}
