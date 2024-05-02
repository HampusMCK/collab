using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]
public class EnemyEditor : Editor
{
    WorldSC world;

    void Awake()
    {
        world = GameObject.Find("World").GetComponent<WorldSC>();
    }

    void OnSceneGUI()
    {
        EnemyController ec = (EnemyController)target;

        //Draw sight
        Handles.color = Color.red;
        float a1 = ec.transform.eulerAngles.y;
        Vector3 sightAngleA = MathX.DirFromAngle(-ec.sightAngle / 2, a1);
        Vector3 sightAngleB = MathX.DirFromAngle(ec.sightAngle / 2, a1);
        Handles.DrawLine(ec.transform.position, ec.transform.position + sightAngleA * ec.sightRange);
        Handles.DrawLine(ec.transform.position, ec.transform.position + sightAngleB * ec.sightRange);
        Handles.DrawWireArc(ec.transform.position, Vector3.up, sightAngleA, ec.sightAngle, ec.sightRange);

        //Draw hearing
        Handles.color = Color.magenta;
        Handles.DrawWireArc(ec.transform.position, Vector3.up, ec.transform.forward, 360, ec.hearingRange);

        //Draw smell
        Handles.color = Color.yellow;
        Vector3 reverseWind = -world.wind;
        float b1 = Vector3.Angle(world.transform.forward, reverseWind);
        if (reverseWind.x < 0) b1 = -b1 + 360;
        float c1 = 45;
        Vector3 smellAngleA = MathX.DirFromAngle(-c1 / 2, b1);
        Vector3 smellAngleB = MathX.DirFromAngle(c1 / 2, b1);
        Handles.DrawLine(ec.transform.position, ec.transform.position + smellAngleA * ec.smellRange);
        Handles.DrawLine(ec.transform.position, ec.transform.position + smellAngleB * ec.smellRange);
        Handles.DrawWireArc(ec.transform.position, Vector3.up, smellAngleA, c1, ec.smellRange);
    }

}
