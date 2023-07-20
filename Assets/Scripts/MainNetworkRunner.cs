using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

[RequireComponent(typeof(NetworkRunner))]
public class MainNetworkRunner : MonoBehaviour, INetworkRunnerCallbacks
{
	public NetworkPlayer playerPrefab;
	
	private NetworkRunner _networkRunner;

	private void Awake()
	{
		_networkRunner = GetComponent<NetworkRunner>();
	}

	private void Start()
	{
		InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(),
			SceneManager.GetActiveScene().buildIndex, null);
	}

	protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address,
		SceneRef scene, Action<NetworkRunner> initialized)
	{
		runner.ProvideInput = true;
		
		Debug.Log("Session Initialized");

		return runner.StartGame(new StartGameArgs
		{
			GameMode = gameMode,
			Address = address,
			Scene = scene,
			SessionName = "SessionName",
			Initialized = initialized,
			SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
		});
	}

	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
		if (runner.IsServer)
		{
			Vector3 spawnLocation = Vector3.zero;
			Debug.Log("Player Joined");
			runner.Spawn(playerPrefab, spawnLocation, Quaternion.identity, player.PlayerId);
		}
	}

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
	}

	public void OnInput(NetworkRunner runner, NetworkInput input)
	{
		if (NetworkPlayer.local != null)
		{
			InputHandler localInputHandler = NetworkPlayer.local.GetComponent<InputHandler>();
			if (localInputHandler != null)
			{
				input.Set(localInputHandler.GetNetworkInput());
			}
		}
	}

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
	}

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
	}

	public void OnConnectedToServer(NetworkRunner runner)
	{
	}

	public void OnDisconnectedFromServer(NetworkRunner runner)
	{
	}

	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
	}

	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
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