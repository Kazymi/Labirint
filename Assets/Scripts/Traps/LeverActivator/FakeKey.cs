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
        var i = other.GetComponent<PlayerConstructor>();
        if (i)
        {
            explosionEffect.SetActive(true);
        }
    }
}