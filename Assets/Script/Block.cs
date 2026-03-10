using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{

    public int width = 1;

    public int height = 1;

    Vector3 startMouse;

    public Vector2Int gridPos;

    // Start is called before the first frame update
    void Start()
    {
        SetupBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetupBlock()
    {
        // scale sprite theo grid
        transform.localScale = new Vector3(width, height, 1);
        // cập nhật position theo grid
        UpdateWorldPosition();
    }

    public void UpdateWorldPosition()
    {
        float cell = GridManager.Instance.cellSize;

        transform.position = new Vector3(gridPos.x * cell + width * cell / 2f,gridPos.y * cell + height * cell / 2f,0);
    }

    public void Move(Vector2Int dir)
    {
        Vector2Int newPos = gridPos + dir;

        if (!CanMove(newPos))
            return;

        ClearCells();

        gridPos = newPos;

        OccupyCells();

        UpdateWorldPosition();
    }
    bool CanMove(Vector2Int pos)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int checkX = pos.x + x;
                int checkY = pos.y + y;

                if (!GridManager.Instance.IsInsideGrid(checkX, checkY))
                    return false;

                Block other = GridManager.Instance.grid[checkX, checkY];

                if (other != null && other != this)
                    return false;
            }
        }

        return true;
    }

    void OccupyCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridManager.Instance.SetCell(gridPos.x + x, gridPos.y + y, this);
            }
        }
    }

    void ClearCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridManager.Instance.ClearCell(gridPos.x + x, gridPos.y + y);
            }
        }
    }

    
    void OnMouseDown()
    {
        startMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //axisLocked = false;
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 delta = currentMousePos - startMouse;

        Vector2Int dir = Vector2Int.zero;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // di chuyển ngang
            if (delta.x > 0.5f)
                dir = Vector2Int.right;

            else if (delta.x < -0.5f)
                dir = Vector2Int.left;
        }
        else
        {
            // di chuyển dọc
            if (delta.y > 0.5f)
                dir = Vector2Int.up;

            else if (delta.y < -0.5f)
                dir = Vector2Int.down;
        }


        if (dir != Vector2Int.zero)
        {
            Move(dir);

            startMouse = currentMousePos;
        }
    }

    void OnMouseUp()
    {
        //axisLocked = false; 
    }
}
