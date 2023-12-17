using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileTypeScheme", menuName = "ScriptableObjects/New TileTypeScheme", order = 1)]
// rename to TileType
public class TileTypeScheme : ScriptableObject
{
    // main fields
    [SerializeField] private string _name;
    [SerializeField] private Tile _tileImage;
    [SerializeField] private int _type;
    // think about external usage
    [SerializeField] private float _spawnDelay;
    // for example for object 2x1 (2 - longitude, 1 - width) [[1,0]]
    [SerializeField] private float _additionalUsedTiles;

    // [SerializeField] private float _activationDistance;

    // fields for linking
    [SerializeField] private int[] _relativeTile;

    // standart actions, linked with tile. destroy, activate, upgrade, relocate for example
    [SerializeField] private ActionScheme[] _actionSchemes;

    //  GameObject, that represents tile
    [SerializeField] private TileObject _objectPrefab;

    // Ask Timmy about realization
    [SerializeField] private int[] _itemsOnFlor;

    public float SpawnDelay => _spawnDelay;

    public void SpawnObject(Transform spawnTransform)
    {
        if (_objectPrefab){
            TileObject Temp;
            Temp = Instantiate(_objectPrefab, spawnTransform.position, Quaternion.identity);
            InitObject(Temp);
        }
    }

    private void InitObject(TileObject obj)
    {
        obj.Init();
    }
}
