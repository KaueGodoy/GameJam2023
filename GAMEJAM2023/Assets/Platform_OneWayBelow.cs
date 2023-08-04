using UnityEngine;

public class Platform_OneWayBelow : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Color _rayColor = Color.yellow;
    [SerializeField] private float _raycastOffsetY = -0.1f;

    private Collider2D _platformCollider;
    private Collider2D _playerCollider;

    private Vector2 _extents;

    private Vector2 _raycastOriginLeft;
    private Vector2 _raycastOriginRight;
    private Vector2 _directionLeft;
    private Vector2 _directionRight;

    private float _raycastDistance;

    private void Awake()
    {
        _platformCollider = GetComponent<Collider2D>();
        _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }
    private void Start()
    {
        _extents = _platformCollider.bounds.extents;
        _raycastDistance = Mathf.Max(_extents.x, _extents.y) + 0.1f;
    }

    private void FixedUpdate()
    {
        // should only use if platform size changes during run time
        // otherwise use cached version
        //Vector2 _extents = _platformCollider.bounds.extents;

        // cant cache if platforms move
        Vector2 leftPoint = (Vector2)transform.position - new Vector2(_extents.x, 0f);
        Vector2 rightPoint = (Vector2)transform.position + new Vector2(_extents.x, 0f);

        // Adjust the raycast origins to be slightly above the platform
        _raycastOriginLeft = leftPoint + Vector2.up * _raycastOffsetY;
        _raycastOriginRight = rightPoint + Vector2.up * _raycastOffsetY;

        // Calculate the center of the platform
        Vector2 platformCenter = (leftPoint + rightPoint) * 0.5f;

        // Calculate the direction from each side to the center of the platform
        _directionLeft = (platformCenter - leftPoint).normalized;
        _directionRight = (platformCenter - rightPoint).normalized;

        RaycastHit2D hitLeft = Physics2D.Raycast(_raycastOriginLeft, _directionLeft, _raycastDistance, _playerLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(_raycastOriginRight, _directionRight, _raycastDistance, _playerLayer);

        bool playerBelowPlatform = hitLeft.collider != null || hitRight.collider != null;

        Physics2D.IgnoreCollision(_platformCollider, _playerCollider, playerBelowPlatform);

        Debug.DrawRay(_raycastOriginLeft, _directionLeft * _raycastDistance, _rayColor);
        Debug.DrawRay(_raycastOriginRight, _directionRight * _raycastDistance, _rayColor);
    }
}
