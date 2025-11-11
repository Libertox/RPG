

using Player;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
            Debug.Log("Take Damage: " + damage);
        }
    }
}
