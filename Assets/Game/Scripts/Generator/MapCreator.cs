using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.IO;


public static class ObjectTypes{
        public static int on_delete = -1;
        public static int walkable_area = 0;
        public static int wall_area = 1;
        public static int jump_area = 2;
        public static int script_area = 3;
        public static int enemy_area = 4;
        public static int building_area = 5;
        // public static int spawner_area = 6;


        public static int[] marksOnTilemap = new int[]{ walkable_area, wall_area, jump_area, script_area, building_area };

        public static int[] obsticles = new int[]{ wall_area, building_area };

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

    public void Subscribe(TileMapGenerator tmg)
    {
        // Підписка на подію
        tmg.OnGlobalMapChange += RedrawTileOnTilemap;
    }


    // temporary public, make event
    public void RedrawTileOnTilemap(int x, int y, int type)
    {
        // Логіка обробки події
        
        foreach (TileMapGenerator tmg in tilemapGenerators){
            if (tmg.GetTileType() == type){
                Debug.Log("Event triggered!");
                Debug.Log(type);
                tmg.PlaceTile(x, y, type);
                
            }
            if (type == ObjectTypes.on_delete){
                if (tmg.GetTileType() == ObjectTypes.walkable_area){
                    tmg.PlaceTile(x, y, ObjectTypes.walkable_area);
                }
                else
                {
                    tmg.PlaceTile(x, y, ObjectTypes.on_delete);
                }
                
            }
        }
        
    }

    // private Tile walkableTile;
    void Start()
    {
        List<Tile> tiles = getTiles();
        // walkableTile = tiles[ObjectTypes.walkable_area];

        //List<string> typeNames = new List<string>();
        tilemapGenerators = this.GetComponentsInChildren<TileMapGenerator>();
        //foreach (TileMapGenerator tmg in tilemapGenerators){
        //    typeNames.Add(tmg.name);
        //}
        foreach (TileMapGenerator tmg in tilemapGenerators){
            Subscribe(tmg);
        }

        string tileMapPath = "assets\\Game\\Scripts\\Generator\\map1.map";
        List<int[]> mapList = new List<int[]>();

        foreach (var line in File.ReadAllLines(tileMapPath))
        {
            var row = Array.ConvertAll(line.Split(' '), int.Parse);
            mapList.Add(row);
        }

        /*string objectsOnMap = "assets\\Game\\Scripts\\Generator\\map2.map";
        List<int[]> BuildingsList = new List<int[]>();

        foreach (var line in File.ReadAllLines(path))
        {
            var row = Array.ConvertAll(line.Split(' '), int.Parse);
            BuildingsList.Add(row);
        }*/

        int[,] mapData = new int[mapList.Count, mapList[0].Length];

        for (int i = 0; i < mapList.Count; i++)
        {
            for (int j = 0; j < mapList[i].Length; j++)
            {
                mapData[i, j] = mapList[i][j];
            }
        }
        //Debug.Log(ObjectTypes.walkable_area);
        tilemapGenerators = this.GetComponentsInChildren<TileMapGenerator>();
        foreach (TileMapGenerator tmg in tilemapGenerators){
            tmg.Init(mapData, tiles, this);
        }

        foreach (TileMapGenerator tmg in tilemapGenerators){
            tmg.AfterStart();
        }
    }

    private List<Tile> getTiles(){
        List<Tile> tiles = new List<Tile>();
        Dictionary<string, Tile> temp_tiles = new Dictionary<string, Tile>();
        // Завантаження усіх Tile об'єктів з папки Resources/Tiles
        Tile[] loadedTiles = Resources.LoadAll<Tile>("Sprites");

        foreach (Tile tile in loadedTiles)
        {
            if (!temp_tiles.ContainsKey(tile.name)){
                temp_tiles.Add(tile.name, tile);
            }
        }

        foreach (string name in ObjectTypes.namesList){
            tiles.Add(temp_tiles[name]);
        }
        return tiles;
    }


}
