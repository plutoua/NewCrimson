using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    public Transform tileParent; // ����������� ��������� ��� ��������������� �������
    public GameObject[] tilePrefabs; // ����� ������� �����
    public MapGenerator mg;

    // ������� ��� ��������� �����
    public void SpawnTile(Vector3Int cellPosition, int prefabIndex)
    {
    /*
        if (prefabIndex < 0 || prefabIndex >= tilePrefabs.Length)
        {
            Debug.LogError("Prefab index is out of range.");
            return;
        }

        // ����������� ������� ������� �� ����� ����������
        Vector3 worldPosition = tileParent.TransformPoint(cellPosition);

        // ������������� ������
        Instantiate(tilePrefabs[prefabIndex], worldPosition, Quaternion.identity, tileParent);
    */
    }

    // ������� ��� ������������ �������
    private void LoadTilePrefabs()
    {
        tilePrefabs = Resources.LoadAll<GameObject>("Pallete");
    }

    // ��������� �����������
    void Start()
    {
        // ����������, �� ���� ����� ����������� � ����� Resources/Tiles
        LoadTilePrefabs();
        // mg.AfterStart(tilePrefabs[0]);
    }
}