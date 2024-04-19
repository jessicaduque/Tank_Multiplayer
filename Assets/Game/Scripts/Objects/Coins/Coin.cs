using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    protected int _coinValue = 10;
    protected bool _alreadyCollected;

    public abstract int Collect();
    protected void Show(bool show)
    {
        _spriteRenderer.enabled = show;
    }
    public void SetValue(int value)
    {
        _coinValue = value;
    }
}
