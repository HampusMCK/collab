using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;

    GameObject target;

    HealthSystem health;

    void Awake()
    {  
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthSystem>();
    }

    void Update()
    {
        
    }
}
