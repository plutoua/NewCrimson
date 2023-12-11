using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using System.Collections;
using NavMeshPlus.Components;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using TimmyFramework;
using System.Linq;


public class MapGenerator : MonoBehaviour
{
    // MAKE 1 SINGLE LIST ON START OF GAME
    [SerializeField]
    private List<Tile> _tiles;

    private void getTiles(){
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
            
            _tiles.Add(temp_tiles[name]);
        }
    }

    private Tilemap _tilemap; // Посилання на Tilemap
    // public Tile walkableTile; // Тайл для вільного пересування
    // public Tile blockedTile; // Тайл з забороною на пересування
    
    public static class ObjectTypes{
        public static int walkable_area = 0;
        public static int wall_area = 1;
        public static int jump_area = 2;
        public static int script_area = 3;
        public static int enemy_area = 4;

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


    }



    public GameObject testEnemyObject;
    private PlayerLocatorController _playerLocatorController;

    // public Transform player_transform;

    // public bool started = false;
    [SerializeField]
    private int type = 0;

    public NavMeshSurface surface;

    public void loadTiles(){
        
    }

    // Метод для генерації карти
    private void Start(){

        getTiles();
        string path = "assets\\Game\\Scripts\\Generator\\map1.map";
        List<int[]> mapList = new List<int[]>();

        foreach (var line in File.ReadAllLines(path))
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

        // Виведення мапи для перевірки
        /*for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                Console.Write(mapData[i, j] + " ");
            }
            Console.WriteLine();
        }
        
        int[,] mapData = new int[,] 
        {
            { 1, 1, 0, 1, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 2, 2, 1, 1 },
        };
        */
        _tilemap = GetComponent<Tilemap>();
        GenerateMap(mapData);

        surface.BuildNavMesh();
        
        if (Game.IsReady){
           _playerLocatorController = Game.GetController<PlayerLocatorController>();
        }
        else {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
    }

    private void GenerateMap(int[,] mapData)
    {
        for (int x = 0; x < mapData.GetLength(0); x++)
        {
            for (int y = 0; y < mapData.GetLength(1); y++)
            {
                PlaceTile(x, y, mapData[x, y]);
            }
        }
    }

    // Метод для розміщення тайлів
    private void PlaceTile(int x, int y, int tileType)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        if (tileType == type){
            _tilemap.SetTile(position, _tiles[tileType]);
        }
        // тимчасове місце для спавну ворогів і обєктів на першому генераторі (виділить окремий генератор ворогів)
        if (type == ObjectTypes.enemy_area && tileType == ObjectTypes.enemy_area){
            float fx = (float)x + 0.5f;
            float fy = (float)y + 0.5f;
            GameObject obj_clone = Instantiate(testEnemyObject, new Vector3(fx, fy, 0.1f), Quaternion.identity);
        }
    }

 


}