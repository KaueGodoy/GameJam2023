using UnityEngine;

public class Platform_Moving : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;
    private bool _movingToEnd = true;

    [SerializeField] private Vector3 _movePosition;
    [SerializeField] private float _speed = 2f;

    private FixedJoint2D _playerJoint;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        _endPosition = _startPosition + _movePosition;

        Vector3 targetPosition = _movingToEnd ? _endPosition : _startPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            _movingToEnd = !_movingToEnd;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_startPosition, _endPosition);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && _playerJoint == null)
    //    {
    //        // Attach the player to the platform using a fixed joint
    //        _playerJoint = collision.gameObject.AddComponent<FixedJoint2D>();
    //        _playerJoint.connectedBody = GetComponent<Rigidbody2D>();
    //        _playerJoint.autoConfigureConnectedAnchor = false;
    //        _playerJoint.anchor = collision.transform.InverseTransformPoint(collision.contacts[0].point);
    //        _playerJoint.connectedAnchor = collision.transform.InverseTransformPoint(collision.contacts[0].point);
    //    }
    //}
        
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && _playerJoint != null)
    //    {
    //        // Detach the player from the platform by destroying the fixed joint
    //        Destroy(_playerJoint, 0.3f);
    //        _playerJoint = null;
    //    }
    //}

    //private void OnValidate()
    //{
    //    _startPosition = transform.position;
    //    _endPosition = _startPosition + _movePosition;
    //}
}
