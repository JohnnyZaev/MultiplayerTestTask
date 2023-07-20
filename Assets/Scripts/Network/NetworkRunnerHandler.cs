using System;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
	[RequireComponent(typeof(NetworkRunner))]
	public class NetworkRunnerHandler : MonoBehaviour
	{
		public NetworkRunner networkRunnerPrefab;

		private NetworkRunner _networkRunner;
		
		private void Start()
		{
			_networkRunner = Instantiate(networkRunnerPrefab);
			_networkRunner.name = "Network runner";
			InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(),
				SceneManager.GetActiveScene().buildIndex, null);
		}
		
		protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address,
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
				SessionName = "TestRoom",
				Initialized = initialized,
				SceneManager = sceneManager
			});
		}
	}
}
