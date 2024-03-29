using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.IO;
using TimmyFramework;


public static class ObjectTypes{
        public static int on_delete = -1;
        public static int walkable_area = 0;
        public static int wall_area = 1;
        public static int jump_area = 2;
        public static int script_area = 3;
        public static int enemy_area = 4;
        public static int building_area = 5;
        public static int spawner_area = 6;
        public static int background_area = 7;
        public static int road_area = 17;
        public static int big_pig_tavern = 13;


        public static int[] marksOnTilemap = new int[]{ walkable_area, wall_area, jump_area, script_area, building_area, spawner_area, background_area, road_area, big_pig_tavern };

        public static int[] obsticles = new int[]{ wall_area, building_area, big_pig_tavern  };

        public static bool isObsticle(int type){
            if (Array.IndexOf(obsticles, type) != -1){
                return true;
            }
            return false;
        }

        public static bool isMarked(int type){
            if (Array.IndexOf(marksOnTilemap, type) != -1){
                return true;
            }
            return false;
        }

        public static List<string> namesList = new List<string>{ 
            nameof(walkable_area),
            nameof(wall_area),
            nameof(jump_area),
            nameof(script_area),
            nameof(enemy_area),
            nameof(building_area),
            nameof(spawner_area),
            nameof(background_area),
            "",
            "",
            "",
            "",
            "",
            nameof(big_pig_tavern),
            "",
            "",
            "",
            nameof(road_area),
            };

        public static string asString(int type){
            if (namesList.Count > type){
                return namesList[type];
            }
            return "";
        }

        public static int asInt(string type){
            int i = 0;
            foreach (string str in namesList){
                if (str == type){
                    return i;
                }
                i++;
            }
            return -1;
        }
    }

public class MapCreator : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    TileMapGenerator[] tilemapGenerators;

    public ItemScheme _testItemScheme;

    /*public void Subscribe(TileMapGenerator tmg)
    {
        // ϳ������ �� ����
        tmg.OnGlobalMapChange += RedrawTileOnTilemap;
    }*/


    // temporary public, make event
    public void RedrawTileOnTilemap(int x, int y, int type, TileObject tileToUse=null)
    {
        // ����� ������� ��䳿
        
        foreach (TileMapGenerator tmg in tilemapGenerators){
            if (tmg.GetTileType() == type){
                tmg.PlaceTile(x, y, type, tileToUse);
                
            }
            if (type == ObjectTypes.on_delete){
/*                if (tmg.GetTileType() == ObjectTypes.walkable_area){
                    tmg.PlaceTile(x, y, ObjectTypes.walkable_area, tileToUse);
                }*/
                /*else
                {*/
                    tmg.PlaceTile(x, y, ObjectTypes.on_delete, tileToUse);
                /*}*/
                
            }
        }
        
    }

    // private Tile walkableTile;
    void Start()
    {
        Dictionary<string, Tile> tiles = getTiles();
        Dictionary<string, CustomRuleTile> customTiles = getCustomTiles();

        GroundDetectionController _groundDetectionController = Game.GetController<GroundDetectionController>();
        // _groundDetectionController._testItemScheme = _testItemScheme;

        //List<string> typeNames = new List<string>();
        tilemapGenerators = this.GetComponentsInChildren<TileMapGenerator>();
        //foreach (TileMapGenerator tmg in tilemapGenerators){
        //    typeNames.Add(tmg.name);
        //}
        /*foreach (TileMapGenerator tmg in tilemapGenerators){
            Subscribe(tmg);
        }*/

        int start_maps = 21;

        List<string> tileMapPaths = new List<string>();

        
        int m = 0;
        while (m < start_maps)
        {
            try {
                string tempString = "assets\\Game\\Scripts\\Generator\\map" + m.ToString() + ".map";
                tileMapPaths.Add(tempString);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            m++;
        }


        /*string tileMapPath = "assets\\Game\\Scripts\\Generator\\map1.map";
        string tileMapPath = "assets\\Game\\Scripts\\Generator\\map2.map";*/

        int map_num = 0;
        foreach (string tileMapPath in tileMapPaths) { 
            List<int[]> mapList = new List<int[]>();

            foreach (var line in File.ReadAllLines(tileMapPath))
            {
                var row = Array.ConvertAll(line.Split(' '), int.Parse);
                mapList.Add(row);
            }

            int[,] mapData = new int[mapList.Count, mapList[0].Length];

            for (int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Length; j++)
                {
                    mapData[i, j] = mapList[i][j];
                }
            }

            if (map_num == 0) {
                /*string objectsOnMap = "assets\\Game\\Scripts\\Generator\\map2.map";
                List<int[]> objectsList = new List<int[]>();

                foreach (var line in File.ReadAllLines(path))
                {
                    var row = Array.ConvertAll(line.Split(' '), int.Parse);
                    BuildingsList.Add(row);
                }*/

                
                //Debug.Log(ObjectTypes.walkable_area);
                tilemapGenerators = this.GetComponentsInChildren<TileMapGenerator>();
                foreach (TileMapGenerator tmg in tilemapGenerators){
                    tmg.Init(mapData, tiles, this, customTiles);
                }

                foreach (TileMapGenerator tmg in tilemapGenerators){
                    tmg.AfterStart();
                }
            }

            if (map_num == 0 && map_num == 1 && map_num == 2 && map_num == 3 && map_num == 4)
            {
                // UP, RIGHT, DOWN, LEFT, CURRENT
                _groundDetectionController.DownloadMap(mapData, map_num);
            }
            
            map_num++;
        }
    }

    private Dictionary<string, CustomRuleTile> getCustomTiles(){
        Dictionary<string, CustomRuleTile> tiles = new Dictionary<string, CustomRuleTile>();
        Dictionary<string, CustomRuleTile> temp_tiles = new Dictionary<string, CustomRuleTile>();
        // ������������ ��� CustomRuleTile ��'���� � ����� Resources/Tiles
        CustomRuleTile[] loadedRuleTiles = Resources.LoadAll<CustomRuleTile>("Sprites");

        foreach (CustomRuleTile tile in loadedRuleTiles)
        {
            if (!temp_tiles.ContainsKey(tile.name)){
                temp_tiles.Add(tile.name, tile);
            }
        }

        foreach (string name in ObjectTypes.namesList){
            if (temp_tiles.ContainsKey(name)){
                tiles.Add(name, temp_tiles[name]);
            }
        }
        return temp_tiles;
    }

    private Dictionary<string, Tile> getTiles(){
        // make_dynamic

        Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
        Dictionary<string, Tile> temp_tiles = new Dictionary<string, Tile>();
        // ������������ ��� Tile ��'���� � ����� Resources/Tiles
        Tile[] loadedTiles = Resources.LoadAll<Tile>("Sprites");

        foreach (Tile tile in loadedTiles)
        {
            if (!temp_tiles.ContainsKey(tile.name)){
                temp_tiles.Add(tile.name, tile);
            }
        }

        foreach (string name in ObjectTypes.namesList){
            if (temp_tiles.ContainsKey(name)){
                tiles.Add(name, temp_tiles[name]);
            }
        }
        return tiles;
    }


}
