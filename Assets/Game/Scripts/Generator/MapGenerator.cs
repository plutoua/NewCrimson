using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using System.Collections;
using NavMeshPlus.Components;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap; // ��������� �� Tilemap
    // public Tile walkableTile; // ���� ��� ������� �����������
    public Tile blockedTile; // ���� � ��������� �� �����������
    public bool started = false;
    public NavMeshSurface surface;

    // ����� ��� ��������� �����
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

    // ����� ��� ��������� �����
    void PlaceTile(int x, int y, int tileType)
    {
        
        Vector3Int position = new Vector3Int(x, y, 0);

        switch (tileType)
        {
                case 0:
                    // tilemap.SetTile(position, walkableTile);
                    break;
                case 1:
                    tilemap.SetTile(position, blockedTile);
                    break;
        }
        
    }


    void Start(){
        
        int[,] mapData = new int[,] 
        {
            { 1, 1, 0, 1, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 1, 1, 1, 1 }
        };
        GenerateMap(mapData);

        surface.BuildNavMesh();
    }



}