using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Network
{

	public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
	{
		public NetworkPlayer networkPlayerPrefab;

		private PlayerInputHandler _playerInputHandler;
		
		public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			if (runner.IsServer)
			{
				Debug.Log("OnPlayerJoined - Server spawned");
				runner.Spawn(networkPlayerPrefab, Utils.Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
			}
			else
			{
				Debug.Log("OnPlayerJoined - Client spawned");
			}
		}

		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
		}

		public void OnInput(NetworkRunner runner, NetworkInput input)
		{
			if (_playerInputHandler == null && NetworkPlayer.Local != null)
			{
				_playerInputHandler = NetworkPlayer.Local.GetComponent<PlayerInputHandler>();
			}

			if (_playerInputHandler != null)
			{
				input.Set(_playerInputHandler.GetNetworkInput());
			}
		}

		public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{
		}

		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
			Debug.Log("OnShutdown");
		}

		public void OnConnectedToServer(NetworkRunner runner)
		{
			Debug.Log("OnConnectedToServer");
		}

		public void OnDisconnectedFromServer(NetworkRunner runner)
		{
			Debug.Log("OnDisconnectedFromServer");
		}

		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
			Debug.Log("OnConnectRequest");
		}

		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
			Debug.Log("OnConnectFailed");
		}

		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
		{
		}

		public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
		{
		}

		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
		{
		}

		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
		{
		}

		public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
		{
		}

		public void OnSceneLoadDone(NetworkRunner runner)
		{
		}

		public void OnSceneLoadStart(NetworkRunner runner)
		{
		}
	}
}
