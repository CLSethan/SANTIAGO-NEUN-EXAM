using UnityEngine;
using TMPro;
using NF.Main.Core;

public class GridDebugObject : MonoExt
{
    [SerializeField]
    private TextMeshPro _gridText;
    private object _gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }
    protected virtual void Update()
    {
        _gridText.text = _gridObject.ToString();
    }
}
