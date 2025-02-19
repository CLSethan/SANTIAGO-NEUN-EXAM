using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    [SerializeField]
    private Button _button;
    [SerializeField]
    private GameObject _selectedGameObject;

    private BaseAction _baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        this._baseAction = baseAction;
        // get action name
        _textMeshPro.text = baseAction.GetActionName().ToUpper();

        //create anonymous function for action behaviour
        _button.onClick.AddListener(() =>
        {
            BaseUnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = BaseUnitActionSystem.Instance.GetSelectedAction();
        _selectedGameObject.SetActive(selectedBaseAction == _baseAction);
    }
}
