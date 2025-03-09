using System;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private TGridObject[,] _gridObjectArray;

   public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridObjectArray = new TGridObject[width, height];

        //cycle through width and height
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                //create and store gridobjects
                _gridObjectArray[x, z] =  createGridObject(this, gridPosition);
            }
        }
    }

    //convert grid position to world position
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    //convert world position to grid position
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
    }

    // Create debug objects for each grid cell
    public void CreateDebugObjects(GameObject debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                GameObject debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    // get the grid object at a given grid position
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    // check if selected grid position is within bounds
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.z >= 0 && 
            gridPosition.x < _width && gridPosition.z < _height;
    }
    public int GetWidth() => _width;
    public int GetHeight() => _height;
}
