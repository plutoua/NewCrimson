using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeSet", menuName = "ScriptableObjects/New RecipeSet", order = 4)]
public class RecipeSetScheme : ScriptableObject
{
    [SerializeField] private List<RecipeScheme> _recepts;

    public List<RecipeScheme> Recepts => _recepts;

    public void AddRecept(RecipeScheme recept)
    {
        _recepts.Add(recept);
    }

    public void RemoveRecept(RecipeScheme recept)
    {
        _recepts.Remove(recept);
    }
}
