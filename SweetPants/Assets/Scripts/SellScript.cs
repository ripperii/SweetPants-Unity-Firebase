using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class SellScript : MonoBehaviour {

    public static GameObject SalePrompt;
    public Button minusAmount;
    public Button plusAmount;
    public InputField amountOfItem;
    public Slider amountOfItemSlider;
    public Button minusPrice;
    public Button plusPrice;
    public InputField priceOfItem;
    public Text pricePercentage;
    public int maxItem, currentNumberItems;
    public int itemPrice, currentPriceItem, itemMaxPrice;
    public Button PutOnSale;
    public Button Cancel;
    public bool valuesReady;
    public string itemid;

	// Use this for initialization
	void Awake () {

        Cancel.onClick.RemoveAllListeners();
        Cancel.onClick.AddListener(CancelSalePrompt);

        minusAmount.onClick.RemoveAllListeners();
        minusAmount.onClick.AddListener(MinusAmountFunc);

        plusAmount.onClick.RemoveAllListeners();
        plusAmount.onClick.AddListener(PlusAmountFunc);

        minusPrice.onClick.RemoveAllListeners();
        minusPrice.onClick.AddListener(MinusPriceFunc);

        plusPrice.onClick.RemoveAllListeners();
        plusPrice.onClick.AddListener(PlusPriceFunc);

        PutOnSale.onClick.RemoveAllListeners();
        PutOnSale.onClick.AddListener(PutOnSaLeFunc);

        amountOfItemSlider.onValueChanged.AddListener(SliderFunc);

        currentNumberItems = 1;

        SalePrompt = this.gameObject;

        Cancel.onClick.RemoveAllListeners();
        Cancel.onClick.AddListener(CancelSalePrompt);
        
	}
    void SliderFunc(float value)
    {
        currentNumberItems = int.Parse(value.ToString());

        amountOfItem.text = currentNumberItems.ToString();
    }
    void MinusAmountFunc()
    {
        if (currentNumberItems <= 0)
            currentNumberItems = maxItem;
        else
            currentNumberItems--;

        amountOfItem.text = currentNumberItems.ToString();
        amountOfItemSlider.value = currentNumberItems;
    }
    void PlusAmountFunc()
    {
        if (currentNumberItems >= maxItem)
            currentNumberItems = 1;
        else
            currentNumberItems++;

        amountOfItem.text = currentNumberItems.ToString();
        amountOfItemSlider.value = currentNumberItems;
    }
    void MinusPriceFunc()
    {
        if (currentPriceItem <= 0)
            currentPriceItem = itemMaxPrice;
        else
            currentPriceItem--;

        priceOfItem.text = currentPriceItem.ToString();
    }
    void PlusPriceFunc()
    {
        if(currentPriceItem >= itemMaxPrice)
            currentPriceItem = 0;
        else
            currentPriceItem++;

        priceOfItem.text = currentPriceItem.ToString();
    }
	public static void CancelSalePrompt()
    {
        Destroy(SalePrompt);
    }
    public void PutOnSaLeFunc()
    {
        List<string> temp = new List<string>();
        temp.Add(currentNumberItems.ToString());
        temp.Add(currentPriceItem.ToString());
        //temp.Add(NetworkingWebGL.storageItems[int.Parse(itemid)][0]);
        //NetworkingWebGL.putItemOnSale(temp);

        Destroy(SalePrompt);
    }
	// Update is called once per frame
	void Update () {
        if (valuesReady)
        {

            double itemPercentage =((double)currentPriceItem / (double)itemPrice) * 100;

            pricePercentage.text = "This is <color=#00ff00ff><b>" + itemPercentage + "%</b></color> of the base value.";
            
        }
	}
}
