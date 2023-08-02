using UnityEngine;

public class Platform_Sticky : MonoBehaviour
{
    private SpriteRenderer _playerSpriteRenderer;
    private Rigidbody2D _playerRigidbody;

    private void Awake()
    {
        _playerSpriteRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        _playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float playerFootY = collision.transform.position.y - _playerSpriteRenderer.bounds.extents.y - 0.1f;

            if (transform.position.y < playerFootY)
            {
                _playerRigidbody.interpolation = RigidbodyInterpolation2D.None;
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerRigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
            collision.transform.SetParent(null);
        }
    }
}
