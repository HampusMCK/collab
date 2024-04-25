using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;

    HealthSystem health;

    [Header("Sensing")]
    public float sightRange;
    public float sightAngle;
    public float hearingRange;
    public float smellRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthSystem>();
    }

    void Update()
    {



        
        // Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit1, sightRange, Physics.DefaultRaycastLayers);
        // Debug.DrawRay(transform.position + Vector3.up, transform.forward * sightRange, Color.green);

        // Physics.Raycast(transform.position + Vector3.up, DirFromAngle(20), out RaycastHit hit2, sightRange, Physics.DefaultRaycastLayers);
        // Debug.DrawRay(transform.position + Vector3.up, DirFromAngle(20) * sightRange, Color.green);

        // Physics.Raycast(transform.position + Vector3.up, DirFromAngle(-20), out RaycastHit hit3, sightRange, Physics.DefaultRaycastLayers);
        // Debug.DrawRay(transform.position + Vector3.up, DirFromAngle(-20) * sightRange, Color.green);
    }

    public Vector3 DirFromAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
