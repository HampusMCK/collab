using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHP;
    int _hP;

    public void ApplyDamage(int dmg)
    {
        _hP -= dmg;
    }

    public int GetHealth()
    {
        return _hP;
    }
}
