using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
// using System.Diagnostics;

[CreateAssetMenu]
public class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor>
{
    [Header("Advanced Tile")]
    [Tooltip("If enabled, the tile will connect to these tiles too when the mode is set to \"This\"")]
    public bool alwaysConnect;
    [Tooltip("Tiles to connect to")]
    public CustomRuleTile[] tilesToConnect;
    public int type;
    [Space]
    [Tooltip("Check itseft when the mode is set to \"any\"")]
    public bool checkSelf = true;
    public bool first_match;

    [Header("Tile Settings")]
    public string mapName = "Map1";  // Назва карти

    private Sprite[,] rotationSprites;  // Масив для зберігання спрайтів

    void OnEnable()
    {
        LoadRotationSprites();
    }

    void LoadRotationSprites()
    {
        rotationSprites = new Sprite[3, 4];  // 3 ассети, 4 повороти для кожного (4 кутові, 4 дальні кутові, 4 центральні)
        string basePath = $"Sprites/{mapName}/Rocks/angled_asset_";
        Debug.Log("WE ARE HERE");

        for (int i = 0; i < 3; i++) // Індекс ассету
        {
            for (int j = 0; j < 4; j++) // Індекс повороту
            {
                int rotation = j * 90; // Кут повороту
                rotationSprites[i, j] = Resources.Load<Sprite>($"{basePath}{i + 1}_{rotation}");
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        List<CustomRuleTile> matchingTiles = new List<CustomRuleTile>();

        foreach (CustomRuleTile ruleTile in tilesToConnect)
        {
            if (ruleTile!=null)
            {
                matchingTiles.Add(ruleTile);
            }
        }

        // Якщо є підходящі тайли, вибираємо один випадково
        if (matchingTiles.Count > 0)
        {   
            int randomIndex = Random.Range(0, matchingTiles.Count);
            CustomRuleTile randomTile = matchingTiles[randomIndex];
            randomTile.GetTileData(position, tilemap, ref tileData);
        }
        if (type == 1) { 
            tileData.sprite = ChooseRotationSprite(position, tilemap);
        }
    }

    /*Sprite ChooseRotationSprite(Vector3Int position, ITilemap tilemap)
    {
        // Перевірка наявності сусідніх тайлів
        bool hasLeft = tilemap.GetTile(position + new Vector3Int(-1, 0, 0)) != null;
        bool hasRight = tilemap.GetTile(position + new Vector3Int(1, 0, 0)) != null;
        bool hasTop = tilemap.GetTile(position + new Vector3Int(0, 1, 0)) != null;
        bool hasBottom = tilemap.GetTile(position + new Vector3Int(0, -1, 0)) != null;



        // Визначення повороту на основі позиції сусідніх тайлів
        if (hasLeft) return rotationSprites[1];  // 90 градусів
        if (hasBottom) return rotationSprites[2];  // 180 градусів
        if (hasRight) return rotationSprites[3];  // 270 градусів
        if (hasTop) return rotationSprites[0];  // 0 градусів

        // Якщо немає сусідів, повертаємо спрайт за замовчуванням
        return rotationSprites[0];  // Приклад
    }*/

    Sprite ChooseRotationSprite(Vector3Int position, ITilemap tilemap)
    {
        CustomRuleTile currentTile = tilemap.GetTile<CustomRuleTile>(position);
        if (currentTile == null) return null; // Перевіряємо, чи поточний тайл є CustomRuleTile

        // Функція для перевірки сусіднього тайлу
        bool IsSameTypeNeighbor(Vector3Int offset)
        {
            CustomRuleTile neighborTile = tilemap.GetTile<CustomRuleTile>(position + offset);
            return neighborTile != null && neighborTile.type == currentTile.type;
        }

        // Перевірка наявності сусідніх тайлів того ж типу
        bool hasLeft = IsSameTypeNeighbor(new Vector3Int(-1, 0, 0));
        bool hasRight = IsSameTypeNeighbor(new Vector3Int(1, 0, 0));
        bool hasTop = IsSameTypeNeighbor(new Vector3Int(0, 1, 0));
        bool hasBottom = IsSameTypeNeighbor(new Vector3Int(0, -1, 0));

        // Індекс правила - змініть цей індекс відповідно до вашої логіки або налаштувань
        int ruleIndex = 0; // Приклад індексу правила (// 3 ассети)

        int conditionIndex = 0; // Приклад індексу кута (0-360, крок 90, всього 4)


        if (hasLeft && hasRight && hasBottom && hasTop) { 
            // blank center
        }

        else if(hasLeft && hasRight && hasBottom)
        {
            // up one sided
            return rotationSprites[ruleIndex, conditionIndex];
        }

        else if (hasLeft && hasRight && hasTop && hasBottom)
        {
            // down one sided
            return rotationSprites[ruleIndex, conditionIndex];
        }

        else if(hasLeft && hasTop && hasBottom)
        {
            // right one sided
            return rotationSprites[ruleIndex, conditionIndex];
        }

        else if(hasRight && hasTop && hasBottom)
        {
            // left one sided
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasTop && hasBottom)
        {
            // two sided hirisontal
            return rotationSprites[ruleIndex, conditionIndex];
        }

        else if (hasRight && hasLeft)
        {
            // two sided vertical
            return rotationSprites[ruleIndex, conditionIndex];
        }

        else if (hasTop && hasLeft)
        {
            // two angled down right
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasTop && hasRight)
        {
            // two angled down left
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasBottom && hasLeft)
        {
            // two angled up right
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasBottom && hasRight)
        {
            // two angled up left
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasBottom)
        {
            // one angled up
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasRight)
        {
            // one angled left
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasLeft)
        {
            // one angled right
            return rotationSprites[ruleIndex, conditionIndex];
        }
        else if (hasTop)
        {
            // one angled down
            return rotationSprites[ruleIndex, conditionIndex];
        }
        

        // Для інших умов...
        // 

        // 4 sided

        // Якщо немає сусідів, повертаємо спрайт за замовчуванням
        // Переробити на оригінальний спрайт з правил, якщо правило не прокнуло.
        return rotationSprites[ruleIndex, 0]; // Приклад
    }


    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int This = 1;
        public const int NotThis = 2;
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
    }

    public void SetConnected(CustomRuleTile[] tilesToConnect){
        if (this.tilesToConnect != null && this.tilesToConnect.Length < 1){
            this.tilesToConnect = tilesToConnect;
        }
        first_match = true;
    }

    private void MakeConnect(){
        Debug.Log("MakeConnect");
        if (tilesToConnect != null){
            foreach (CustomRuleTile ruleTile in tilesToConnect)
            {   
                if (ruleTile!=null && ruleTile != this)
                {
                    ruleTile.SetConnected(tilesToConnect);
                }
            }
        }
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        if (!first_match){
            MakeConnect();
            first_match = true;
        }
        switch (neighbor) {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Any: return Check_Any(tile);
            case Neighbor.Specified: return Check_Specified(tile);
            case Neighbor.Nothing: return Check_Nothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    /// <summary>
    /// Returns true if the tile is this, or if the tile is one of the tiles specified if always connect is enabled.
    /// </summary>
    /// <param name="tile">Neighboring tile to compare to</param>
    /// <returns></returns>
    bool Check_This(TileBase tile)
    {
        CustomRuleTile customTile = tile as CustomRuleTile;
        if (customTile != null)
        {
            // If the type of customTile is the same as this CustomRuleTile's type, return false
            if (customTile.type == this.type)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true if the tile is not this.
    /// </summary>
    /// <param name="tile">Neighboring tile to compare to</param>
    /// <returns></returns>
    bool Check_NotThis(TileBase tile)
    {
        //if (!alwaysConnect) return tile != this;
        //else return !tilesToConnect.Contains(tile) && tile != this;
        CustomRuleTile customTile = tile as CustomRuleTile;
        if (customTile != null)
        {
            // If the type of customTile is the same as this CustomRuleTile's type, return false
            if (customTile.type == this.type)
            {
                return false;
            }
        }
        return true;
        // return !(tile is CustomRuleTile);
        // return !tilesToConnect.Contains(tile) && tile != this;
        //.Contains requires "using System.Linq;"
    }

    /// <summary>
    /// Return true if the tile is not empty, or not this if the check self option is disabled.
    /// </summary>
    /// <param name="tile">Neighboring tile to compare to</param>
    /// <returns></returns>
    bool Check_Any(TileBase tile)
    {
        // if (checkSelf) return tile != null;
        // else return tile != null && tile != this;
        return tile != null && tile != this;
    }

    /// <summary>
    /// Returns true if the tile is one of the specified tiles.
    /// </summary>
    /// <param name="tile">Neighboring tile to compare to</param>
    /// <returns></returns>
    bool Check_Specified(TileBase tile)
    {
        
        return tilesToConnect.Contains(tile);

        //.Contains requires "using System.Linq;"
    }

    /// <summary>
    /// Returns true if the tile is empty.
    /// </summary>
    /// <param name="tile">Neighboring tile to compare to</param>
    /// <param name="tile"></param>
    /// <returns></returns>
    bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }


}