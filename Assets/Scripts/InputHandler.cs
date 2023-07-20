using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public struct NetworkInputData : INetworkInput
{
	public Vector2 direction;
}

public class InputHandler : MonoBehaviour
{
	private Vector2 _inputVector;

	public void OnMove(InputAction.CallbackContext context)
	{
		_inputVector = context.ReadValue<Vector2>();
	}

	public NetworkInputData GetNetworkInput()
	{
		NetworkInputData networkInputData = new NetworkInputData();
		networkInputData.direction = _inputVector;

		return networkInputData;
	}
}
