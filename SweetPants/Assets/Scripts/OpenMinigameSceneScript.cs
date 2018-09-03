using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenMinigameSceneScript : MonoBehaviour {

    private Button minigames;
    public int sceneToLoad = 0;
    
	void Start () {
        minigames = this.GetComponent<Button>();
        minigames.onClick.RemoveAllListeners();
        minigames.onClick.AddListener(ChangeScene);
    }
	
    void ChangeScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(sceneToLoad);
        else
            SceneManager.LoadScene(1);
    }
}
