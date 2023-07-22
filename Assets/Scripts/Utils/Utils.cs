using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static Vector3 GetRandomSpawnPoint()
        {
            return new Vector3(Random.Range(-9, 10), Random.Range(-2, 3), 0);
        }
    }
}
