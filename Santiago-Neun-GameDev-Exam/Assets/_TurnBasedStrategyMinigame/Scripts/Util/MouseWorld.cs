using UnityEngine;
using NF.Main.Core;

public class MouseWorld : Singleton<MouseWorld>
{
    [SerializeField]
    private LayerMask mousePlaneLayerMask;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        transform.position = GetPosition();
    }

    public Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
