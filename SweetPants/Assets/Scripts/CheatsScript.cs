using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheatsScript : MonoBehaviour {
    
    public Button addxp;
    public Button setxp;
    public Button close;
    public Button addLevel;
    public Button showResolution;
    public Button shopCDTimerCheat;
    public Button forceUpdateRemoteConfig;

    public GameObject XPPopup;
    public Button CloseXPPopupButton;
    public Button AddOrSetXPButton;

    public Text resolution;

    public FadeInOutScript fade;
    
	void Start ()
    {
        addxp.onClick.RemoveAllListeners();
        addxp.onClick.AddListener(AddXPButton);

        setxp.onClick.RemoveAllListeners();
        setxp.onClick.AddListener(SetXPButton);

        close.onClick.RemoveAllListeners();
        close.onClick.AddListener(CloseMenu);

        addLevel.onClick.RemoveAllListeners();
        addLevel.onClick.AddListener(AddLevelButton);

        showResolution.onClick.RemoveAllListeners();
        showResolution.onClick.AddListener(ShowResolutionButton);

        shopCDTimerCheat.onClick.RemoveAllListeners();
        shopCDTimerCheat.onClick.AddListener(ShopCDTimerCheat);

        forceUpdateRemoteConfig.onClick.RemoveAllListeners();
        forceUpdateRemoteConfig.onClick.AddListener(ForceUpdateRemoteConfig);

        CloseXPPopupButton.onClick.RemoveAllListeners();
        CloseXPPopupButton.onClick.AddListener(CloseXPPopup);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void AddXPButton()
    {
        transform.gameObject.SetActive(false);
        XPPopup.SetActive(true);
        Button b = XPPopup.transform.GetChild(1).GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.transform.GetChild(0).GetComponent<Text>().text = "Add XP";
        b.onClick.AddListener(() => AddXP(XPPopup.transform.GetChild(0).GetComponent<InputField>()));
    }
    void AddXP(InputField xp)
    {
        DatabaseManager.sharedInstance.UpdateXP(Player.currentPlayer.XP + int.Parse(xp.text));
    }
    void SetXPButton()
    {
        transform.gameObject.SetActive(false);
        XPPopup.SetActive(true);
        Button b = XPPopup.transform.GetChild(1).GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.transform.GetChild(0).GetComponent<Text>().text = "Set XP";
        b.onClick.AddListener(() => SetXP(XPPopup.transform.GetChild(0).GetComponent<InputField>()));
    }
    void SetXP(InputField xp)
    {
        DatabaseManager.sharedInstance.UpdateXP(int.Parse(xp.text));
    }
    void ShowResolutionButton()
    {
        resolution.gameObject.SetActive(!resolution.gameObject.activeSelf);
    }
    
    void AddLevelButton()
    {

    }
    void CloseMenu()
    {
        transform.gameObject.SetActive(false);
    }
    void ShopCDTimerCheat()
    {

    }
    void ForceUpdateRemoteConfig()
    {
        RemoteSettings.ForceUpdate();
        RemoteConfig.rc.RemoteConfigUpdatedEvent();
    }
    void CloseXPPopup()
    {
        XPPopup.SetActive(false);
    }
}
