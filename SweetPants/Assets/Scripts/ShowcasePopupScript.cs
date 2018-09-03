using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShowcasePopupScript : MonoBehaviour {

    public InputField input;
    public Button plus;
    public Button minus;
    public Button cancel;
    public Button showcase;
    public Slider slider;
    public Image itemIcon;
    public Image bg;
    public Text amountText;
    public Text itemName;
    public FadeInOutScript fade;

    public Item item;
    public int amount;
    public int baseValue;
    public double priceCoef = 1;

    public float red, green;
    public float fadeDuration = .5f;

    private void Awake()
    {
        plus.onClick.RemoveAllListeners();
        minus.onClick.RemoveAllListeners();
        cancel.onClick.RemoveAllListeners();
        showcase.onClick.RemoveAllListeners();

        plus.onClick.AddListener(PlusButton);
        minus.onClick.AddListener(MinusButton);
        cancel.onClick.AddListener(CancelButton);
        showcase.onClick.AddListener(ShowcaseButton);

        input.onValueChanged.AddListener(ChangeInputColor);
        slider.onValueChanged.AddListener(ChangeSlider);
        
        fade = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<FadeInOutScript>();

        Debug.Log("ShowcasePopup Awake finished!");
    }
    public void OpenShowcasePopup(Item it, int amount)
    {
        Debug.Log("ShowcasePopup OpenShowcasePopup started!");
        item = it;
        this.amount = amount;
        baseValue = it.baseValue;
        priceCoef = 1;
        slider.maxValue = amount;
        itemIcon.sprite = Resources.Load<Sprite>(it.icon);
        itemName.text = item.name;
        amountText.text = "1";

        slider.GetComponent<CanvasGroup>().alpha = amount<=1 ? 0 : 1;
        itemName.color = bg.color = ItemRarityColors.colors.Where(x => x.Key.ToLower() == item.rarity.ToLower()).FirstOrDefault().Value;

        input.text = baseValue.ToString();

        StartCoroutine(fade.fadeIn(transform.GetComponent<CanvasGroup>(), fadeDuration));

    }
    // Update is called once per frame
    void Update () {
		
	}
    public void PlusButton()
    {
        input.text = (int.Parse(input.text) + 10).ToString();
    }
    public void MinusButton()
    {
        input.text = (int.Parse(input.text) - 10).ToString();
    }
    public void CancelButton()
    {
        StartCoroutine(DestroyAfterFade());
    }
    public IEnumerator DestroyAfterFade()
    {
        yield return StartCoroutine(fade.fadeOut(transform.GetComponent<CanvasGroup>(), fadeDuration));
        Destroy(gameObject);
    }
    public void ShowcaseButton()
    {

    }
    public void ChangeInputColor(string text)
    {
        Debug.Log("Text in Event: " + text);
        float f;
        if (!float.TryParse(text, out f))
        {
            f = 1;
            input.text = "1";
        }
        if (f < 1)
        {
            f = 1;
            input.text = "1";
        }
        if (f > Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value)
        {
            f = Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value;
            input.text = Player.currentPlayer.Inventory.Where(x => x.Key.id == "2").FirstOrDefault().Value.ToString();
        }

        if (f == baseValue * slider.value)
        {
            green = red = 1f;
        }
        if (f > baseValue * slider.value)
        {
            green = 1 - ((f - (baseValue * slider.value)) / (baseValue * slider.value));
            red = 1f;
        }
        else if (f < baseValue * slider.value)
        {
            red = f / (baseValue * slider.value);
            green = 1f;
        }
        
        input.GetComponent<Image>().color = new Color(red, green, 0);
    }
    public void ChangeSlider(float amount)
    {
        amountText.text = ((int)amount).ToString();
        input.text = (baseValue * ((int)amount)* priceCoef).ToString();
    }
}
