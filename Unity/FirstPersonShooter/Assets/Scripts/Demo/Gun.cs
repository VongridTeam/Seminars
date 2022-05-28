using UnityEngine;

namespace Vongrid.DemoFPS.Demo
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
    public class Gun : ScriptableObject
    {
        public string gunName;
        public float baseDamage;
        public float range;
        public float fireRate;
        public int magSize;
        public float reloadTime;
    }
}
