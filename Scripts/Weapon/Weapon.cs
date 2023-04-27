using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public enum WeaponType
    {
        Melee,
        Ranged
    }
    public WeaponType Type;
    public GameObject WeaponPrefab;
    public float AttackCooldown;
    public AudioClip AttackSound;
}
