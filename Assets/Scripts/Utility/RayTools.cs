using UnityEngine;

namespace Utility
{
    public static class RayTools
    {
        /// <summary>
        /// Returns the nearest target mask position, adjusted for an objects size.
        /// </summary>
        public static Vector3 FindNearestSurface(Vector3 spawnOrigin, LayerMask targetMask, float heightOffSurface)
        {
            Physics.Raycast(spawnOrigin, Vector3.down, out RaycastHit hit, Mathf.Infinity, targetMask);
            Vector3 safePosition = new Vector3(hit.point.x, hit.point.y + heightOffSurface, hit.point.z);
            return safePosition;
        }
        //
        // public static Vector3 FindSpotInArea(Vector3 origin, LayerMask targetMask, float heightOffSurface, float radius)
        // {
        //     Physics.SphereCast(origin, radius, Vector3.zero, out RaycastHit hit, radius);
        //     Vector3 position = new Vector3(hit.collider.poi);
        // }
    }
}
