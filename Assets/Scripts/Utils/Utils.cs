using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector3 GetRandomSpawnPoint()
        {
            return new Vector3(Random.Range(-9, 9), Random.Range(-3, 3), 0);
        }
    }
}
