using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHP;
    float _hP;

    private void Awake()
    {
        _hP = maxHP;
    }

    public void ApplyDamage(int dmg)
    {
        _hP -= dmg;
    }

    public float GetHealth()
    {
        return _hP;
    }
}