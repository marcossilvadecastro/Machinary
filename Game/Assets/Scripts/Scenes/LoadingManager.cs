using UnityEngine;
using System.Collections;
using System;

public class LoadingManager : MonoBehaviour {


    public IEnumerator LoadAsyncScene(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation async = Application.LoadLevelAdditiveAsync(sceneName);
        yield return async;
        //All scenes need have a Root GameObject
        Destroy(GameObject.Find("Root"));
    }

    public void LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
