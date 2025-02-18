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
    private PlayerUnit _unit;
   

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    GridSystemVisual.Instance.HideAllGridPosition();
        //    GridSystemVisual.Instance.ShowAllGridPosition(_unit.GetMoveAction().GetValidActionGridPositionList());
        //}

    }

   
}
