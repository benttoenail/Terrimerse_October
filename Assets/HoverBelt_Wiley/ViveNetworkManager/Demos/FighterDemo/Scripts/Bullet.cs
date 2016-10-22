using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TakeDamage>() != null || other.gameObject.GetComponent<DestroysBullet>() != null)
            Destroy(gameObject);
    }
}
