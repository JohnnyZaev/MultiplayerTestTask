using Fusion;
using Network;
using UnityEngine;

namespace Movement
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private Transform NickNameTransform;
        [SerializeField] private float speed = 4f;
        [SerializeField] private float rotationSpeed = 0.15f;
    
        private Rigidbody2D _playerRb;
        private Vector2 _moveDirection, _rotationDirection;
        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                _moveDirection = data.MoveDirection;
                _rotationDirection = data.RotationDirection;
                NickNameTransform.rotation = Quaternion.identity;
                NickNameTransform.position = transform.position + Vector3.up;
            }
            else
            {
                _moveDirection = Vector2.zero;
                _rotationDirection = Vector2.zero;
            }

            _playerRb.velocity = _moveDirection.normalized * speed;
            var position = transform.position;
            if (_rotationDirection != Vector2.zero)
            {
                transform.right = Vector3.Slerp(transform.right, ((Vector2)position + _rotationDirection) - (Vector2) position, rotationSpeed);
                // make shooting here
            }
            else
            {
                transform.right = Vector3.Slerp(transform.right, ((Vector2)position + _moveDirection) - (Vector2) position, rotationSpeed);
            }
        }
    }
}
