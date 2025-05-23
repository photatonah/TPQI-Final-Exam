using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destroy the explosion effect after 2 second
        Destroy(gameObject, 2f);
    }
}
