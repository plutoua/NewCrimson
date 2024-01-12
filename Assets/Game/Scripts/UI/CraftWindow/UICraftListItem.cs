using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICraftListItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _amount;

    [Header("Background")]
    [SerializeField] private Sprite _defaultIcon;
    [SerializeField] private Sprite _activeIcon;

    public RecipeScheme Recipe => _recipe;

    private RecipeScheme _recipe;
    private UICraftList _controller;

    private bool _isActive;

    private void Start()
    {
        _isActive = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MakeActive();
    }

    public void SetRecipe(RecipeScheme recipe)
    {
        _recipe = recipe;

        var item = _recipe.GetItem();
        _icon.sprite = item.Sprite;
        _name.text = item.Name;
        _amount.text = item.Amount.ToString();
    }

    public void SetController(UICraftList controller)
    {
        _controller = controller;
    }

    public void MakeNonActive()
    {
        if (_isActive)
        {
            _isActive = false;
            _background.sprite = _defaultIcon;
        }
    }

    private void MakeActive()
    {
        if (!_isActive)
        {
            _isActive = true;
            _background.sprite = _activeIcon;
            _controller.SetActiveItem(this);
        }
    }
}
