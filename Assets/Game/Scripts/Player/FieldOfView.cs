using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimmyFramework;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] ViewFieldScheme _viewFieldScheme;

    Mesh mesh;
    int layerMask;
    private Vector3 origin;
    private float startingAngle;
    [SerializeField] private bool player;
    [SerializeField] private int rays;
    [SerializeField] private float distance;
    [SerializeField] private float fov;

    public GameObject originObj;

    private Vector3 targetVector;


    private MouseLocatorController _mouseLocatorController;

   

    public void setOriginObject(GameObject o)
    {
        originObj = o;
    }
    [SerializeField] int[] layersToReact;

    public void setTargetVector(Vector3 vector)
    {
        targetVector = vector;
    }

    Vector3 angler(float angle)
    {
        float anglRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(anglRad), Mathf.Sin(anglRad));
    
    }

    float getAngleFromDirection(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    private void Start()
    {
        if (fov == 0.0f)
        {
            fov = 90.0f;
        }
        if (rays == 0.0f)
        {
            rays = 50;
        }
        if (distance == 0.0f)
        {
            distance = 10f;
        }

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        layerMask = LayerMask.GetMask("InteractiveLayer");
    
        if (player) { 
            if (Game.IsReady)
            {
               _mouseLocatorController = Game.GetController<MouseLocatorController>();
            }
            else
            {
                 Game.OnInitializedEvent += OnGameReady;
            }
        }
    }

    private void LateUpdate()
    {
        // transform.position = transform.parent.parent.position;
        // int layerMask = LayerMask.GetMask("InteractiveLayer");
        // Quaternion desiredGlobalRotation = Quaternion.Euler(0, 0, 0); // Приклад
        // Quaternion parentRotation = transform.parent.rotation;
        // transform.rotation = Quaternion.Inverse(parentRotation) * desiredGlobalRotation;
        if (player && Game.IsReady)
        {
            targetVector = _mouseLocatorController.MouseWorldPosition;
        }
        if (targetVector != null && originObj != null) {

            // Vector3 pos = transform.position;
            origin = originObj.transform.position;
            // origin = Vector3.zero;
            
            // Vector3 aimDir = (targetObj.transform.parent.TransformPoint(originObj.transform.localPosition) - originObj.transform.parent.TransformPoint(originObj.transform.localPosition)).normalized;
            Vector3 aimDir = (targetVector - originObj.transform.position).normalized;
            // Debug.Log(aimDir);

            float angle = getAngleFromDirection(aimDir);
            // Debug.Log(angle);
            angle += fov / 2.0f;
            float angle_increase = fov / rays;
            
            Vector3[] verticles = new Vector3[rays + 2];
            Vector2[] uv = new Vector2[verticles.Length];
            int[] triangles = new int[rays * 3];


            verticles[0] = origin;
            // Debug.Log("----------------");
            // Debug.Log(originObj.transform.position.x);
            // Debug.Log(originObj.transform.position.y);
            // Debug.Log("----------------");


            int v_ind = 1;
            int t_ind = 0;

            for (int i = 0; i <= rays; i++)
            {

                Vector3 vert;

                // Vector3 temp = originObj.transform.parent.position + originObj.transform.localPosition + transform.localPosition;
                // Vector3 temp = originObj.transform.parent.TransformPoint(originObj.transform.localPosition);
                //Vector3 temp = originObj.transform.position;
                //Debug.Log(temp.x);
                //Debug.Log(temp.y);
                // Vector2 temp2 = new Vector2(temp.x, temp.y);
                // Debug.Log(originObj.transform.parent.TransformPoint(originObj.transform.localPosition));
                if (angle < 0.0f)
                {
                    angle = 0.0f;
                }
                RaycastHit2D hit = Physics2D.Raycast(originObj.transform.position, angler(angle), distance, layerMask);

                // придумати краще рішення, ніж перевірка обєкту на імя. можливо шари або теги.

                if (!player && hit.collider != null && hit.collider.name == "Circle")
                {
                    // Це об'єкт гравця, повідомляємо про знаходження
                    // if 
                    EnemyPathFinder _enemyPathFinder = originObj.GetComponent<EnemyPathFinder>();
                    _enemyPathFinder.PlayerExistanceConfirmation();


                }
                // transform.InverseTransformPoint(hit.point);
                //Vector3 localHitPoint = transform.InverseTransformPoint(hit.point);
                // Debug.Log(angler(angle));
                if (hit.collider == null)
                {
                    // Debug.Log("NULLL");
                    vert = origin + angler(angle) * distance;
                    // Debug.Log(angler(angle));
                }
                else
                {
                    vert = hit.point;
                
                }
                verticles[v_ind] = vert;

                if (i > 0)
                {
                    triangles[t_ind] = 0;
                    triangles[t_ind + 1] = v_ind - 1;
                    triangles[t_ind + 2] = v_ind;

                    t_ind += 3;
                }
                angle -= angle_increase;
                v_ind += 1;
            }

            mesh.vertices = verticles;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
        }
    }

    private void OnGameReady()
    {
        _mouseLocatorController = Game.GetController<MouseLocatorController>();
    }

}
