using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] InputReader _inputReader;
    [SerializeField] Transform _projectileSpawnPoint;
    [SerializeField] GameObject _serverProjectilePrefab;
    [SerializeField] GameObject _clientProjectilePrefab;
    [SerializeField] GameObject _muzzleFlash;
    [SerializeField] Collider2D _playerCollider;
    [SerializeField] CoinWallet _coinWallet;

    [Header("Settings")]
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _fireRate; // 0.75
    [SerializeField] float _muzzleFlashDuration; // 0.075
    [SerializeField] int _costToFire;

    private bool _shouldFire;
    private float _timer;
    private float _muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        _inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }

        _inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void Update()
    {
        if(_muzzleFlashTimer > 0f)
        {
            _muzzleFlashTimer -= Time.deltaTime;
            if (_muzzleFlashTimer <= 0f)
            {
                _muzzleFlash.SetActive(false);
            }
        }

        if (!IsOwner)
        {
            return;
        }

        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if (!_shouldFire)
        {
            return;
        }

        if (_timer > 0)
        {
            return;
        }

        if(_coinWallet.totalCoins.Value < _costToFire)
        {
            return;
        }

        PrimaryFireServerRpc(_projectileSpawnPoint.position, _projectileSpawnPoint.up);
        SpawnDummyProjectile(_projectileSpawnPoint.position, _projectileSpawnPoint.up);

        _timer = 1 / _fireRate;
    }

    [ServerRpc]
    void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (_coinWallet.totalCoins.Value < _costToFire)
        {
            return;
        }

        _coinWallet.SpendPoints(_costToFire);

        GameObject projectileInstance = Instantiate(_serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;

        Physics2D.IgnoreCollision(_playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * _projectileSpeed;
        }

        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner)
        {
            return;
        }

        SpawnDummyProjectile(spawnPos, direction);
    }

    void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        _muzzleFlash.SetActive(true);
        _muzzleFlashTimer = _muzzleFlashDuration;

        GameObject projectileInstance = Instantiate(_clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;

        Physics2D.IgnoreCollision(_playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * _projectileSpeed;
        }

    }

    void HandlePrimaryFire(bool shouldFire)
    {
        this._shouldFire = shouldFire;
    }

}
