using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TimmyFramework;

public class EnemyPathFinder : MonoBehaviour
{
    public NavMeshAgent agent;
    private PlayerLocatorController _playerLocatorController;
    private EnemysLocatorController _enemysLocatorController;
    private float timer = 0.0f;
    private float interval = 0.5f;
    private float decision_time_max = 1.5f;
    private float decision_time_min = 0.3f;
    private int _id = -1;
    private bool new_move = false;

    void Start()
    {
        _id = -1;
        timer = 0.0f;
        interval = 0.5f;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        // start new_move position, may be game logic part
        new_move = true;
    }

    private void OnChangePlayerPosition(Vector3 position){
        new_move = true;
    }

    private void Awake(){
        if (Game.IsReady){
           _playerLocatorController = Game.GetController<PlayerLocatorController>();
           _enemysLocatorController = Game.GetController<EnemysLocatorController>();
        }
        else {
            Game.OnInitializedEvent += OnGameReady;
        }
    }

    private void OnGameReady()
    {
        Game.OnInitializedEvent -= OnGameReady;
        _playerLocatorController = Game.GetController<PlayerLocatorController>();
        _playerLocatorController.PlayerChangePositionEvent+= OnChangePlayerPosition;

        _enemysLocatorController = Game.GetController<EnemysLocatorController>();
        _id = _enemysLocatorController.InsertEnemy(this.gameObject);

    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (new_move && timer >= interval)
        {
                timer = 0f; // Скидання таймера
                interval = UnityEngine.Random.Range(decision_time_min, decision_time_max);
                if (agent){
                    agent.SetDestination(_playerLocatorController.PlayerPosition);
                }
                new_move = false;
        }


    }

    
}
