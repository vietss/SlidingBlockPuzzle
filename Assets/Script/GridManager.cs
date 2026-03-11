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

    public GameObject cellExit;

    public Block[,] grid;

    public Vector2Int exitCell;
    void Awake()
    {
        Instance = this;

        grid = new Block[width, height];
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        InsCellExit();
    }
    //Spawn ra Grid
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize + cellSize / 2f, y * cellSize + cellSize / 2f, 0);

                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity);

                cell.transform.parent = transform;
            }
        }
    }
    // thêm GameObject thể hiện đó là lối thoát
    void InsCellExit()
    {
        GameObject cellExitTrans = Instantiate(cellExit, new Vector3Int(exitCell.x, exitCell.y, 0), Quaternion.identity);

        cellExitTrans.transform.position = new Vector3(cellExitTrans.transform.position.x * cellSize + cellSize, cellExitTrans.transform.position.y * cellSize + cellSize / 2f);

        cellExitTrans.transform.localScale = new Vector3(cellSize * 0.1f, cellSize, 1);
    }
    // check xem có nằm trong grid không
    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }
    // dùng để ghi lại vị trí vừa di chuyển đến 
    public void SetCell(int x, int y, Block block)
    {
        grid[x, y] = block;
    }
    // dùng để xóa những ô vừa có Block nằm ở đó
    public void ClearCell(int x, int y)
    {
        grid[x, y] = null;
    }
}
