using Unity.Netcode;
using UnityEngine;

public class SpawnController : NetworkBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _player1Prefab; 
    [SerializeField] private GameObject _player2Prefab;

    public static SpawnController Singleton { get; private set; }

    private void OnEnable()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }

    }
    public override void OnDestroy()
    {
        base.OnDestroy();

        if (Singleton == this)
        {
            Singleton = null;
        }
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        else
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1);

        _startPanel.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayerServerRpc(ulong clientId, int prefabId)
    {
        GameObject tempNO;
        if (prefabId == 0)
            tempNO = Instantiate(_player1Prefab);
        else
            tempNO = Instantiate(_player2Prefab);
        NetworkObject netObj = tempNO.GetComponent<NetworkObject>();
        tempNO.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId, true);
    }
}
