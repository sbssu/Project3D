using UnityEngine;

public class Block : MonoBehaviour
{
    MeshRenderer[] planes;

    public string ID { get; private set; }

    public void Setup(Tile tile)
    {
        if (planes == null)
            planes = GetComponentsInChildren<MeshRenderer>();

        tile.ToBlock(planes);
    }
}
