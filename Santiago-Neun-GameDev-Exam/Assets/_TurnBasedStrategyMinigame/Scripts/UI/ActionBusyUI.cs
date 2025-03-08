using UnityEngine;
using NF.Main.Core;

public class ActionBusyUI : MonoExt
{

    private void Start()
    {
        Initialize();
        OnSubscriptionSet();
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void OnSubscriptionSet()
    {
        base.OnSubscriptionSet();
        AddEvent(BaseUnitActionSystem.Instance.OnBusyChanged, UnitActionSystem_OnBusyChanged);
    }

    private void UnitActionSystem_OnBusyChanged(bool isBusy)
    {
        if (isBusy) Show();
        else Hide();
    }
}
