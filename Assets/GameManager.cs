using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int size;
    public string[] fruist;
    public int[] quantity;
    string saveLocation;

    [SerializeField] private Transform content;
    [SerializeField] private Text sizeText;
    [SerializeField] private GameObject FruistSlotPerfabs;
    public GameObject[] FruistSlots;
    private void Awake()
    {
        saveLocation = Application.persistentDataPath + "/save.txt";
    }
    public void CreateBasket()
    {
        if (string.IsNullOrEmpty(sizeText.text)) return;
        int size = int.Parse(sizeText.text);
        fruist = new string[size];
        quantity = new int[size];
        InitSlot(size);
        

    }

    private void InitSlot(int size)
    {
        
        foreach (var item in FruistSlots)
        {
            Destroy(item.gameObject);
        }
        FruistSlots = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            var slot = Instantiate(FruistSlotPerfabs, content);
            if (slot.TryGetComponent<FruitSlot>(out var fl))
            {
                if (!string.IsNullOrEmpty(fruist[i]))
                {
                    fl.name.text = fruist[i].ToString();
                }
                if (quantity[i] != null)
                {
                    fl.quantity.text = quantity[i].ToString();
                }

            }
            FruistSlots[i] = slot;
        }
    }

    public void SaveBasket()
    {
        FruitsBasket basket = new FruitsBasket();
        basket.size = FruistSlots.Length;
        quantity = new int[basket.size];
        fruist = new string[basket.size];
        for (int i = 0; i < basket.size; i++)
        {
            if (FruistSlots[i].TryGetComponent<FruitSlot>(out var slot))
            {
                if (!string.IsNullOrEmpty(slot.quantity.text))
                {
                    quantity[i] = int.Parse(slot.quantity.text);
                }
                else
                {
                    quantity[i] = 0;
                }
                if (!string.IsNullOrEmpty(slot.name.text))
                {
                    fruist[i] = slot.name.text;
                }
                else
                {
                    fruist[i] = "Null";
                }

            }

        }
        basket.quantity = quantity;
        basket.name = fruist;
        string json = JsonUtility.ToJson(basket);
        File.WriteAllText(saveLocation, json);
        Debug.Log(saveLocation);
    }
    public void LoadBasket()
    {
        if (!File.Exists(saveLocation)) return;

        string json = File.ReadAllText(saveLocation);
        Debug.Log(json);
        FruitsBasket basket = new FruitsBasket();
        basket = JsonUtility.FromJson<FruitsBasket>(json);
        size = basket.size;
        fruist = basket.name;
        Debug.Log(fruist);
        quantity = basket.quantity;

        InitSlot(size);

    }
    public void QuitApp()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class FruitsBasket
{
    public int size;
    public string[] name;
    public int[] quantity;
}
