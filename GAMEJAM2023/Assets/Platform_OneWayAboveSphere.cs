using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_OneWayAboveSphere : MonoBehaviour
{
    private PlayerControls _playerControls;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private GameObject downButton;

    private Collider2D _platformCollider;
    private Collider2D _playerCollider;

    [SerializeField] private float _collisionDisableDuration = 0.25f; // Change the duration as needed
    private float _collisionDisableTime = 0.0f;
    private bool _ignoreCollision = false;

    private bool _playerAbovePlatform;

    private void Awake()
    {
        _platformCollider = GetComponent<Collider2D>();
        _playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        _playerControls = new PlayerControls();
    }

    private void Start()
    {
        downButton.SetActive(false);
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
        // Define a rectangle below the player's feet to check for overlapping platforms
        Vector2 playerFeetPosition = _playerCollider.bounds.center - new Vector3(0f, _playerCollider.bounds.extents.y, 0f);
        Vector2 playerFeetSize = new Vector2(_playerCollider.bounds.size.x, 0.1f); // Adjust the size as needed

        // Check if the player's feet overlap with the platform collider
        _playerAbovePlatform = Physics2D.OverlapArea(playerFeetPosition - playerFeetSize * 0.5f, playerFeetPosition + playerFeetSize * 0.5f, playerLayer);

        downButton.SetActive(_playerAbovePlatform);

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

        Physics2D.IgnoreCollision(_platformCollider, _playerCollider, _ignoreCollision);
    }

    public bool isSelected;

    private void OnDrawGizmos()
    {
        // Ensure that the Gizmos are only drawn when the object is selected
        if (!isSelected) return;

        // Set the color for the sphere
        Gizmos.color = _color;

        // Draw the sphere at the transform position with a radius of 0.5 units (adjust as needed)
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

}
