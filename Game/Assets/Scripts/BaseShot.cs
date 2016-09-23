using UnityEngine;
using System.Collections;

public class BaseShot : AutoDestruction {

    public float mySpeed = 5, myAmplitude = 1.5f, myOmega = 1.5f;
    public int damage = 2;
    public Vector3 Orientation { set; get; }
    public Component ExplosionComponent;
    public Transform MyOwnInstance;
    private float mydefaultDistance = 10f;
    int p = 0;

    new void Awake()
    {
        base.Awake();
        enabled = false;
    }

	void Start ()
    {
        p = Random.Range(1, 3);

	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        //TODO: verificar tag correta para aplicar dano
        if (coll.gameObject.tag == "Player" && coll.transform != MyOwnInstance && enabled)
        {
            coll.gameObject.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
        Die();
    }

    Quaternion LookAt(Vector3 newPosition, Vector3 position)
    {
        Vector3 dir = position - newPosition;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, angle - 180);
    }

	void Update () {
        if (Orientation == Vector3.zero || !enabled)
        {
            return;
        }
        float myNewSpeed = mySpeed * Time.deltaTime;
        Vector3 movimentPosition = new Vector3(Orientation.x*myNewSpeed , Orientation.y*myNewSpeed, 0);
        float cos =   Mathf.Cos(myOmega * Time.time);
        float sin = Mathf.Sin(myOmega * Time.time);
        Vector3 wavePosition = new Vector3(Orientation.y * myAmplitude * ( p == 1 ? cos : sin) * Time.deltaTime, Orientation.x * myAmplitude * sin * Time.deltaTime, 0);
        Vector3 newPosition = movimentPosition + wavePosition;
        myTransform.position += newPosition;
        myTransform.rotation = LookAt(newPosition, newPosition * mydefaultDistance);
    }

    new void Die()
    {
        GameObject.Instantiate(ExplosionComponent, myTransform.position, Quaternion.identity);
        base.Die();
    }
}
