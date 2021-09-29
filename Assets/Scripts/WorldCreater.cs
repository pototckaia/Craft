using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCreater : MonoBehaviour
{
    public int WorldSize;
    public int MaxHeight;
    public GameObject Ground;
    public PlayerController Player;
    private Dictionary<Vector3Int, GameObject> world;

    void Start()
    {
        world = new Dictionary<Vector3Int, GameObject>();
        for (int x = 0; x < WorldSize; ++x)
        {
            for (int z = 0; z < WorldSize; ++z)
            {
                int h = Random.Range(1, MaxHeight);
                for (int y = 0; y < h; ++y)
                {
                    CreateGameObject(new Vector3Int(x, y, z));
                }
            }
        }
        var pos = new Vector3(WorldSize / 2, MaxHeight + 1, WorldSize / 2);
        Instantiate(Player, pos, Quaternion.identity);
    }

    public bool CreateGameObject(Vector3Int p)
    {
        if (world.ContainsKey(p))
        {
            return false;
        }
        var obj = Instantiate<GameObject>(Ground, p, Quaternion.identity);
        obj.transform.SetParent(transform);
        world[p] = obj;
        return true;
    }

    public bool DestroyGameObject(Vector3Int pos)
    {
        if (!world.ContainsKey(pos))
        {
            return false;
        }
        var obj = world[pos];
        world.Remove(pos);
        Destroy(obj);
        return true;
    }
}
