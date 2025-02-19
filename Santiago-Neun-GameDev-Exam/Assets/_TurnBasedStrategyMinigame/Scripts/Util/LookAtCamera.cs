using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] 
    private bool _invert;
    private Transform _cameraTransform;


    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (_invert)
        {
            Vector3 cameraDir = (_cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + cameraDir * -1);
        }
        else
        {
            transform.LookAt(_cameraTransform);
        }
    }

}
