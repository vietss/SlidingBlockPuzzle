using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitGrid : MonoBehaviour
{
    public GridManager grid;
    // Start is called before the first frame update
    void Start()
    {
        FitCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FitCamera()
    {
        float gridWidth = grid.width * grid.cellSize;
        float gridHeight = grid.height * grid.cellSize;

        float screenRatio = (float)Screen.width / Screen.height;
        float gridRatio = gridWidth / gridHeight;

        Camera cam = GetComponent<Camera>();

        if (gridRatio > screenRatio)
        {
            cam.orthographicSize = gridWidth / screenRatio / 2f;
        }
        else
        {
            cam.orthographicSize = gridHeight / 2f;
        }
        cam.orthographicSize += 1;
        cam.transform.position = new Vector3(gridWidth / 2f - grid.cellSize / 2f,gridHeight / 2f - grid.cellSize / 2f,-10);
    }
}
