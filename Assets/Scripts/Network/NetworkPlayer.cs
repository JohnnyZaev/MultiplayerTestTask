using Fusion;
using UnityEngine;

namespace Network
{
	public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
	{
		public static NetworkPlayer Local { get; set; }
	
		public override void Spawned()
		{
			if (Object.HasInputAuthority)
			{
				Local = this;
				Debug.Log("Local Player Spawned");
			}
			else
			{
				Debug.Log("Client Player Spawned");
			}
		}
	
		public void PlayerLeft(PlayerRef player)
		{
			if (player == Object.HasInputAuthority)
			{
				Runner.Despawn(Object);
			}
		}
	}
}
