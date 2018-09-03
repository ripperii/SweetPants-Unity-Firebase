using System.Collections;
using UnityEngine;

public class FadeInOutScript : MonoBehaviour {
    
    public IEnumerator fadeIn(CanvasGroup canvasgroup, float duration)
    {
        while (canvasgroup.alpha < 1)
        {
            canvasgroup.alpha += Time.deltaTime / duration;
            yield return null;
        }
        canvasgroup.blocksRaycasts = true;
        canvasgroup.interactable = true;

        yield return null;
    }
    public IEnumerator fadeOut(CanvasGroup canvasgroup, float duration)
    {
        while (canvasgroup.alpha > 0)
        {
            canvasgroup.alpha -= Time.deltaTime / duration;
            yield return null;
        }
        canvasgroup.blocksRaycasts = false;
        canvasgroup.interactable = false;
        //canvasgroup.transform.GetChild(0).gameObject.SetActive(false);
        yield return null;
    }
}
