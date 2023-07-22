using Fusion;
using TMPro;
using UnityEngine;

namespace Network
{
	public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
	{
		public TMP_Text playerNickname;
		public static NetworkPlayer Local { get; set; }
		
		[Networked(OnChanged = nameof(OnNicknameChanged))]
		public NetworkString<_16> NickName { get; set; }

		// public Transform playerNicknameWatcher;
		
		public override void Spawned()
		{
			if (Object.HasInputAuthority)
			{
				Local = this;
				
				RPC_SetNickName(PlayerPrefs.GetString("PlayerNickname"));
				Debug.Log("Local Player Spawned");
			}
			else
			{
				Debug.Log("Client Player Spawned");
			}
			
			Runner.SetPlayerObject(Object.InputAuthority, Object);
		}

		// public override void FixedUpdateNetwork()
		// {
		// 	if (Object.HasInputAuthority)
		// 	{
		// 		playerNickname.transform.rotation = Quaternion.Euler(Vector3.zero);
		// 		playerNickname.transform.position = Object.transform.position + Vector3.up;
		// 	}
		// }

		public void PlayerLeft(PlayerRef player)
		{
			if (player == Object.HasInputAuthority)
			{
				Runner.Despawn(Object);
			}
		}

		private static void OnNicknameChanged(Changed<NetworkPlayer> changed)
		{
			changed.Behaviour.OnNicknameChanged();
		}

		private void OnNicknameChanged()
		{
			Debug.Log($"Nickname changed to {NickName} for player {gameObject.name}");

			playerNickname.text = NickName.ToString();
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		public void RPC_SetNickName(string nickName, RpcInfo info = default)
		{
			Debug.Log($"[RPC] SetNickName {nickName}");
			this.NickName = nickName;
		}
	}
}
