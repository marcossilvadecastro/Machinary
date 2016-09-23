using UnityEngine;
using System.Collections;

public class TouchToDie : MonoBehaviour
{


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            FighterBase fighter = coll.gameObject.GetComponent<FighterBase>();
            fighter.Die();
        }
    }
}
