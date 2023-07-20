using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
	public static NetworkPlayer local { get; set; }
	
	public override void Spawned()
	{
		if (Object.HasInputAuthority)
		{
			local = this;
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
