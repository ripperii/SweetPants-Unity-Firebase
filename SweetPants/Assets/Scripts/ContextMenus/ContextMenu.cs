using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu: MonoBehaviour
{

    public static GameObject InstantiateContextMenu(Item it, GameObject prefab, Transform parent)
    {
        GameObject context = Instantiate(prefab, parent);

        GameObject[] prefabs = context.GetComponent<ContextMenuScript>().prefabs;

        // Inspect
        Button inspect = Instantiate(prefabs[0], context.transform.GetChild(0)).GetComponent<Button>();
        inspect.onClick.AddListener(() => InspectItem(it, parent));
        inspect.onClick.AddListener(() => { Destroy(context); });
        // Put on Showcase
        Button showcase = Instantiate(prefabs[1], context.transform.GetChild(0)).GetComponent<Button>();
        showcase.onClick.AddListener(() => PutOnShowcase(it, parent)); ;
        showcase.onClick.AddListener(() => { Destroy(context); });

        if (it.equipable)
        {
            // Equip
            Button equip = Instantiate(prefabs[2], context.transform.GetChild(0)).GetComponent<Button>();
            equip.onClick.AddListener(() => EquipItem(it));
            equip.onClick.AddListener(() => { Destroy(context); });
        }

        ContextMenuPosition.SetPivot(context.transform.GetChild(0).GetComponent<RectTransform>());

        Debug.Log(Input.mousePosition);

        return context;
    }

    public static GameObject InstantiateInfoContext(Item it, GameObject prefab, Transform parent)
    {
        Transform go = Instantiate(prefab, parent).GetComponent<Transform>();
        GameObject[] prefabs = go.GetComponent<InfoContextMenuScript>().prefabs;

        Transform main = Instantiate(prefabs[0], go).GetComponent<Transform>();

        main.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(it.icon);

        Image bg = main.GetChild(0).GetComponent<Image>();
        Text name = main.GetChild(1).GetComponent<Text>();

        name.text = it.name;
        name.color = bg.color = ItemRarityColors.colors.Where(x => x.Key.ToLower() == it.rarity.ToLower()).FirstOrDefault().Value;

        main.GetChild(2).GetComponent<Text>().text = it.type;
        main.GetChild(3).GetComponent<Text>().text = it.subtype;

        if (null != it.description && it.description != "")
        {
            Instantiate(prefabs[1], go).GetComponent<Text>().text = it.description;
        }
        if (null != it.itemStats && it.itemStats.Count > 0)
        {
            string stats = "";
            foreach (var st in it.itemStats)
            {
                stats += st.name + ": " + st.value + "\n";
            }
            Instantiate(prefabs[2], go).GetComponent<Text>().text = stats;
        }

        ContextMenuPosition.SetPivot(go.GetComponent<RectTransform>());

        return go.gameObject;
    }

    public static void PutOnShowcase(Item it, Transform parent)
    {
        ShowcasePopupScript sps = Instantiate(Resources.Load<GameObject>("Prefabs/ShowcasePopup"), parent).GetComponent<ShowcasePopupScript>();

        sps.OpenShowcasePopup(it, Player.currentPlayer.Inventory[it]);
        /*
        DatabaseManager.sharedInstance.GetShowcaseItems(result =>
        {
            int count = result.Count;

            ShowcaseItem sci = new ShowcaseItem(it, count.ToString(), price, currency);

            DatabaseManager.sharedInstance.UpdateShowcaseItem(sci, )
        });*/
    }
    public static void EquipItem(Item it)
    {

    }
    public static void InspectItem(Item it, Transform parent)
    {
        InstantiateInfoContext(
            it, Resources.Load<GameObject>("Prefabs/ContextMenu/InfoContext/InfoContext"), 
            Instantiate(Resources.Load<GameObject>("Prefabs/ContextMenu/InfoContext/background"),
                parent).AddComponent<DestroyContextMenu>().transform);
    }
}
