using TimmyFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICrafter : MonoBehaviour, UIIWindow
{
    [SerializeField] private Image _craftIcon;
    [SerializeField] private TMP_Text _craftName;
    [SerializeField] private UIIngredientList _ingredientList;
    [SerializeField] private Button _craftButton;

    public event UnityAction RecipeSetChangeEvent
    {
        add { _recipeSetChangeEvent += value; }
        remove { _recipeSetChangeEvent -= value; }
    }

    private event UnityAction _recipeSetChangeEvent;

    public RecipeSetScheme RecipeSetScheme { get; private set; }

    private RecipeScheme _activeRecipe;
    private MoveableWindow _moveableWindow;
    private UIWindowsController _windowsController;
    private InventoryController _inventoryController;
    private Crafter _crafter;

    private void Start()
    {
        _moveableWindow = GetComponentInParent<MoveableWindow>();
        _moveableWindow.CloseButtonEvent += OnCloseButton;
        _craftButton.onClick.AddListener(OnCraftButtonClick);

        if (Game.IsReady)
        {
            SetupControllers();
        }
        else
        {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    public void SetRecipeSet(RecipeSetScheme recipeSetScheme)
    {
        RecipeSetScheme = recipeSetScheme;
        DeactivateItem();
        _recipeSetChangeEvent?.Invoke();
    }

    public void SetActiveRecipe(RecipeScheme recipe)
    {
        _activeRecipe = recipe;

        var item = _activeRecipe.GetItem();

        _craftIcon.enabled = true;
        _craftIcon.sprite = item.Sprite;
        _craftName.text = item.Name + " x" + item.Amount;

        _ingredientList.SetRecipe(recipe);
        _ingredientList.CheckIngredients(_inventoryController.PlayerInventory);

        _crafter.SetRecipe(recipe);
    }

    private void DeactivateItem()
    {
        _craftIcon.enabled = false;
        _craftName.text = string.Empty;
        _ingredientList.DeactivateItems();
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        SetupControllers();
    }

    private void OnCraftButtonClick()
    {
        _crafter.Craft();
    }

    private void SetupControllers()
    {
        _windowsController = Game.GetController<UIWindowsController>();
        _windowsController.SetCrafter(this);

        _inventoryController = Game.GetController<InventoryController>();
        _inventoryController.PlayerInventory.InventoryUpdatedEvent += OnPlayerInventoryUpdated;

        _crafter = new Crafter(_inventoryController.PlayerInventory);
    }

    private void OnPlayerInventoryUpdated()
    {
        _ingredientList.CheckIngredients(_inventoryController.PlayerInventory);
    }

    private void OnCloseButton()
    {
        _windowsController.CloseWindow(this);
    }

    public void Activate()
    {
        _moveableWindow.Activate();
    }

    public void Deactivate()
    {
        _moveableWindow.Deactivate();
    }
}
