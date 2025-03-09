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

    private PathNode _pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        if (gridObject is PathNode node)
        {
            _pathNode = node;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_pathNode == null) return;

        _gCostText.text = _pathNode.GetGCost().ToString();
        _hCostText.text = _pathNode.GetHCost().ToString();
        _fCostText.text = _pathNode.GetFCost().ToString();
        _isWalkableSpriteRenderer.color = _pathNode.IsWalkable() ? Color.green : Color.red;

    }

}
