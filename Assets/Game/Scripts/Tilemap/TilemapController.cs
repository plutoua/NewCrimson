using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    public Transform tileParent; // Батьківський трансформ для інстанційованих префабів
    public GameObject[] tilePrefabs; // Масив префабів тайлів
    public MapGenerator mg;

    // Функція для створення тайла
    public void SpawnTile(Vector3Int cellPosition, int prefabIndex)
    {
    /*
        if (prefabIndex < 0 || prefabIndex >= tilePrefabs.Length)
        {
            Debug.LogError("Prefab index is out of range.");
            return;
        }

        // Перетворити позицію клітинки на світові координати
        Vector3 worldPosition = tileParent.TransformPoint(cellPosition);

        // Інстанціювати префаб
        Instantiate(tilePrefabs[prefabIndex], worldPosition, Quaternion.identity, tileParent);
    */
    }

    // Функція для завантаження префабів
    private void LoadTilePrefabs()
    {
        tilePrefabs = Resources.LoadAll<GameObject>("Pallete");
    }

    // Початкова ініціалізація
    void Start()
    {
        // Припустимо, що ваші тайли знаходяться у папці Resources/Tiles
        LoadTilePrefabs();
        // mg.AfterStart(tilePrefabs[0]);
    }
}