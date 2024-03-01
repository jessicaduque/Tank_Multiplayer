using System.Collections;
using System.Collections.Generic;
using Unity.Services.Matchmaker.Models;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;

    private Transform bodyTransform;
    private Rigidbody2D thisRb;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float tunningRate = 100f;

    Vector2 previousMovementInput;

    private void Awake()
    {
        bodyTransform = GetComponent<Transform>();
        thisRb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }

        inputReader.MoveEvent += ControlMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }

        inputReader.MoveEvent -= ControlMovement;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        float zRotation = previousMovementInput.x * -tunningRate * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        thisRb.velocity = (Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;

    }

    #region Movement

    private void ControlMovement(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }

    #endregion

}
