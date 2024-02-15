using System.Collections.Generic;
using UnityEngine;

public class BlockManager : Singleton<BlockManager>
{
    // ��� ������.
    Tile[] tiles;
    Block blockPrefab;

    // �� Ÿ�Ժ� ����� Ǯ���ϱ� ���� �����.
    Stack<Block> storage;
    Transform parent;

    private void Awake()
    {
        storage = new Stack<Block>();

        // ���ҽ� ���� ���� Tile ������ Texture���� ���� �ε��Ѵ�.
        blockPrefab = Resources.Load<Block>("Tile/Block");
        tiles = Resources.LoadAll<Tile>("Tile");

        GameObject newParent = new GameObject("Storage");   // type���� ���� ������Ʈ ����.
        newParent.transform.SetParent(transform);           // �ش� ������Ʈ�� �θ� ���� ����.
        newParent.SetActive(false);                         // �θ� ������Ʈ ��Ȱ��ȭ.
        parent = newParent.transform;                       // �θ� ������Ʈ ����.

        foreach(Tile tile in tiles)
            CreateBlockObject(tile.ID, 10);                   // ���ʿ� 10�� ����.
    }

    private void CreateBlockObject(string ID, int count = 1)
    {
        // �迭���� type�� �ش��ϴ� ��� �������� ã�� �����Ѵ�.
        Tile tile = System.Array.Find(tiles, p => p.ID == ID);
        for (int i = 0; i < count; i++)
        {
            Block newBlock = Instantiate(blockPrefab, parent);
            storage.Push(newBlock);
        }
    }

    public Block GetBlock(string ID)
    {
        if (storage.Count <= 0)
            CreateBlockObject(ID);

        Block block = storage.Pop();
        Tile tile = System.Array.Find(tiles, p => p.ID == ID);
        block.Setup(tile);
        block.transform.SetParent(null);
        return block;
    }
    public void ReturnBlock(Block block)
    {
        block.transform.SetParent(parent);
        storage.Push(block);
    }
}
