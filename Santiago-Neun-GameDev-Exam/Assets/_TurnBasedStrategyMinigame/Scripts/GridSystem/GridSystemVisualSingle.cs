using NF.Main.Core;
using UnityEngine;

public class GridSystemVisualSingle : MonoExt
{
    [SerializeField]
    private MeshRenderer _meshRenderer;

    public void Show(Material material)
    {
        _meshRenderer.enabled = true;
        _meshRenderer.material = material;
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }
}
