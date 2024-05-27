using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabInventry : MonoBehaviour
{
    public static PlayFabInventry Instance;
    public List<ItemInstance> userInventry;
    public List<Item> AllItems;
    public List<CatalogItem> CatalogItems { get; private set; }
    private bool Inventorystop;
    public Inventry Inventory;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Inventorystop = true;
    }

    public void GetCatalogData(string catalogVersion)
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = catalogVersion,
        }
        , result =>
        {
            Debug.Log("�J�^���O�f�[�^�擾����");
            CatalogItems = result.Catalog;

            GetUserInventory();
        }
        , error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    //�C���x���g���̏����擾
    public void GetUserInventory()
    {
        Inventory.AllClear();
        var userInventoryRequest = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest
        {
        }, result =>
         {
             Debug.Log($"�C���x���g���̏��̎擾�ɐ��� : �C���x���g���ɓ����Ă���A�C�e���� {result.Inventory.Count}��");
             Dictionary<string, int> itemCounts = new Dictionary<string, int>();
             foreach (ItemInstance item in result.Inventory)
             {
                 string itemId = item.DisplayName;
                 Debug.Log($"ID : {item.ItemId}, Name : {item.DisplayName}, ItemInstanceId : {item.ItemInstanceId}");
                 if (itemCounts.ContainsKey(itemId))
                 {
                     itemCounts[itemId]++;

                 }
                 else
                 {
                     itemCounts[itemId] = 1;
                     Inventorystop = true;
                 }
                 GetItemDescription(item.ItemId,item.DisplayName,item.ItemInstanceId,itemCounts[itemId]);
            }
         }, error =>
         {
             Debug.LogError($"�C���x���g���̏��̎擾�Ɏ��s");
         });
    }

    private void GetItemDescription(string itemId,string ItemName, string itemInstanceId,int itemCounts)
    {
            // �A�C�e���̃J�^���O�����擾���郊�N�G�X�g���쐬
            CatalogItem targetItem = CatalogItems.Find(item => item.ItemId == itemId);
            // PlayFab�N���C�A���g�Ń��N�G�X�g�����s

            if (targetItem != null)
            {
                // �A�C�e���̐��������擾���ĕ\��
                string description = targetItem.Description;
                Debug.Log("Item Description: " + description);

                // �A�C�e���̏���n��
                Find(itemId, ItemName, itemInstanceId, itemCounts, description);
            }
            else
            {
                Debug.LogError("Item with ID " + itemId + " not found in catalog.");
            }
        }

    public  void Find(string itemName,string itemDisplayName,string itemID,int itemCount,string Descriotion)
    {
        var matchingItem = AllItems.Find(item => item.name == itemDisplayName);
        var guids = UnityEditor.AssetDatabase.FindAssets(itemName);
        Debug.Log("t:" + itemName);
        if (matchingItem == null)
        {
            throw new System.IO.FileNotFoundException("������܂���ł���");
        }
        var obj = matchingItem;

        Debug.Log(obj.name);
        obj.ItemID = itemID;
        obj.name = itemDisplayName;
        obj.itemCounts = itemCount;
        obj.text = Descriotion;

        if (Inventorystop)
        {
            Debug.Log("in");
            Inventory.Add(obj);
            Inventorystop = false;
        }
    }

    public void ConSumeItem(string ID)
    {
        Debug.Log(ID);

        var consumeItemRequest = new ConsumeItemRequest
        {
            ItemInstanceId = ID,
            ConsumeCount = 1,
        };
        Debug.Log("�A�C�e������");
        PlayFabClientAPI.ConsumeItem(consumeItemRequest, OnSuccess, OnError);;
    }

    private void OnSuccess(ConsumeItemResult result)
    {
        Debug.Log($"�C���x���g���̃A�C�e��({result.ItemInstanceId}�̏���ɐ���");
        GetUserInventory();
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError($"�C���x���g���̃A�C�e���̏���Ɏ��s");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}