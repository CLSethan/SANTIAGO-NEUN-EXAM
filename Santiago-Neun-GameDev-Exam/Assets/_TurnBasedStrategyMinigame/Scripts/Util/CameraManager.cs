using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject _actionCameraGameObject;
    [SerializeField]
    private float _characterHeight = 1.7f;
    [SerializeField]
    float _shoulderOffset = 0.5f;

    private void Start()
    {
        //subscribe to events
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        // switch on action
        switch (sender)
        {

            case ShootAction shootAction:

                //get shooter and target
                BaseUnit shooterUnit = shootAction.GetUnit();
                BaseUnit targetUnit = shootAction.GetTargetUnit();

                //adjust for character height
                Vector3 cameraCharacterHeight = Vector3.up * _characterHeight;
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                //create offset
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * _shoulderOffset;
                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

                //move action camera and adjust for character height
                _actionCameraGameObject.transform.position = actionCameraPosition;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                //show camera
                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

}
