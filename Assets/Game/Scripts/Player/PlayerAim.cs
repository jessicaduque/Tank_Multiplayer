using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAim : NetworkBehaviour
{
    private Camera cam;

    [SerializeField] Transform turretTransform;
    [SerializeField] InputReader inputReader;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Por não fazer diferença de funcionamento e ser melhor de perfomance, é usado o LateUpdate
    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        Vector2 aimWorldPosition = cam.ScreenToWorldPoint(inputReader.aimPosition);

        turretTransform.up = new Vector2(aimWorldPosition.x - turretTransform.position.x, aimWorldPosition.y - turretTransform.position.y);
    }

}
