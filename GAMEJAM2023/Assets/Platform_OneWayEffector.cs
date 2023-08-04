using UnityEngine;

public class Platform_OneWayEffector : MonoBehaviour
{
    public bool canPassThrough = false;
    public float passThroughTime = 0.5f;

    private PlatformEffector2D effector;
    private float passThroughTimer;

    private PlayerControls _playerControls;

    private bool _playerOnPlatform;

    private void Awake()
    {
        effector = GetComponent<PlatformEffector2D>();
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
        if (!canPassThrough) return;

        if (_playerOnPlatform && _playerControls.Player.Down.triggered && passThroughTimer <= 0f)
        {
            // Enable the PlatformEffector2D to let the player pass through
            effector.rotationalOffset = 180f;
            passThroughTimer = passThroughTime;
        }

        // Countdown the passThroughTimer
        if (passThroughTimer > 0f)
        {
            passThroughTimer -= Time.deltaTime;
            if (passThroughTimer <= 0f)
            {
                // Reset the PlatformEffector2D when the timer is up
                effector.rotationalOffset = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _playerOnPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _playerOnPlatform = false;
    }
}
