using UnityEngine;

public static class MathX
{
    public static Vector3 DirFromAngle(float angle, float eulerAngleRotation)
    {
        angle += eulerAngleRotation;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}