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


public class MapGenerator : MonoBehaviour
{
    // MAKE 1 SINGLE LIST ON START OF GAME
    public List<Tile> tiles;
    public Tilemap tilemap; // Посилання на Tilemap
    // public Tile walkableTile; // Тайл для вільного пересування
    // public Tile blockedTile; // Тайл з забороною на пересування
    public GameObject testEnemyObject;
    public Transform player_transform;




    public bool started = false;
    public int type = 0;
    public NavMeshSurface surface;

    // Метод для генерації карти
    public void GenerateMap(int[,] mapData)
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
    void PlaceTile(int x, int y, int tileType)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        if (tileType == type){
            tilemap.SetTile(position, tiles[tileType]);
        }
        // тимчасове місце для спавну ворогів і обєктів на першому генераторі (виділить окремий генератор ворогів)
        if (type == 0 && tileType == 4){
            float fx = (float)x + 0.5f;
            float fy = (float)y + 0.5f;
            GameObject obj_clone = Instantiate(testEnemyObject, new Vector3(fx, fy, 0), Quaternion.identity);
            EnemyPathFinder emp = obj_clone.GetComponent<EnemyPathFinder>();
            emp.target = player_transform;
        }
    }


    void Start(){

        string path = "C:\\Users\\loydy\\Desktop\\test_shit\\map1.map";
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

        GenerateMap(mapData);

        surface.BuildNavMesh();
    }



}