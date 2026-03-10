using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{

    public int width = 1;

    public int height = 1;

    public bool moveHorizontal = true;

    public bool moveVertical = false;

    Vector3 startMouse;

    bool axisLocked = false;

    bool horizontalMove;

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

        // chỉnh collider

        // cập nhật position theo grid
        UpdateWorldPosition();
    }

    public void UpdateWorldPosition()
    {
        transform.position = new Vector3(gridPos.x + width / 2f,gridPos.y + height / 2f,0);
    }

    public void Move(Vector2Int dir)
    {
        gridPos += dir;
        UpdateWorldPosition();
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

        // khóa trục khi bắt đầu kéo
        //if (!axisLocked)
        //{
        //    if (Mathf.Abs(delta.x) > 0.2f || Mathf.Abs(delta.y) > 0.2f)
        //    {
        //        horizontalMove = Mathf.Abs(delta.x) > Mathf.Abs(delta.y);
        //        axisLocked = true;
        //    }
        //}

        //if (!axisLocked)
        //    return;

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
