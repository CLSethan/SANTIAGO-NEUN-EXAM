using System.Collections.Generic;
using UnityEngine;
using NF.Main.Core;
using System;

public class GridSystemVisual : Singleton<GridSystemVisual>
{
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        Green,
        Blue,
        Red,
        RedSoft,
        Yellow,
    }

    //grid visual variables
    [SerializeField]
    private GameObject _gridVisualiserSinglePrefab;
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;
    [SerializeField] 
    private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;


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

        BaseUnitActionSystem.Instance.OnSelectedActionChanged += BaseUnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

    }

    private void BaseUnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
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
    public void ShowAllGridPositions(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
        
    }

    private void UpdateGridVisual()
    { 
        HideAllGridPositions();
        //show all selected unit's moveable positions
        BaseUnit selectedUnit = BaseUnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = BaseUnitActionSystem.Instance.GetSelectedAction();
        if(selectedAction != null)
        {
            GridVisualType gridVisualType;

            // switch grid colors based on unit action
            switch (selectedAction)
            {
                default:
                case MoveAction moveAction:
                    gridVisualType = GridVisualType.Green;
                    break;
                case SpinAction spinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
                case ShootAction shootAction:
                    gridVisualType = GridVisualType.Red;
                    //show shooting range
                    ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                    break;
            }

            ShowAllGridPositions(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        }
    }

    //show range from current grid position
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        // cycle through grid range
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                //validate positions
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //create circular range based on range
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowAllGridPositions(gridPositionList, gridVisualType);
    }


    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        // cycle through list of materials
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;
    }

}
