using UnityEngine;
using System.Collections;

public class MovingParentPlatformBehavior : MonoBehaviour {

    private Vector3 LastPosition;


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "Player")
            return;
        LastPosition = transform.position;

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "Player")
            return;
        
        Vector3 newPlatformPosition = transform.position;

        Vector3 distancePlatform = newPlatformPosition - LastPosition;
        if (distancePlatform != Vector3.zero)
        {
            coll.gameObject.transform.Translate(distancePlatform);
        }
        LastPosition = newPlatformPosition;

    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "Player")
            return;
        LastPosition = transform.position;
        
    }

}
