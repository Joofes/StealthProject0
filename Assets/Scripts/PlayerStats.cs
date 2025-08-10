using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public int level;
    public int maxHealth;
    public int health;
    public int money;
    public int ropeAmnt;
    public Slider healthBar;

    public List<ProjectileSO> projectiles = new List<ProjectileSO>();
    private void Start()
    {
        healthBar.maxValue = maxHealth;
        health = maxHealth;
    }
    private void Update()
    {
        healthBar.value = health;
    }
}
