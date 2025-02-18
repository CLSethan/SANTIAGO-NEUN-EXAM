using UnityEngine;

public class PlayerUnit : MonoBehaviour
{

    private Vector3 _targetPosition;

    //movement variables
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _stoppingDistance;

    // animation variables
    [SerializeField]
    private Animator _unitAnimator;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    void Update()
    {
        // if distance between current position and target position is further than stopping position, move to target position.
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;

            //smooth rotation
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, _rotateSpeed * Time.deltaTime);
            _unitAnimator.SetBool("isWalking", true);
        }

        else
        {
            _unitAnimator.SetBool("isWalking", false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }
}
