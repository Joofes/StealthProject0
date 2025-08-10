using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
   public int id;
   public int count; 
   public bool unlocked;

   public abstract bool canUse();
}

[CreateAssetMenu(fileName = "New Projectile", menuName = "Items/Projectile")]
public class ProjectileSO : Item
{
   public string projectileName;
   public GameObject prefab;
   public float refreshTimer, currentTimer; 

   public float throwForce;

    public override bool canUse()
    {
        if(currentTimer < 0 && count > 0)
            return true;
        else
         return false;
    }
}

[CreateAssetMenu(fileName = "New Trap", menuName = "Items/Trap")]
public class TrapSO : Item
{
   public string trapName;
   public string prefab;

   public override bool canUse()
    {
        throw new System.NotImplementedException();
    }
}
