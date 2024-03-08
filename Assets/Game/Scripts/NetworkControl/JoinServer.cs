using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class JoinServer : MonoBehaviour
{
    public void HostServer()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void JoinHostedServer()
    {
        NetworkManager.Singleton.StartHost();
    }
}
