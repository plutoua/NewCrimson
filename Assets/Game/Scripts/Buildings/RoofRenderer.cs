using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofRenderer : MonoBehaviour
{
    // Переконайтеся, що у вашого об'єкта є Collider
    private Collider objectCollider;
    // Renderer для зображення
    private Renderer imageRenderer;

    void Start()
    {
        // Отримуємо компоненти
        objectCollider = GetComponent<Collider>();
        imageRenderer = GetComponent<Renderer>();

        // Переконайтеся, що у об'єкта є компонент Renderer
        if (imageRenderer == null)
        {
            Debug.LogError("Renderer component not found on the object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Перевіряємо, чи об'єкт, який увійшов - це гравець
        Debug.Log(collision.tag);
        if (collision.CompareTag("Player"))
        {
            // Вимикаємо рендеринг
            imageRenderer.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Перевіряємо, чи об'єкт, який вийшов - це гравець
        if (collision.CompareTag("Player"))
        {
            // Вмикаємо рендеринг
            imageRenderer.enabled = true;
        }
    }
}
