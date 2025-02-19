using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    //Test Grid
    /*
    [SerializeField]
    private GameObject _gridDebugObjectPrefab;

    private GridSystem _gridSystem;
    void Start()
    {
        _gridSystem = new GridSystem(10, 10, 2);
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);

    }

    private void Update()
    {
        Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
    */

    //test unit
    
    [SerializeField]
    private BaseUnit _unit;
   

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    GridSystemVisual.Instance.HideAllGridPosition();
        //    GridSystemVisual.Instance.ShowAllGridPosition(_unit.GetMoveAction().GetValidActionGridPositionList());
        //}
        if (Input.GetKeyDown(KeyCode.T))
        {
            //GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            //GridPosition startGridPosition = new GridPosition(0, 0);

            //List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            //for (int i = 0; i < gridPositionList.Count - 1; i++)
            //{
            //    Debug.DrawLine(
            //        LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
            //        LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
            //        Color.white,
            //        10f
            //    );
            //}

        }

    }


}
