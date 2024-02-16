using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "ItemDB", menuName = "Item/ItemDB")]
public class ItemDB : ScriptableObject
{
    [SerializeField] Item[] items;

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

    [ContextMenu("Update ItemDB")]
    async void UpdateItemDB()
    {
        Debug.Log($"Try web request : {url}");
        UnityWebRequest web = UnityWebRequest.Get(url);
        web.SendWebRequest();
        while(!web.isDone)
            await Task.Yield();

        if(web.result == UnityWebRequest.Result.Success)
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
}
