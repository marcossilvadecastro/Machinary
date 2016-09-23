using UnityEngine;
using System.Collections;

public class ArrowEffectBobbing : MonoBehaviour
{

    private Transform ThisTransform = null;
    public Vector2 moveDir = Vector2.zero;
    public float movementSpeed = 0.0f;
    public float travelDistance = 0.0f;
    public bool isBobbling = false;


    // Use this for initialization
    void Start()
    {
        ThisTransform = transform;
        StartCoroutine("StartTravel");
    }

    IEnumerator StartTravel()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        while (true)
        {
            if (isBobbling)
            {
                moveDir = moveDir * -1;
            }
            yield return StartCoroutine(Travel());
        }
    }


    IEnumerator Travel()
    {

        float DistanceTravelled = 0.0f;
        while ((DistanceTravelled < travelDistance) && movementSpeed > 0)
        {
            Vector2 distanceToTravel = moveDir * movementSpeed * Time.deltaTime;
            ThisTransform.position += new Vector3(distanceToTravel.x, distanceToTravel.y, 0);
            DistanceTravelled += distanceToTravel.magnitude;
            yield return null;
        }
    }

}