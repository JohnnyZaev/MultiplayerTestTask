using Fusion;
using UnityEngine;

namespace Network
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 MoveDirection;
        public Vector2 RotationDirection;
    }
}