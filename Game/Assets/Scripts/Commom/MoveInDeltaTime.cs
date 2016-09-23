using UnityEngine;
using System.Collections;

public static class MoveInDeltaTime {

    public static IEnumerator MoveInTime(this Transform t, Vector3 target, float deltaTime)
    {

        float counter = 0;
        Vector3 distance = (target - t.position);
        float length = distance.magnitude;
        distance.Normalize();

        while (counter <= deltaTime)
        {
            float movAmount = (length * Time.deltaTime) / deltaTime;
            t.position += distance * movAmount;
            counter += Time.deltaTime;   
            yield return null;
        }
        t.position = target;
    }


}
