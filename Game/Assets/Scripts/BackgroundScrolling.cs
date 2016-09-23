using UnityEngine;
using System.Collections;

public class BackgroundScrolling : MonoBehaviour {

    public GameObject Prefab;
    public int timeToInstanciate = 5;

    private int TimeToInstanciate
    {
        get
        {
            return Random.Range(timeToInstanciate - 5, timeToInstanciate);
        }
    }

    void Start()
    {
         StartCoroutine("Create");
    }

    private IEnumerator Create()
    {
        yield return new WaitForSeconds(0);
        float time = 0;
        do
        {
            if (time <= 0)
            {
                Instantiate(Prefab, transform.position, transform.rotation);
                time = TimeToInstanciate;
            }
            else
            {
                time -= 1;
                yield return new WaitForSeconds(1);
            }
        }while (true);
    }
}
