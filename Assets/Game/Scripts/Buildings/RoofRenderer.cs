using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofRenderer : MonoBehaviour
{
    // �������������, �� � ������ ��'���� � Collider
    private Collider objectCollider;
    // Renderer ��� ����������
    private Renderer imageRenderer;

    void Start()
    {
        // �������� ����������
        objectCollider = GetComponent<Collider>();
        imageRenderer = GetComponent<Renderer>();

        // �������������, �� � ��'���� � ��������� Renderer
        if (imageRenderer == null)
        {
            Debug.LogError("Renderer component not found on the object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����������, �� ��'���, ���� ������ - �� �������
        Debug.Log(collision.tag);
        if (collision.CompareTag("Player"))
        {
            // �������� ���������
            imageRenderer.enabled = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // ����������, �� ��'���, ���� ������ - �� �������
        if (collision.CompareTag("Player"))
        {
            // ������� ���������
            imageRenderer.enabled = true;
        }
    }
}
