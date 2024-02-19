using UnityEngine;

[CreateAssetMenu(fileName = "ItemDB", menuName = "Item/ItemDB")]
public class ItemDB : ScriptableObject
{
    static ItemDB instance;
    public static ItemDB Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<ItemDB>("Scriptable/ItemDB");

            return instance;
        }
    }


    [SerializeField] ItemData[] itemDatas;
    
    public ItemData GetItemData(string id)
    {
        ItemData itemData = System.Array.Find(itemDatas, item => item.ID == id);
        if (itemData == null)
        {
            Debug.Log($"not valid item code : {id}");
            return null;
        }
        return itemData;
    }

#if UNITY_EDITOR
    [ContextMenu("Load Item Data")]
    private void LoadItemData()
    {        
        itemDatas = Resources.LoadAll<ItemData>("ItemData");        // 에디터 상에서 로드
        UnityEditor.EditorUtility.SetDirty(this);                   // 변경점 기록
        UnityEditor.AssetDatabase.SaveAssets();                     // 에셋 저장 (변경점이 있는 것들)
    }
#endif


    /*
    const string url = "https://docs.google.com/spreadsheets/d/1q9e8nMh5ACeoqvDYSboAZtUrlQr1BNv5RsgWjQrnE90/export?format=csv";
    public Item GetItem(string id)
    {
        Item item = System.Array.Find(items, item => item.id == id);
        if(item == null)
        {
            Debug.Log($"not valid item code : {id}");
            return null;
        }

        return item.Copy();
    }
    public ItemData GetTile(string id)
    {
        ItemData tile = System.Array.Find(tiles, tile => tile.ID == id);
        if(tile == null)
        {
            Debug.Log($"not valid item code : {id}");
            return null;
        }
        return tile;
    }


    [ContextMenu("Update ItemDB")]
    void UpdateItemDB()
    {
        ItemData();
        TileData();
    }
    async void ItemData()
    {
        Debug.Log($"Try web request : {url}");
        UnityWebRequest web = UnityWebRequest.Get(url);
        web.SendWebRequest();
        while (!web.isDone)
            await Task.Yield();

        if (web.result == UnityWebRequest.Result.Success)
        {
            string csv = web.downloadHandler.text;
            string[] csvs = csv.Split('\n');

            items = new Item[csvs.Length - 1];
            for (int i = 1; i < csvs.Length; i++)
            {
                Debug.Log(csvs[i]);
                items[i - 1] = new Item(csvs[i]);
            }
        }
        else
        {
            Debug.Log($"Faild : {web.error}");
        }
    }
    */

}
