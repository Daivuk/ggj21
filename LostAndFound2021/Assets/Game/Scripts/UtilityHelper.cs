using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityHelper
{
    public static Vector3 GetVectorFromAngle(float angle)
    {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
    public static float GetFloatAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
    public static float getAngleBetweenTwoPoints(Vector3 pos1, Vector3 pos2)
    {
        var dir = pos1 - pos2;
        float angle = GetFloatAngleFromVector(dir);
        return angle;
    }
    public static string serialzedVetor3(Vector3 value)
    {
        string returnValue = value.x.ToString() + "," + value.y.ToString() + "," + value.z;
        return returnValue;
    }
    public static int GetAngleFromVector180(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }
    public static Vector3 ApplyRotationToVector(Vector3 normalRotation, float angle)
    {
        float normalAngle = GetFloatAngleFromVector(normalRotation);
        normalAngle += angle;
        return GetVectorFromAngle(normalAngle);
    }
    public static Vector3 deserialzedVector3(string value)
    {
        Vector3 returnVector = Vector3.zero;

        string[] strArray = value.Split(',');
        returnVector.x = float.Parse(strArray[0]);
        returnVector.y = float.Parse(strArray[1]);
        returnVector.z = float.Parse(strArray[2]);

        return returnVector;
    }

}
