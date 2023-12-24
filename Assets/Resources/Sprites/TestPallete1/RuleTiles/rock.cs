using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

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

        // якщо Ї п≥дход€щ≥ тайли, вибираЇмо один випадково
        if (matchingTiles.Count > 0)
        {   
            int randomIndex = Random.Range(0, matchingTiles.Count);
            CustomRuleTile randomTile = matchingTiles[randomIndex];
            randomTile.GetTileData(position, tilemap, ref tileData);
        }
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