using Network;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
	private Vector2 _inputVector, _rotationVector;

	public void OnMove(InputAction.CallbackContext context)
	{
		_inputVector = context.ReadValue<Vector2>();
	}

	public void OnRotate(InputAction.CallbackContext context)
	{
		_rotationVector = context.ReadValue<Vector2>();
	}

	public NetworkInputData GetNetworkInput()
	{
		NetworkInputData networkInputData = new NetworkInputData();
		networkInputData.MoveDirection = _inputVector;
		networkInputData.RotationDirection = _rotationVector;

		return networkInputData;
	}
}
