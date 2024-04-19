using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin _coinPrefab;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private int _maxCoins = 50;
    [SerializeField] private int _coinValue = 10;
    [SerializeField] private Vector2 _xSpawnRange;
    [SerializeField] private Vector2 _ySpawnRange;

    private Collider2D[] _coinBuffer = new Collider2D[1];
    private float _coinRadius;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius;

        for(int i=0; i<_maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        RespawningCoin coinInstance = Instantiate(_coinPrefab, GetSpawnPoint(), Quaternion.identity).GetComponent<RespawningCoin>();
        coinInstance.SetValue(_coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();
    }

    private Vector2 GetSpawnPoint()
    {
        float x = 0;
        float y = 0;

        while (true)
        {
            x = Random.Range(_xSpawnRange.x, _xSpawnRange.y);
            y = Random.Range(_ySpawnRange.x, _ySpawnRange.y);

            Vector2 spawnPoint = new Vector2(x, y);

            int numCollider = Physics2D.OverlapCircleNonAlloc(spawnPoint, _coinRadius, _coinBuffer, _layerMask);

            if(numCollider == 0)
            {
                return spawnPoint;
            }
        }
    }
}
