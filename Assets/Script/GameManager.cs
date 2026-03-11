using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Block;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject blockPrefab;

    public TextMeshProUGUI TextWin;

    int[,] level =
    {
        {1,1,3,0,0,0},
        {0,0,3,0,0,0},
        {4,4,3,0,0,0},
        {0,0,0,5,5,0},
        {0,2,0,5,5,0},
        {0,0,0,0,0,0}
    };
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnLevel();

    }
    // Update is called once per frame
    void Update()
    {
        
    }


    void SpawnLevel()
    {
        int rows = level.GetLength(0);
        int cols = level.GetLength(1);

        Dictionary<int, List<Vector2Int>> blocks = new Dictionary<int, List<Vector2Int>>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int id = level[rows - 1 - y, x]; // đảo trục Y

                if (id == 0)
                    continue;

                if (!blocks.ContainsKey(id))
                    blocks[id] = new List<Vector2Int>();

                blocks[id].Add(new Vector2Int(x, y));
            }
        }

        foreach (var pair in blocks)
        {
            CreateBlock(pair.Key, pair.Value);
        }
    }

    void CreateBlock(int id, List<Vector2Int> cells)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (var c in cells)
        {
            minX = Mathf.Min(minX, c.x);
            minY = Mathf.Min(minY, c.y);
            maxX = Mathf.Max(maxX, c.x);
            maxY = Mathf.Max(maxY, c.y);
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        GameObject obj = Instantiate(blockPrefab);

        Block block = obj.GetComponent<Block>();

        block.width = width;
        block.height = height;
        block.gridPos = new Vector2Int(minX, minY);

        if (id == 1)
        {
            block.isTarget = true;
            block.moveType = MoveType.Free;
        }
        else if (id == 3 || id == 5)
        {
            block.moveType = MoveType.Horizontal;
        }
        else if (id == 4 || id == 2)
        {
            block.moveType = MoveType.Vertical;
        }

        block.SetupBlock();
        block.OccupyCells();
    }
}
