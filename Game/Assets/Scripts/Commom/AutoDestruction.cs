using UnityEngine;
using System.Collections;

public class AutoDestruction : MonoBehaviour
{
    public float timeToLive = 3.2f;
    protected Transform myTransform;

    protected void Awake()
    {
        myTransform = transform;
        Invoke("Die", timeToLive);
    }

    protected void Die() {
        Destroy(myTransform.gameObject);
    }
}
