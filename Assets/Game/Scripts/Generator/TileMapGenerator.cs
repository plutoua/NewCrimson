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
using System.Security.Permissions;


public class TileMapGenerator : MonoBehaviour
{

    // temporary MapCreator

    private MapCreator _mapCreator;
    Dictionary<string, Tile> _tiles;
    Dictionary<string, CustomRuleTile> _custom_tiles;
    // private Tile _tile;
    // private Tile _walkable_tile;
    // private Tile _obsticle_tile;
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

    bool onse = false;

    public delegate void GlobalMapChange(int x, int y, int type, TileObject tileToUse);

    public event GlobalMapChange OnGlobalMapChange;



    public void Init(int[,] mapData, Dictionary<string, Tile> tiles, MapCreator mapCreator, Dictionary<string, CustomRuleTile> customTiles){
        _mapCreator = mapCreator;
        
        //CHANGE _tile or _tiles, remove mix
        _tiles = tiles;
        _custom_tiles = customTiles;
        
        
        // _walkable_tile = tiles[ObjectTypes.walkable_area];
        // _obsticle_tile = tiles[ObjectTypes.wall_area];
        _mapData = mapData;
        _tilemap = GetComponent<Tilemap>();
        
    }

    private Tilemap _tilemap; // Посилання на Tilemap
    
