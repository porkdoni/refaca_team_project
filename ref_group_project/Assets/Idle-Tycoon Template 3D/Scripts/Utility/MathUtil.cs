using UnityEngine;

namespace IdleTycoon
{
    public static class MathUtil
    {
        public static Vector3[] GetArcPoints(Vector3 center, float radius, float angle, int segments)
        {
            Vector3[] points = new Vector3[segments + 1];
            int p = 0;

            int step = Mathf.RoundToInt(angle / segments);

            for (int i = 0; i <= angle; i += step)
            {
                Vector3 to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad)) + center;
                points[p] = to;
                p++;
            }
            return points;
        }

        public static Vector3[] GetCirclePoints(Vector3 center, float radius, int segments) => GetArcPoints(center, radius, 360, segments);
    }

}
