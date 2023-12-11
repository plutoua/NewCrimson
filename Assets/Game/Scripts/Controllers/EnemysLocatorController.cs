using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using TimmyFramework;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EnemysLocatorController : IController
{
    public Dictionary<int, Vector3> enemys_coords_links;
    public Dictionary<int, GameObject> enemys;

    public void Initialize()
    {
    }

    public int InsertEnemy(GameObject obj){
        // add recursive try-except with key +1 on collision, couse single link usage.
        if (enemys == null){
            enemys = new Dictionary<int, GameObject>
            {
                { 0, null }
            };
        }
        int maxKey = enemys.Keys.Max();
        enemys.Add(maxKey + 1, obj);
        return maxKey + 1;
    }

    public int DeleteEnemy(GameObject obj = null, int id = -1){
        // -1 on error, or enemy id on success
        if (obj) {
            int keyToRemove = -1;
            foreach (var pair in enemys)
            {
                if (pair.Value == obj)
                {
                    id = pair.Key;
                    
                }
            }   
        
        }
        if (id != -1){
            enemys_coords_links.Remove(id);
            enemys.Remove(id);
            return id;
        }
        return -1;
    }

    public void UpdateStorage(){
    }
}