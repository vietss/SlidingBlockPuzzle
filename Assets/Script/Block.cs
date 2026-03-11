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

    public bool isTarget = false;

    public MoveType moveType = MoveType.Free;


    // Start is called before the first frame update
    void Start()
    {
        RandomColor();
        SetupBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // hàm random màu sắc cho Block
    void RandomColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(!isTarget)
        {
            float h = Random.Range(0f, 1f);

            while (h < 0.05f || h > 0.95f)
            {
                h = Random.Range(0f, 1f);
            }

            float s = Random.Range(0.6f, 1f);
            float v = Random.Range(0.7f, 1f);

            sr.color = Color.HSVToRGB(h, s, v);
        }
        else
            sr.color = Color.red;


    }    
    public void SetupBlock()
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
        if (moveType == MoveType.Horizontal && dir.y != 0)
            return;

        if (moveType == MoveType.Vertical && dir.x != 0)
            return;

        Vector2Int newPos = gridPos + dir;

        if (!CanMove(newPos))
            return;

        ClearCells();

        gridPos = newPos;

        OccupyCells();

        UpdateWorldPosition();

        CheckWin();
    }
    // check xem có thể di chuyển được không
    bool CanMove(Vector2Int pos)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int checkX = pos.x + x;
                int checkY = pos.y + y;

                Debug.Log("x: " + checkX + " " + GridManager.Instance.exitCell.x + " y " + checkY + " " + (GridManager.Instance.exitCell.y + 0.5f));
                

                if (!GridManager.Instance.IsInsideGrid(checkX, checkY))
                {
                    if (isTarget && checkX >= GridManager.Instance.exitCell.x && checkY == GridManager.Instance.exitCell.y)
                        continue;

                    return false;
                }

                Block other = GridManager.Instance.grid[checkX, checkY];

                if (other != null && other != this)
                    return false;
            }
        }

        return true;
    }
    // ghi lại vị trí mới
    public void OccupyCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int gx = gridPos.x + x;
                int gy = gridPos.y + y;

                if (GridManager.Instance.IsInsideGrid(gx, gy))
                {
                    GridManager.Instance.SetCell(gx, gy, this);
                }
            }
        }
    }

    // xóa vị trí cũ của Block
    void ClearCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int gx = gridPos.x + x;
                int gy = gridPos.y + y;

                if (GridManager.Instance.IsInsideGrid(gx, gy))
                {
                    GridManager.Instance.ClearCell(gx, gy);
                }
            }
        }
    }
    // check xem user win chưa
    void CheckWin()
    {
        if (!isTarget) return;

        if (gridPos.x >= GridManager.Instance.width)
        {
            Time.timeScale = 0;
            GameManager.Instance.TextWin.gameObject.SetActive(true);
            Debug.Log("YOU WIN!");
        }
    }
    void OnMouseDown()
    {
        if (Time.timeScale == 0) return;
        startMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //axisLocked = false;
    }

    void OnMouseDrag()
    {
        if (Time.timeScale == 0) return;
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

    public enum MoveType
    {
        Horizontal,
        Vertical,
        Free
    }

}