    public void AfterStart(){
        
        GenerateMap();
        // Debug.Log(type);
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
                SetBackground(x, y);
            }
        }
        _tilemap.RefreshAllTiles();
    }

    private void SetMapPartToWall(int x, int y){
        //if (OnGlobalMapChange != null)
        //    {
        //        OnGlobalMapChange(x, y, ObjectTypes.wall_area);
        //    }
        _mapData[x, y] = ObjectTypes.wall_area;
        _mapCreator.RedrawTileOnTilemap(x, y, ObjectTypes.wall_area);
        //SetBackground(x, y);
        
    }

    private void SetMapPartToWalkable(int x, int y){
        //if (OnGlobalMapChange != null)
        //    {
        //        OnGlobalMapChange(x, y, ObjectTypes.walkable_area);
        //    }
        _mapData[x, y] = ObjectTypes.walkable_area;
        _mapCreator.RedrawTileOnTilemap(x, y, -1);
        //SetBackground(x, y);
    }

    private void SetBackground(int x, int y){
        // _mapData[x, y] = ObjectTypes.walkable_area;
        _mapCreator.RedrawTileOnTilemap(x, y, ObjectTypes.background_area);
    }

      private void SetMapPartToScript(int x, int y){
          //if (OnGlobalMapChange != null)
          //      {
          //          OnGlobalMapChange(x, y, ObjectTypes.script_area);
           //     }
          _mapData[x, y] = ObjectTypes.script_area;
          _mapCreator.RedrawTileOnTilemap(x, y, ObjectTypes.script_area);
        
    }

    /*
    private bool tileConnected(Vector3Int checkPosition, string direction, int type )
    {
        
        Vector3Int neighborPosition = checkPosition;

        switch (direction)
        {
            case "right":
                neighborPosition.x += 1;
                break;
            case "left":
                neighborPosition.x -= 1;
                break;
                // Можна додати додаткові напрямки якщо потрібно
        }

        if (neighborPosition.x >= 0 && neighborPosition.x < _mapData.GetLength(0) &&
            neighborPosition.y >= 0 && neighborPosition.y < _mapData.GetLength(1))
        {
            return _mapData[neighborPosition.x, neighborPosition.y] == type;
        }
        
        return false;
    }*/


    public void PlaceTile(int x, int y, int tileType=0, TileObject tileToUse=null)
    {
        // СИСТЕМА ТИПОВИХ ТА УНІКАЛЬНИХ ОБЄКТІВ. У обєкта є тип і унікальний ід обєкта, якщо він потрібен
        if (tileToUse){
            tileType = tileToUse.GetTileType();
        }
        Vector3Int position = new Vector3Int(x, y, 0);

        if (tileType == ObjectTypes.on_delete){
            _tilemap.SetTile(position, null);
            // REUSE THIS BLOCK FOR ALL OBJECTS DELETION IN ADDITIONAL AND SCRIPT
        }

        else if (type == tileType || additionalTypes.Contains(tileType)){
            if (ObjectTypes.isMarked(tileType)){
                if (ObjectTypes.isObsticle(tileType)){

                    if (_custom_tiles.ContainsKey(ObjectTypes.asString(ObjectTypes.wall_area))){


                        CustomRuleTile tileToSet = _custom_tiles[ObjectTypes.asString(ObjectTypes.wall_area)];
                        _tilemap.SetTile(position, tileToSet);
                        /*float angle = 0f;
                        if (tileConnected(position, "right", type))
                        {
                            angle = 90f;
                            Debug.Log(angle);
                        }
                        else if (tileConnected(position, "left", type))
                        {
                            angle = -90f;
                        }

                        // REMOVE IT
                        if (!onse) {
                            onse = true;
                            TileFlags flags = _tilemap.GetTileFlags(position);

                            // Видалення флага LockTransform
                            flags &= ~TileFlags.LockTransform;

                            // Застосування змінених флагів до тайла
                            _tilemap.SetTileFlags(position, flags);
                        }*/

                        //Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle), Vector3.one);
                        //_tilemap.SetTransformMatrix(position, matrix);
                        //_tilemap.RefreshTile(position);
                    }

                    else if (_tiles.ContainsKey(ObjectTypes.asString(ObjectTypes.wall_area))){
                        _tilemap.SetTile(position, _tiles[ObjectTypes.asString(ObjectTypes.wall_area)]);
                    }

                    // _tilemap.SetTile(position, _obsticle_tile);
                }
                else
                {

                    if (_custom_tiles.ContainsKey(ObjectTypes.asString(tileType))){
                        _tilemap.SetTile(position, _custom_tiles[ObjectTypes.asString(tileType)]);
                    }

                    else if (_tiles.ContainsKey(ObjectTypes.asString(tileType))){
                        _tilemap.SetTile(position, _tiles[ObjectTypes.asString(tileType)]);
                    }
                    else
                    {
                        _tilemap.SetTile(position, _tiles[ObjectTypes.asString(ObjectTypes.wall_area)]);
                    }

                    // _tilemap.SetTile(position, _tiles[tileType]);
                } 
            }
            else{
                if (_custom_tiles.ContainsKey(ObjectTypes.asString(ObjectTypes.walkable_area))){
                        _tilemap.SetTile(position, _custom_tiles[ObjectTypes.asString(ObjectTypes.walkable_area)]);
                    }

                    else if (_tiles.ContainsKey(ObjectTypes.asString(ObjectTypes.walkable_area))){
                        _tilemap.SetTile(position, _tiles[ObjectTypes.asString(ObjectTypes.walkable_area)]);
                    }
                // _tilemap.SetTile(position, _walkable_tile);
            }

            if (tileToUse){
                

                    float fx = (float)x + tileToUse.GetLocationChanger();
                    float fy = (float)y + tileToUse.GetLocationChanger();
                    int[] additionalCoords = tileToUse.GetAdditionalCoords();
                    
                    

                    TileObject obj_clone = Instantiate(tileToUse, new Vector3(fx, fy, 0.1f), Quaternion.identity);

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
                                    // SetMapPartToWalkable(t_x, t_y, _walkable_tile);
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
                                    SetMapPartToWalkable(t_x, t_y);
                                    SetMapPartToScript(t_x, t_y);
                                }
                        }
                    }

            }

            if (_objectsPrefabs.Count() > 0) {
                int random_number = UnityEngine.Random.Range(0, _objectsPrefabs.Count());
                // random_number = random.randint(0, _objectsPrefabs.Count);
                TileObject to = _objectsPrefabs[random_number];
                // MAKE RANDOM MECHANIZM
                if (to && (tileType == to.type || additionalTypes.Contains(tileType))){

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
                                    // Debug.Log("SetMapPartToWall " + i.ToString() + ";" + t_x.ToString() + ";" + t_y.ToString());
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
                                    // SetMapPartToWalkable(t_x, t_y, _walkable_tile);
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
                                    SetMapPartToWalkable(t_x, t_y);
                                    SetMapPartToScript(t_x, t_y);
                                }
                        }
                    }
                    
                }
            }
            
        }
    }

 


}