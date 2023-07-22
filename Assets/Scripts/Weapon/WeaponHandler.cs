using System;
using System.Collections;
using Fusion;
using Network;
using UnityEngine;
using Weapon.Projectiles;
using NetworkPlayer = Network.NetworkPlayer;

public class WeaponHandler : NetworkBehaviour
{
	public BulletHandler bulletPrefab;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private LayerMask collisionLayers;
	[Networked(OnChanged = nameof(OnFireChanged))]
	public bool IsFiring { get; set; }

	private float _lastTimeFired;
	private static readonly int IsShooting = Animator.StringToHash("IsShooting");

	private NetworkObject _networkObject;
	private NetworkPlayer _networkPlayer;

	private void Awake()
	{
		_networkPlayer = GetComponentInParent<NetworkPlayer>();
		_networkObject = GetComponentInParent<NetworkObject>();
	}

	public override void FixedUpdateNetwork()
	{
		if (!GetInput(out NetworkInputData networkInputData)) return;
		if (networkInputData.RotationDirection != Vector2.zero)
		{
			Fire(transform.right);
		}
	}

	private void Fire(Vector2 aimDirection)
	{
		if (Time.time - _lastTimeFired < 0.15f)
			return;

		StartCoroutine(StartFireAnimation());
		// Runner.LagCompensation.Raycast(transform.position, transform.right, 100, Object.InputAuthority, out var hitInfo,
		// 	collisionLayers, HitOptions.IgnoreInputAuthority);

		Runner.Spawn(bulletPrefab, transform.position, transform.rotation, Object.InputAuthority,
			(runner, spawnedBullet) =>
			{
				spawnedBullet.GetComponent<BulletHandler>().Fire(Object.InputAuthority, _networkObject,
					_networkPlayer.NickName.ToString(), aimDirection);
			});

		// float hitDistance = 100f;
		// bool isHitOtherPlayer = false;
		//
		// if (hitInfo.Distance > 0)
		// 	hitDistance = hitInfo.Distance;
		//
		// if (hitInfo.Hitbox != null)
		// {
		// 	if (Object.HasStateAuthority)
		// 	{
		// 		hitInfo.Hitbox.transform.root.GetComponent<HealthHandler>().OnTakeDamage();
		// 	}
		// 	
		// 	isHitOtherPlayer = true;
		// }
		// else if (hitInfo.Collider != null)
		// {
		// 	
		// }
		//
		// if (isHitOtherPlayer)
		// {
		// 	Debug.DrawRay(transform.position, transform.right * hitDistance, Color.red, 1);
		// }
		// else
		// {
		// 	Debug.DrawRay(transform.position, transform.right * hitDistance, Color.green, 1);
		// }
		
		_lastTimeFired = Time.time;
	}

	IEnumerator StartFireAnimation()
	{
		IsFiring = true;
		
		playerAnimator.SetBool(IsShooting, true);
		yield return new WaitForSeconds(0.09f);
		playerAnimator.SetBool(IsShooting, false);

		IsFiring = false;
	}

	static void OnFireChanged(Changed<WeaponHandler> changed)
	{
		Debug.Log($"{Time.time}On FireChanged value{changed.Behaviour.IsFiring}");

		bool isFiringCurrent = changed.Behaviour.IsFiring;
		
		changed.LoadOld();

		bool isFiringOld = changed.Behaviour.IsFiring;
		
		if (isFiringCurrent && !isFiringOld)
			changed.Behaviour.OnFireRemote();
		else if (!isFiringCurrent && isFiringOld)
			changed.Behaviour.OnStopFireRemote();
	}

	void OnFireRemote()
	{
		if (!Object.HasInputAuthority)
			playerAnimator.SetBool(IsShooting, true);
	}

	void OnStopFireRemote()
	{
		if (!Object.HasInputAuthority)
			playerAnimator.SetBool(IsShooting, false);
	}
}
