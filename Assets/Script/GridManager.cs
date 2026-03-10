using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 6;

    public int height = 6;

    public float cellSize = 1f;

    public GameObject cellPrefab;
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize,y * cellSize,0);

                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity);

                cell.transform.parent = transform;
            }
        }
    }
}
