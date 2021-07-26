using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FakeKey : LeaverActivator
{
    [SerializeField] private GameObject explosionEffect;

    public override void Deactivate()
    {
        explosionEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerConstructor = other.GetComponent<PlayerConstructor>();
        if (playerConstructor)
        {
            explosionEffect.SetActive(true);
        }
    }
}