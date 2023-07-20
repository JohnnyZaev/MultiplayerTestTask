using Network;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
	private Vector2 _inputVector;

	public void OnMove(InputAction.CallbackContext context)
	{
		_inputVector = context.ReadValue<Vector2>();
	}

	public NetworkInputData GetNetworkInput()
	{
		NetworkInputData networkInputData = new NetworkInputData();
		networkInputData.Direction = _inputVector;

		return networkInputData;
	}
}
