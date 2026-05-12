using System;
using Unity.VisualScripting;
using UnityEngine;

public class BeeSwarmBehavior : MonoBehaviour
{
    Enemy enemyComponent;
    CircleCollider2D circleCollider;
    ParticleSystem.MainModule beeParticle;

    int startingSwarmSize;
    float startingColliderRadius;
    int startingHealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyComponent = GetComponent<Enemy>();
        circleCollider = GetComponent<CircleCollider2D>();
        beeParticle = GetComponentInChildren<ParticleSystem>().main;

        startingColliderRadius = circleCollider.radius;
        startingSwarmSize = beeParticle.maxParticles;
        startingHealth = enemyComponent.Health;

        enemyComponent.OnEnemyDamaged += OnBeeDamaged;
        enemyComponent.OnEnemyDeath += OnBeeDeath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBeeDamaged(object sender, Enemy e)
    {
        float healthPercent = (float)enemyComponent.Health/(float)startingHealth;
        //Debug.Log($"Calculated health percent as {healthPercent} from enemy health currently at {enemyComponent.Health} divided by original value of {startingHealth}");

        circleCollider.radius = healthPercent * startingColliderRadius;
        beeParticle.maxParticles = Mathf.RoundToInt(healthPercent * (float)startingSwarmSize);

        //Debug.Log($"Setting particle count to: {Mathf.RoundToInt(healthPercent * (float)startingSwarmSize)} with a reduced size of {healthPercent} from original count of {startingSwarmSize}");
    }

    void OnBeeDeath(object sender , Enemy e)
    {
        enemyComponent.OnEnemyDamaged -= OnBeeDamaged;
        enemyComponent.OnEnemyDeath -= OnBeeDeath;
    }
}
