using Fusion;
using Network;
using UnityEngine;

namespace Movement
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed = 4f;
    
        private Rigidbody2D _playerRb;
        private Vector2 _inputDirection;
        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            _inputDirection = GetInput(out NetworkInputData data) ? data.Direction : Vector2.zero;

            _playerRb.velocity = _inputDirection.normalized * speed;
            if (_playerRb.velocity == Vector2.zero) return;
            var transform1 = transform;
            var position = transform1.position;
            transform1.right = ((Vector2) position + _inputDirection) - (Vector2) position;
        }
    }
}
