using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Weapon.Projectiles
{
    public class BulletHandler : NetworkBehaviour
    {
        [Header("Collision detection")] 
        
        public LayerMask collisionLayers;

        public Transform checkForImpactPoint;

        private TickTimer _maxLiveDurationTickTimer = TickTimer.None;

        private int _bulletSpeed = 20;

        private List<LagCompensatedHit> _hits = new List<LagCompensatedHit>();

        private PlayerRef _firedByPlayerRef;
        private string _firedByPlayerName;
        private NetworkObject _firedByNetworkObject;

        private NetworkObject _networkObject;

        public void Fire(PlayerRef firedByPlayerRef, NetworkObject firedByNetworkObject, string firedByPlayerName, Vector2 direction)
        {
            _firedByPlayerRef = firedByPlayerRef;
            _firedByNetworkObject = firedByNetworkObject;
            _firedByPlayerName = firedByPlayerName;

            _networkObject = GetComponent<NetworkObject>();

            _maxLiveDurationTickTimer = TickTimer.CreateFromSeconds(Runner, 3);
        }

        public override void FixedUpdateNetwork()
        {
            transform.position += transform.right * Runner.DeltaTime * _bulletSpeed;
            
            if (Object.HasStateAuthority)
            {
                if (_maxLiveDurationTickTimer.Expired(Runner))
                {
                    Runner.Despawn(_networkObject);

                    return;
                }

                Runner.GetPhysicsScene2D();
                int hitCount = Runner.LagCompensation.OverlapSphere(checkForImpactPoint.position, 0.1f,
                    _firedByPlayerRef, _hits, collisionLayers, HitOptions.IncludePhysX);

                bool isValidHit = hitCount > 0;

                for (int i = 0; i < hitCount; i++)
                {
                    if (_hits[i].Hitbox != null)
                    {
                        if (_hits[i].Hitbox.Root.GetBehaviour<NetworkObject>() == _firedByNetworkObject)
                            isValidHit = false;
                    }
                }

                if (isValidHit)
                {
                    for (int i = 0; i < hitCount; i++)
                    {
                        HealthHandler healthHandler = _hits[i].Hitbox.transform.root.GetComponent<HealthHandler>();
                        
                        if (healthHandler != null)
                            healthHandler.OnTakeDamage();
                    }
                    
                    Runner.Despawn(_networkObject);
                }
            }
        }
    }
}
