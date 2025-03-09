using System;
using UnityEngine;
using UniRx;
using NF.Main.Core;

public class CameraManager : MonoExt
{
    [SerializeField]
    private GameObject _actionCameraGameObject;
    [SerializeField]
    private float _characterHeight = 1.7f;
    [SerializeField]
    private float _shoulderOffset = 0.5f;

    private void Start()
    {
        Initialize();
        OnSubscriptionSet();
        HideActionCamera();
    }

    public override void OnSubscriptionSet()
    {
        AddEvent(BaseAction.OnAnyActionStarted, OnActionStarted);
        AddEvent(BaseAction.OnAnyActionCompleted, OnActionCompleted);
    }

    private void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
    }

    private void OnActionStarted(BaseAction action)
    {
        if (action is ShootAction shootAction)
        {
            // Get shooter and target
            BaseUnit shooterUnit = shootAction.GetUnit();
            BaseUnit targetUnit = shootAction.GetTargetUnit();

            // Adjust for character height
            Vector3 cameraCharacterHeight = Vector3.up * _characterHeight;
            Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

            // Create shoulder offset
            Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * _shoulderOffset;
            Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

            // Move action camera
            _actionCameraGameObject.transform.position = actionCameraPosition;
            _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

            // Show camera
            ShowActionCamera();
        }
    }

    private void OnActionCompleted(BaseAction action)
    {
        if (action is ShootAction)
        {
            HideActionCamera();
        }
    }
}
