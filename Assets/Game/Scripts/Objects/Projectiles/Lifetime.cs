using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] float lifetime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
