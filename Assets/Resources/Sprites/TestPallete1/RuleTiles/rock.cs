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
    static float[] angles = { 0f, 90f, 180f, 270f };
    int checkOnlyCounter;
    int fined;


    private void OnAwake()
    {
        fined = 0;
        checkOnlyCounter = 0;
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

        if (matchingTiles.Count > 0)
        {   
            int randomIndex = Random.Range(0, matchingTiles.Count);
            CustomRuleTile randomTile = matchingTiles[randomIndex];
            randomTile.GetTileData(position, tilemap, ref tileData);
        }
       
        float angle = ChooseRotationSprite(position, tilemap);
        
       
        if (angle != 0f)
        {
            //onse = true;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            tileData.transform = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        }
        
    }

    float ChooseRotationSprite(Vector3Int position, ITilemap tilemap)
    {
        CustomRuleTile currentTile = tilemap.GetTile<CustomRuleTile>(position);
        if (currentTile == null) return 0f; // Перевіряємо, чи поточний тайл є CustomRuleTile

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
        // start position - zero
        // 5 block of logic for:
        // possible variants by priority:
        //
        // unblanked 4x (random) + 
        // blank three 4x
        // blank two paralel 4x (2x 2 random)
        // blank two 4x
        // blank one 4x

        bool[] trueCounts = { hasTop, hasBottom, hasLeft, hasRight };

        int trueCount = 0;
        foreach (bool state in trueCounts) { 
            if (state)
            {
                trueCount++;
            }
        }

        float angle = 0f;

        if (trueCount == 4)
        {
            // unblanked 4x (random)
            angle = angles[UnityEngine.Random.Range(0, 4)];
            return angle;
        }
        else if (trueCount == 3)
        {
            if (!hasTop) { angle = 180f; }
            // if (!hasBottom) { angle = 0f; }
            if (!hasLeft) { angle = 270f; }
            if (!hasRight) { angle = 90f; }
        }
        else if (trueCount == 2)
        {
            // double logic
            if (hasTop && hasBottom)
            {
                 angle = 90f;
            }
            if (hasLeft && hasRight)
            {
                // angle = 90f;
            }

            // double angled

            if (hasLeft && hasTop)
            {
                angle = 270f;
            }

            if (hasLeft && hasBottom)
            {
                // angle = 0f;
            }

            if (hasRight && hasTop)
            {
                angle = 180f;
            }

            if (hasRight && hasBottom)
            {
                angle = 90f;
            }
        }
        else if (trueCount == 1)
        {
            if (hasTop)
            {
                angle = 180f;
            }
            if (hasBottom)
            {
                // angle = 180f;
            }
            if (hasLeft)
            {
                angle = 270;
            }
            if (hasRight)
            {
                angle = 90f;
            }
        }


            // Для інших умов...
            // 

            // 4 sided

            // Якщо немає сусідів, повертаємо спрайт за замовчуванням
            // Переробити на оригінальний спрайт з правил, якщо правило не прокнуло.
            return angle;
    }
    


    public class Neighbor : RuleTile.TilingRule.Neighbor {
        new public const int This = 1;
        new public const int NotThis = 2;
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
        public const int Only = 6;
        public const int Two = 7;
        public const int Three = 8;
    }

    public void SetConnected(CustomRuleTile[] tilesToConnect){
        if (this.tilesToConnect != null && this.tilesToConnect.Length < 1){
            this.tilesToConnect = tilesToConnect;
        }
        first_match = true;
    }

    private void MakeConnect(){
        // Debug.Log("MakeConnect");
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


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        fined = 0;
        checkOnlyCounter = 0;
        base.RefreshTile(position, tilemap);
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
            case Neighbor.Only: return Check_Only(tile);
            case Neighbor.Two: return Check_Two(tile);
            case Neighbor.Three: return Check_Three(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }


    /// <summary>
    /// Returns true if the tile is this, or if the tile is one of the tiles specified if always connect is enabled.
    /// </summary>
    /// <param name="tile">Neighboring tile to compare to</param>
    /// <returns></returns>
    bool Check_Only(TileBase tile)
    {
        bool res = true;
        checkOnlyCounter++;
        

        CustomRuleTile customTile = tile as CustomRuleTile;
        if (customTile != null)
        {
            // If the type of customTile is the same as this CustomRuleTile's type, return false
            if (customTile.type == this.type)
            {
                if (fined > 0)
                {
                    res = false;
                    // return false;
                }
                fined = 1;
                res = true;
                // return true;
            }
        }

        if (checkOnlyCounter % 4 == 0)
        {

            if (fined == 1)
            {
                res = true;
                // return true;
            }
            else
            {
                res = false;
                // return false;
            }
            fined = 0;


        }
       
        return res;
    }

    bool Check_Two(TileBase tile)
    {

        checkOnlyCounter++;
        bool res = true;

        CustomRuleTile customTile = tile as CustomRuleTile;
        if (customTile != null)
        {
            // If the type of customTile is the same as this CustomRuleTile's type, return false
            if (customTile.type == this.type)
            {
                // Debug.Log(fined);
                fined += 1;
                if (fined > 2)
                {
                    res = false;
                    // return false;
                }
                res = true;
                // return true;
            }
        }

        if (checkOnlyCounter % 4 == 0)
        {


            
            if (fined == 2)
            {
                res = true;
                // return true;
            }
            else
            {

                res = false;
                // return false;
            }
            fined = 0;


        }

        return res;
    }

    bool Check_Three(TileBase tile)
    {
        bool res = true;
        checkOnlyCounter++;


        CustomRuleTile customTile = tile as CustomRuleTile;
        if (customTile != null)
        {
            // If the type of customTile is the same as this CustomRuleTile's type, return false
            if (customTile.type == this.type)
            {
                fined += 1;
                if (fined > 3)
                {
                    res = false;
                    // return false;
                }

                res = true;
                // return true;
            }
        }

        if (checkOnlyCounter % 4 == 0)
        {

            checkOnlyCounter = 0;

            if (fined == 3)
            {

                res = true;
                //return true;
            }
            else
            {
                res = false;
            }
            fined = 0;


        }

        return res;
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