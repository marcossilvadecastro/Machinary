using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ListenerControlController : MonoBehaviour
{	
	public GameObject[] spawns;
	public Component[] controls;

	private Transform myTransform;

    private GameManager gameManager;
    public float Distance = 2.5f;

    private ControlController cachedControl;


    void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.DetectController();
        myTransform = transform;
    }

    void OnDisable()
    {
        ControlController.OnPlayerSelectedHandler -= OnPlayerSelected;
    }


   void Start () {
        ControlController.OnPlayerSelectedHandler += OnPlayerSelected;
        if (gameManager.ControllerConnected)
        {
            StartCoroutine(CreateControl(gameManager.Controls.Keys.ElementAt(0), "initialteclado"));
            StartCoroutine(CreateControl(gameManager.Controls.Keys.ElementAt(1), "initialcontrole"));
        }
        else
        {
            //Teste para debug
            gameManager.CreateDebugControl();
            StartCoroutine(CreateControl("tecladoP1", "initialteclado"));
        }
    }

    void OnPlayerSelected(ControlController control)
    {
        if (cachedControl == null)
        {
            cachedControl = control;
            //LOAD NEXT SCENE IF IS DEBUGGING
            if (gameManager.IsDebugging)
            {
                gameManager.LoadNextScene();
            }
            return;
        }
        if (cachedControl == control)
        {
            return;
        }

        gameManager.LoadNextScene();
        
    }

    IEnumerator CreateControl(string controlName, string spawnName){
		GameObject spawn = GetSpawnByName (spawnName);
		Component control = controls.Where (c => c.name.Contains (controlName)).First ();

        Component newControl = Instantiate (control,spawn.transform.position,Quaternion.identity) as Component;
		newControl.name = controlName;
		newControl.transform.parent = myTransform;

        ControlController controlController = newControl.GetComponent<ControlController>();
        controlController.DistanceToMove = Distance;
        controlController.MyInput = gameManager.Controls[controlName];
		yield break;
	}

    private Component GetComponentByName(string name){
		return myTransform.GetComponentsInChildren<Component> ().Where (g =>g.tag.ToLower().Contains(name)).FirstOrDefault ();

	}
	
	private GameObject GetSpawnByName(string name){
		GameObject spawn = spawns.Where (a => a.name.ToLower ().Contains (name)).First ();
		return spawn;
	}
	
	IEnumerator DestroyControl(string name){
		Component c = GetComponentByName (name);
		DestroyImmediate (c.gameObject);
		yield break;
	}
}
