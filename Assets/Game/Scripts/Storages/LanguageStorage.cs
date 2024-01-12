using System.Collections;
using System.Collections.Generic;
using TimmyFramework;

public class LanguageStorage : IStorage
{
    private Dictionary<string, string> _languages;

    public void Initialize()
    {
        _languages = new Dictionary<string, string>();

        _languages["ground_inventory_name"] = "Ground";
        _languages["player_inventory_name"] = "Inventory";
        _languages["inner_inventory_name"] = "Outside";
        _languages["craft_label_text"] = "Craft cost:";
        _languages["craft_button_text"] = "Craft";
    }

    public string Translation(string keyWord)
    {
        if (_languages.ContainsKey(keyWord)) 
        { 
            return _languages[keyWord]; 
        }

        return keyWord;
    }
}
