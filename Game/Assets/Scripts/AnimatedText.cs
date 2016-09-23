using UnityEngine;
using System.Collections;

public class AnimatedText : MonoBehaviour {

    public GUILabel Label;
    private string SavedText;

    // Use this for initialization
    void Start()
    {
        SavedText = Label.LabelData.text;
        StartCoroutine(PlayLoadingAnimationLabel());
    }

    private IEnumerator PlayLoadingAnimationLabel()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(0.8f);
                Label.LabelData.text += " .";
            }
            Label.LabelData.text = SavedText;
        }
    }
}
