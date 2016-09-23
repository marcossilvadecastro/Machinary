using UnityEngine;
using System.Collections;

public class WhiteWeapon : MonoBehaviour {

    public int DamageValue;
    private FighterBase fighter;

    void Awake()
    {
        fighter = transform.GetComponentInParent<FighterBase>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (fighter.IsAttaking && !fighter.UsingWeapon &&
            other.transform.tag == "Player" && !other.transform.GetInstanceID().Equals(fighter.transform.GetInstanceID()))
        {
            FighterBase fig = other.transform.GetComponent<FighterBase>();
            fig.ApplyDamage(DamageValue);
        }
    }
}