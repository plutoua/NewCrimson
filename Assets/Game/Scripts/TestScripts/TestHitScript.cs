using UnityEngine;

public class TestHitScript : MonoBehaviour
{
    void Start()
    {
        // Знищує цей об'єкт через 0.1 секунду
        Destroy(gameObject, 0.2f);
    }
}
