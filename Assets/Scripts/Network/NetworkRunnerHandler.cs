using System;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
	public class NetworkRunnerHandler : MonoBehaviour
	{
		public NetworkRunner networkRunnerPrefab;

		private NetworkRunner _networkRunner;

		private void Awake()
		{
			NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>();

			if (networkRunnerInScene != null)
				_networkRunner = networkRunnerInScene;
		}

		private void Start()
		{
			if (_networkRunner == null)
			{
				_networkRunner = Instantiate(networkRunnerPrefab);
				_networkRunner.name = "Network runner";
				
				if (SceneManager.GetActiveScene().name == "Game")
				{
					var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, "TestSession", NetAddress.Any(),
						SceneManager.GetActiveScene().buildIndex, null);
				}
			}


			Debug.Log("Server networkRunner started");
		}
		
		protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName, NetAddress address,
			SceneRef scene, Action<NetworkRunner> initialized)
		{
			var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>()
				.FirstOrDefault();
			if (sceneManager == null)
			{
				runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
			}
			
			runner.ProvideInput = true;
		
			Debug.Log("Session Initialized");

			return runner.StartGame(new StartGameArgs
			{
				GameMode = gameMode,
				Address = address,
				Scene = scene,
				SessionName = sessionName,
				CustomLobbyName = "OurLobbyID",
				Initialized = initialized,
				SceneManager = sceneManager
			});
		}

		public void OnJoinLobby()
		{
			var clientTask = JoinLobby();
		}

		private async Task JoinLobby()
		{
			Debug.Log("JoinLobby started");

			string lobbyID = "OurLobbyID";

			var resullt = await _networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyID);

			if (!resullt.Ok)
			{
				Debug.Log("Unable to join lobby");
			}
			else
			{
				Debug.Log("Join lobby ok");
			}
		}

		public void CreateGame(string sessionName, string sceneName)
		{
			var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.Host, sessionName, NetAddress.Any(),
				SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}"), null);
		}

		public void JoinGame(string sessionName)
		{
			MainMenuUIHandler mainMenuUIHandler = FindObjectOfType<MainMenuUIHandler>();
			if (mainMenuUIHandler == null)
				return;
			if (mainMenuUIHandler.SessionInfos.Count == 0)
				return;
			if (mainMenuUIHandler.SessionInfos.Any(session => session.Name == sessionName))
			{
				var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.Client, sessionName, NetAddress.Any(),
					1, null);
			}
		}
	}
}
