using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage;

    [SerializeField] private GameObject _prefabVFXDamage;
    [SerializeField] private GameObject _prefabVFXWall;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tag))
        {
            Character character = other.GetComponent<Character>();

            if (character != null)
            {
                character.TakeDamage(_damage);
                InstantiateVisualEffect(transform.position,true);
            }
            else
            {
                InstantiateVisualEffect(transform.position, false);
            }

        }
    }

    private void InstantiateVisualEffect(Vector3 position, bool touch)
    {
        bool useDamage = touch;

        if (_prefabVFXDamage == null && useDamage) { useDamage = false; }
        else if (_prefabVFXWall == null && !useDamage) { useDamage = true; }

        if (touch)
        {
            if (_prefabVFXDamage != null)
            {
                Instantiate(_prefabVFXDamage, transform.position, transform.rotation);
            }
        }
        else if (_prefabVFXWall != null)
        {
            Instantiate(_prefabVFXWall, transform.position, transform.rotation);
        }

    }
}
