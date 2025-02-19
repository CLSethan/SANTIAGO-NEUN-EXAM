using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{

    [SerializeField] 
    private TextMeshPro _gCostText;
    [SerializeField] 
    private TextMeshPro _hCostText;
    [SerializeField] 
    private TextMeshPro _fCostText;
    [SerializeField] 
    private SpriteRenderer _isWalkableSpriteRenderer;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        _gCostText.text = pathNode.GetGCost().ToString();
        _hCostText.text = pathNode.GetHCost().ToString();
        _fCostText.text = pathNode.GetFCost().ToString();
        _isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;

    }

}
