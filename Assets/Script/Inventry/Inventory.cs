using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    InventoryUI inventryUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        inventryUI = GetComponent<InventoryUI>();
    }

    public List<Item> items = new List<Item>();

    //�C���x���g���ɃA�C�e���X���b�g�ǉ�
    public void Add(List<Item> item)
    {
        Debug.Log("Add");
        items = item; 
        inventryUI.UpdateUI(items);
    }
    
    public void Remove(Item item)
    {
        items.Remove(item);
        inventryUI.UpdateUI(items);
    }

    //�C���x���g�����Z�b�g
    public void AllClear()
    {
        Debug.Log("�C���x���g�����Z�b�g");
        items.Clear();
        inventryUI.UpdateUI(items);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}