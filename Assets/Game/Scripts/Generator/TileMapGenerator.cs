using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using System.Collections;
using NavMeshPlus.Components;
using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using TimmyFramework;
using System.Linq;


public class TileMapGenerator : MonoBehaviour
{

    // temporary MapCreator
    private MapCreator _mapCreator;
    private Tile _tile;
    private Tile _walkable_tile;
    private Tile _obsticle_tile;
    private PlayerLocatorController _playerLocatorController;
    [SerializeField]
    private int type;
    [SerializeField]
    private NavMeshSurface _surface;

    [SerializeField]
    private int[] additionalTypes;

    private int[,] _mapData;

    //  GameObject, that represents tile
    public TileObject[] _objectsPrefabs;

    public int GetTileType(){
        return type;
    }

    public delegate void GlobalMapChange(int x, int y, int type);

    public event GlobalMapChange OnGlobalMapChange;

    public void Init(int[,] mapData, List<Tile> tiles, MapCreator mapCreator){
        _mapCreator = mapCreator;
        _tile = tiles[type];
        
        
        _walkable_tile = tiles[ObjectTypes.walkable_area];
        _obsticle_tile = tiles[ObjectTypes.wall_area];
        _mapData = mapData;
        AfterStart();
    }

    private Tilemap _tilemap; // Посилання на Tilemap
    
    private void AfterStart(){
        _tilemap = GetComponent<Tilemap>();
        GenerateMap();

        _surface.BuildNavMesh();
        
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

    private void GenerateMap()
    {
        for (int x = 0; x < _mapData.GetLength(0); x++)
        {
            for (int y = 0; y < _mapData.GetLength(1); y++)
            {
                PlaceTile(x, y, _mapData[x, y]);
            }
        }
    }

    private void SetMapPartToWall(int x, int y){
        //if (OnGlobalMapChange != null)
        //    {
        //        OnGlobalMapChange(x, y, ObjectTypes.wall_area);
        //    }
        _mapData[x, y] = ObjectTypes.wall_area;
        _mapCreator.RedrawTileOnTilemap(x, y, ObjectTypes.wall_area);
        
    }

      private void SetMapPartToScript(int x, int y){
          //if (OnGlobalMapChange != null)
          //      {
          //          OnGlobalMapChange(x, y, ObjectTypes.script_area);
           //     }
          _mapData[x, y] = ObjectTypes.script_area;
          _mapCreator.RedrawTileOnTilemap(x, y, ObjectTypes.script_area);
        
    }

    public void PlaceTile(int x, int y, int tileType)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        if (type == tileType || additionalTypes.Contains(tileType)){
            Debug.Log("+++++++++++++++++");
            if (ObjectTypes.isMarked(tileType)){
                
                if (ObjectTypes.isObsticle(tileType)){
                    Debug.Log("isObsticle " + x.ToString() + ";" + y.ToString());
                    _tilemap.SetTile(position, _obsticle_tile);
                }
                else{
                    _tilemap.SetTile(position, _tile);
                } 
            }
            else{
                _tilemap.SetTile(position, _walkable_tile);
            }
            
            
            foreach (TileObject to in _objectsPrefabs){
                if (to && (tileType == to.type || additionalTypes.Contains(tileType))){

                    Debug.Log("Type" + tileType.ToString());



                    float fx = (float)x + to.GetLocationChanger();
                    float fy = (float)y + to.GetLocationChanger();
                    int[] additionalCoords = to.GetAdditionalCoords();
                    
                    

                    TileObject obj_clone = Instantiate(to, new Vector3(fx, fy, 0.1f), Quaternion.identity);

                    int[] coords =  new int[] { x, y };
                    
                  
                    if (additionalCoords != null){
                        int t_x = x;
                        int t_y = y;

                        int i = 0;

                    
                        // 0,1,1,1,1,0
                        int temp_coord;
                        foreach (int coord in additionalCoords){
                            if (i % 2 == 0) {
                                Array.Resize(ref coords, coords.Length + 2); // Resizing to 4 elements
                                
                                if (i != 0){
                                    Debug.Log("SetMapPartToWall " + i.ToString() + ";" + t_x.ToString() + ";" + t_y.ToString());
                                    SetMapPartToWall(t_x, t_y);
                                }
                            }
                            
                            if (i % 2 == 0){
                                
                                temp_coord = x + coord;
                                t_x = temp_coord;
                            }
                            else{
                                temp_coord = y + coord;
                                t_y = temp_coord;
                            }
                            
                            coords[i + 2] = temp_coord;
                            i++;
                        }
                        if (i % 2 == 0) {
                                if (i != 0){
                                    Debug.Log("SetMapPartToWall " + i.ToString() + ";" + t_x.ToString() + ";" + t_y.ToString());
                                    SetMapPartToWall(t_x, t_y);
                                }
                        }
                    }
                    
                    obj_clone.SetObjectCoords(coords);

                    int[] activatorsCoords = obj_clone.GetActivatorsCoords();

                    if (activatorsCoords != null){
                        int t_x = x;
                        int t_y = y;

                        int i = 0;
                        
                        foreach (int coord in activatorsCoords){
                            if (i % 2 == 0) {
                                Array.Resize(ref coords, coords.Length + 2); // Resizing to 4 elements
                                if (i != 0){
                                    Debug.Log("SetMapPartToScript " + i.ToString() + ";" + t_x.ToString() + ";" + t_y.ToString());
                                    SetMapPartToScript(t_x, t_y);
                                }
                            }
                            int temp_coord;
                            if (i % 2 == 0){
                                
                                temp_coord = x + coord;
                                t_x = temp_coord;
                            }
                            else{
                                temp_coord = y + coord;
                                t_y = temp_coord;
                            }
                            
                            coords[i] = temp_coord;
                            i++;
                        }
                        if (i % 2 == 0) {
                                if (i != 0){
                                    Debug.Log("SetMapPartToScript " + i.ToString() + ";" + t_x.ToString() + ";" + t_y.ToString());
                                    SetMapPartToScript(t_x, t_y);
                                }
                        }
                    }
                    
                }
            }
            
        }
    }

 


}