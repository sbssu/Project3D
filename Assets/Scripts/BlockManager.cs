using System.Collections.Generic;
using UnityEngine;

public class BlockManager : Singleton<BlockManager>
{
    // 블록 프리팹.
    Tile[] tiles;
    Block blockPrefab;

    // 각 타입별 블록을 풀링하기 위한 저장소.
    Stack<Block> storage;
    Transform parent;

    private void Awake()
    {
        storage = new Stack<Block>();

        // 리소스 폴더 내부 Tile 폴더의 Texture들을 전부 로드한다.
        blockPrefab = Resources.Load<Block>("Tile/Block");
        tiles = Resources.LoadAll<Tile>("Tile");

        GameObject newParent = new GameObject("Storage");   // type명을 가진 오브젝트 생성.
        newParent.transform.SetParent(transform);           // 해당 오브젝트의 부모를 나로 지정.
        newParent.SetActive(false);                         // 부모 오브젝트 비활성화.
        parent = newParent.transform;                       // 부모 오브젝트 참조.

        foreach(Tile tile in tiles)
            CreateBlockObject(tile.ID, 10);                   // 최초에 10개 생성.
    }

    private void CreateBlockObject(string ID, int count = 1)
    {
        // 배열에서 type에 해당하는 블록 프리팹을 찾아 생성한다.
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
