using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PlayerShoot : MonoBehaviour
{
    public Camera playerOrientation;

    PlayerStats stats;

    public KeyCode shootKey;

    public PostProcessVolume volume;
    Vignette vg;
    public Transform projectileSpawnPoint;

    public List<ProjectileSO> availableProjectiles;
    int currentIndex;
    ProjectileSO currentProjectile;

    private void Start()
    {
        stats = FindFirstObjectByType<PlayerStats>();
        vg = volume.profile.GetSetting<Vignette>();
        foreach(ProjectileSO projectile in  stats.projectiles)
        {
            if(projectile.unlocked)
            {
                availableProjectiles.Add(projectile);
            }
        }
        currentProjectile = availableProjectiles[0];
        currentIndex = 0;
    }
    void projectileCycle() {
        float scrollInput = Input.mouseScrollDelta.y;
        if (scrollInput > 0)
            currentIndex++;
        else if (scrollInput < 0)
            currentIndex--;
        currentIndex = Math.Clamp(currentIndex, 0, availableProjectiles.Count - 1);
        currentProjectile = availableProjectiles[currentIndex];
    }
    void Update()
    {
            projectileCycle();
            if(Input.GetKey(shootKey) && currentProjectile.canUse())
            {
                playerOrientation.fieldOfView = Mathf.Lerp(playerOrientation.fieldOfView, 30, .1f);
                vg.intensity.value = Mathf.Lerp(vg.intensity.value, .32f, .1f);
            }
            else
            {
                playerOrientation.fieldOfView = Mathf.Lerp(playerOrientation.fieldOfView, 60, .1f);
                vg.intensity.value = Mathf.Lerp(vg.intensity.value, 0f, .1f);
            }
            if(currentProjectile.canUse() && Input.GetKeyUp(shootKey))
            {
                currentProjectile.count--;
                currentProjectile.currentTimer = currentProjectile.refreshTimer;
                GameObject projectile = Instantiate(currentProjectile.prefab);
                projectile.transform.rotation = playerOrientation.transform.rotation;
                projectile.transform.position = projectileSpawnPoint.transform.position;
                if(currentProjectile.throwForce > 0)
                {
                    projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.up * (currentProjectile.throwForce / 2), ForceMode.Impulse);
                    projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * currentProjectile.throwForce, ForceMode.Impulse);
                }
                
            }
    }
    
    private void FixedUpdate()
    {
        foreach(ProjectileSO projectile in availableProjectiles)
        {
            projectile.currentTimer -= Time.deltaTime;
        }
    }
}