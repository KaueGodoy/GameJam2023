using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    private Transform _target;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    public bool IsFacingRight;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) _target = playerObject.transform;
    }

    public void FlipBasedOnMovingDirection()
    {
        Vector3 direction = _rb.velocity;

        if (direction.x < 0.1f)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void FlipBasedOnPlayerPosition()
    {
        Vector3 direction = (_target.position - transform.position).normalized;

        if (IsFacingRight && direction.x < 0.1f || !IsFacingRight && direction.x > 0.1f)
        {
            IsFacingRight = !IsFacingRight;
            _spriteRenderer.flipX = IsFacingRight;
        }
    }

    public void FlipTest()
    {
        Vector3 direction = (_target.position - transform.position).normalized;

        if (direction.x < 0.1f)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }

    //public void FlipBasedOnPlayerPosition(Vector3 distance)
    //{
    //    Vector3 direction = (_target.position - transform.position).normalized;

    //    distance = direction;

    //    if (IsFacingRight && distance.x < 0f || !IsFacingRight && distance.x > 0f)
    //    {
    //        IsFacingRight = !IsFacingRight;
    //        _spriteRenderer.flipX = IsFacingRight;
    //    }
    //}
}
