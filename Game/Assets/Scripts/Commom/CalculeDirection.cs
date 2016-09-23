using UnityEngine;
using System.Collections;
using System.Linq;

public class CalculeDirection : MonoBehaviour {

    private Transform[] Points;
    
	// Use this for initialization
	void Awake () {
        Points = transform.GetComponentsInChildren<Transform>().Where(obj => obj != transform).ToArray();
	}

    public Vector3 GetDirection()
    {
        return (Points[0].localPosition - Points[1].localPosition).normalized;
    }
	
}
