using UnityEngine;
using TMPro;


public class GridDebugObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _gridText;
    private object _gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this._gridObject = gridObject;
    }
    protected virtual void Update()
    {
        _gridText.text = _gridObject.ToString();
    }
}
