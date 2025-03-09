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
    [SerializeField] 
    private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();

        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[width, height];

        // Create visuals along the grid
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject gridVisualiserGO = Instantiate(_gridVisualiserSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                _gridSystemVisualSingleArray[x, z] = gridVisualiserGO.GetComponent<GridSystemVisualSingle>();
            }
        }

        OnSubscriptionSet();
    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();

        // Subscribe to events using MonoExt's AddEvent method
        AddEvent(LevelGrid.Instance.OnAnyUnitMovedGridPosition, _ => UpdateGridVisual());
        AddEvent(BaseUnitActionSystem.Instance.OnSelectedActionChanged, _ => UpdateGridVisual());

    }

    //hide all grid visuals
    public void HideAllGridPositions()
    {
        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                _gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    //show all grid visuals in given list
    public void ShowAllGridPositions(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        Material material = GetGridVisualTypeMaterial(gridVisualType);

        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(material);
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
