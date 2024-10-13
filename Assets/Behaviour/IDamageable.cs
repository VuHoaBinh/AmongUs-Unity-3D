using UnityEngine;

public interface IDamageable
{
    void TakeHit(float Damege, RaycastHit hit);
    void TakeDamage(float Damege);
}
