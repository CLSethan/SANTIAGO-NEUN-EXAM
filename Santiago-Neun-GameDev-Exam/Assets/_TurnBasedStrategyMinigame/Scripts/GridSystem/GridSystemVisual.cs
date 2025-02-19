using System.Collections.Generic;
using UnityEngine;
using NF.Main.Core;

public class GridSystemVisual : Singleton<GridSystemVisual>
{
    [SerializeField]
    private GameObject _gridVisualiserSinglePrefab;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        // Create visuals along the grid
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                GameObject gridVisualiserGO = Instantiate(_gridVisualiserSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                _gridSystemVisualSingleArray[x, z] = gridVisualiserGO.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    //hide all grid visuals
    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                
                _gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    //show all grid visuals in given list
    public void ShowAllGridPositions(List<GridPosition> gridPositionList)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
        
    }

    private void UpdateGridVisual()
    { 
        HideAllGridPositions();
        //show all selected unit's moveable positions
        BaseAction selectedAction = BaseUnitActionSystem.Instance.GetSelectedAction();
        if(selectedAction != null)
        {
            ShowAllGridPositions(selectedAction.GetValidActionGridPositionList());
        }
    }
}
