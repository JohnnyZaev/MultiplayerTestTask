using System;
using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthHandler : NetworkBehaviour
{
	[Networked(OnChanged = nameof(OnHPChanged))]
	private byte Hp { get; set; }
	
	[Networked(OnChanged = nameof(OnStateChanged))]
	public bool IsDead { get; set; }

	private bool _isInitialized;

	private const byte STARTING_HEALTH = 5;

	public Color uiOnHitColor;
	private Image _uiOnHitImage;
	public SpriteRenderer bodySpriteRenderer;
	private Color _defaultBodySpriteRendererColour;

	public GameObject playerModel;

	private HitboxRoot _hitboxRoot;
	private PlayerInput _playerInput;

	private void Awake()
	{
		_uiOnHitImage = GameObject.FindWithTag("TakeDamageImage").GetComponent<Image>();
		_hitboxRoot = GetComponent<HitboxRoot>();
		_playerInput = GetComponent<PlayerInput>();
	}

	private void Start()
	{
		Hp = STARTING_HEALTH;
		IsDead = false;

		_defaultBodySpriteRendererColour = bodySpriteRenderer.color;

		_isInitialized = true;
	}

	IEnumerator OnHitColor()
	{
		bodySpriteRenderer.color = Color.red;

		if (Object.HasInputAuthority)
			_uiOnHitImage.color = uiOnHitColor;

		yield return new WaitForSeconds(0.2f);

		bodySpriteRenderer.color = _defaultBodySpriteRendererColour;

		if (Object.HasInputAuthority && !IsDead)
			_uiOnHitImage.color = new Color(0, 0, 0, 0);
	}

	public void OnTakeDamage()
	{
		if (IsDead)
			return;

		Hp -= 1;

		if (Hp <= 0)
		{
			IsDead = true;
			Debug.Log("Dead");
		}
	}

	private static void OnHPChanged(Changed<HealthHandler> changed)
	{
		Debug.Log("OnHPChanged");

		byte newHealth = changed.Behaviour.Hp;
		
		changed.LoadOld();

		byte oldHealth = changed.Behaviour.Hp;

		if (newHealth < oldHealth)
			changed.Behaviour.OnHealthReduced();
	}

	private void OnHealthReduced()
	{
		if (!_isInitialized)
			return;

		StartCoroutine(OnHitColor());
	}
	
	private static void OnStateChanged(Changed<HealthHandler> changed)
	{
		Debug.Log("OnStateChanged");

		bool isDeadCurrent = changed.Behaviour.IsDead;
		
		changed.LoadOld();

		bool isDeadOld = changed.Behaviour.IsDead;
		
		if (isDeadCurrent)
			changed.Behaviour.OnDeath();
		else if (!isDeadCurrent && isDeadOld)
		{
			// maybe add revive later
		}
	}
	
	private void OnDeath()
	{
		Debug.Log("OnDeath");
		
		playerModel.gameObject.SetActive(false);
		_hitboxRoot.HitboxRootActive = false;
		_playerInput.enabled = false;
	}
}
