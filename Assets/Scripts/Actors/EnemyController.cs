using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    HealthSystem health;
    SmellSystem smell;

    [HideInInspector]
    public WorldSC world;

    [Header("Sensing")]
    public float sightRange;
    public float sightAngle;
    public float hearingRange;
    public float smellRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthSystem>();
        smell = GetComponent<SmellSystem>();
        world = GameObject.Find("World").GetComponent<WorldSC>();

        Noise.SoundEvent += OnHearNoise;
    }

    void OnDestroy()
    {
        Noise.SoundEvent -= OnHearNoise;
    }

    void Update()
    {
        GameObject[] inSight = GetObjectsInSight();
        foreach (GameObject g in inSight)
        {
            if (g.tag == "Player")
            {
                // agent.SetDestination(g.transform.position);
            }
        }

        GameObject[] inHear = GetObjectsInHearing();
        foreach (GameObject g in inHear)
        {
            if (g.tag == "Player")
            {

            }
        }

        GameObject[] inSmell = GetObjectsInSmell(world.wind);
        foreach (GameObject g in inSmell)
        {
            if (g.tag == "Player")
            {
                if (g.GetComponent<SmellSystem>().odour > 2)
                {
                    agent.SetDestination(g.transform.position);
                }
            }
        }
    }

    GameObject[] GetObjectsInSight()
    {
        string[] layers = { LayerMask.LayerToName(6), LayerMask.LayerToName(7) };
        LayerMask mask = LayerMask.GetMask(layers);
        Collider[] colidsInRange = Physics.OverlapSphere(transform.position, sightRange, mask, QueryTriggerInteraction.UseGlobal);
        List<GameObject> objsInSight = new();

        foreach (Collider c in colidsInRange)
        {
            Vector3 dirToColid = c.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dirToColid) < sightAngle / 2)
            {
                Vector3 pos = transform.position + Vector3.up;
                dirToColid = c.transform.position - pos;
                Physics.SphereCast(pos, 0.5f, dirToColid, out RaycastHit hit, sightRange, mask);
                if (hit.collider == null)
                {
                    GameObject[] e = { };
                    return e;
                }

                Debug.DrawLine(pos, hit.point, Color.green);

                if ((mask & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    GameObject g = c.gameObject;
                    if (g.transform.parent != null) g = g.transform.parent.gameObject;
                    objsInSight.Add(g);
                }
            }
        }

        return objsInSight.ToArray();
    }

    GameObject[] GetObjectsInHearing()
    {
        string[] layers = { LayerMask.LayerToName(6) };
        LayerMask mask = LayerMask.GetMask(layers);
        Collider[] colidsInHear = Physics.OverlapSphere(transform.position, hearingRange, mask, QueryTriggerInteraction.UseGlobal);
        List<GameObject> objsInHear = new();

        foreach (Collider c in colidsInHear)
        {
            GameObject g = c.gameObject;
            if (g.transform.parent != null) g = g.transform.parent.gameObject;
            objsInHear.Add(g);
        }

        return objsInHear.ToArray();
    }

    GameObject[] GetObjectsInSmell(Vector3 windDir)
    {
        Vector3 reverseWind = -windDir;
        string[] layers = { LayerMask.LayerToName(6) };
        LayerMask mask = LayerMask.GetMask(layers);
        Collider[] smellyColids = Physics.OverlapSphere(transform.position, smellRange, mask, QueryTriggerInteraction.UseGlobal);
        List<GameObject> smellyObjs = new();

        foreach (Collider c in smellyColids)
        {
            Vector3 dirToColid = c.transform.position - transform.position;
            if (Vector3.Angle(reverseWind, dirToColid) < 45 / 2)
            {
                GameObject g = c.gameObject;
                if (g.transform.parent != null) g = g.transform.parent.gameObject;
                smellyObjs.Add(g);
            }
        }

        return smellyObjs.ToArray();
    }

    void OnHearNoise(Vector3 Position, int Intensity)
    {
        float distance = Vector3.Distance(transform.position, Position) * 0.1f;

        float intense = 20 - Intensity;
        float distIntens = (distance / Intensity);

        float radius = intense * (distance / 2);

        if (distance < hearingRange + Intensity)
        {
            Vector3 randomDir = Random.insideUnitSphere;

            Vector3 destination = Position + (randomDir * radius);

            agent.SetDestination(destination);
        }

        print(distance);
        print(Intensity);
        print(radius);



    }
}
