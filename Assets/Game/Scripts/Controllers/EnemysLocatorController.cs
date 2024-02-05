using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using TimmyFramework;
using UnityEngine.Events;
using System.Linq;

public class EnemysLocatorController : IController
{
    public Dictionary<int, Vector3> enemies_coords_links;

    public Dictionary<int, GameObject> enemies;
    public int partyId;
    public float locatorDistance;
    //private bool linked = false;

    public void Initialize()
    {
        partyId = 0;
        locatorDistance = 10.0f;
        //linked = false;
        enemies_coords_links = new Dictionary<int, Vector3>();

    }

    public int GetNewPartyId()
    {
        partyId++;
        return partyId;
    }

    private GameObject FindNearestTargetableEnemy(Vector3 pointFrom)
    {
        GameObject nearestEnemy = null;

        foreach (var item in enemies_coords_links)
        {
            int enemyId = item.Key;
            Vector3 enemyPosition = item.Value;

            if (enemies.TryGetValue(enemyId, out GameObject enemy)) // Перевірка на специфічну характеристику
            {
                float distance = Vector3.Distance(pointFrom, enemyPosition);

                if (distance < locatorDistance && distance > 1.0f && !enemy.GetComponent<EnemyPathFinder>().chain_part)
                {
                    // maxDistance = distance;
                    nearestEnemy = enemy.gameObject;
                    // nearestEnemy.GetComponent<EnemyPathFinder>().chain_part = true;
                }
            }
        }

        return nearestEnemy;
    }

    public bool LinkEnemy(List<int> ids, int leaderLevel, Vector3 last_position)
    {
        // find nearest Enemy with specific bool values using "wave" method and avoiding tiles on "wall" layer

        GameObject nearesEnemy = null;
        
        

            nearesEnemy = FindNearestTargetableEnemy(last_position);
            //Debug.Log(nearesEnemy.transform.position);
            //nearesEnemy = FindNearestTargetableEnemy(last_position);
            if (nearesEnemy!= null) { 
                EnemyPathFinder _enemyPathFinder = nearesEnemy.GetComponent<EnemyPathFinder>();

                if (_enemyPathFinder != null)
                {
                    List<GameObject> party = new List<GameObject>();
                

                
                    // ids.Add(_enemyPathFinder._id);

                    foreach (int id in ids)
                    {
                        Debug.Log(id);
                        party.Add(enemies[id]);
                    }

                    int newleaderLevel;
                    bool new_group_leader = false;
                    if (_enemyPathFinder.leaderLevel > leaderLevel)
                    {
                    
                        newleaderLevel = _enemyPathFinder.leaderLevel;
                    }
                    else
                    {
                        newleaderLevel = leaderLevel;
                    }

                    //if (newleaderLevel > party.Count)
                    //{
                    //    new_group_leader = true;
                    //}

                    Debug.Log(newleaderLevel);
                    Debug.Log(new_group_leader);
                    // _enemyPathFinder.chain_part = true;

                    _enemyPathFinder.partyId = partyId;
                    _enemyPathFinder.LinkEnemy(newleaderLevel, party);
                

                    if (newleaderLevel < party.Count)
                    {
                        partyId++;
                    }
                    return true;

                }
            }
        
        
        return false;
        
    }

    public int InsertEnemy(GameObject obj){
        // add recursive try-except with key +1 on collision, couse single link usage.
        if (enemies == null){
            enemies = new Dictionary<int, GameObject>
            {
                { 0, null }
            };
        }
        int maxKey = enemies.Keys.Max();
        enemies.Add(maxKey + 1, obj);
        enemies_coords_links.Add(maxKey + 1, obj.transform.position);
        return maxKey + 1;
    }

    public int DeleteEnemy(GameObject obj = null, int id = -1){
        // -1 on error, or enemy id on success
        if (obj) {
            // int keyToRemove = -1;
            foreach (var pair in enemies)
            {
                if (pair.Value == obj)
                {
                    id = pair.Key;
                    
                }
            }   
        
        }
        if (id != -1){
            enemies_coords_links.Remove(id);
            enemies.Remove(id);
            return id;
        }
        return -1;
    }

    public void UpdateStorage(){
    }
}