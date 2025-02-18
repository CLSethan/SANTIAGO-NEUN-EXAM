using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _gridText;
    private GridObject _gridObject;
    
    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
    }

    private void Update()
    {
        _gridText.text = _gridObject.ToString();
    }

}
