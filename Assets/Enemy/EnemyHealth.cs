using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
	Color originalColor = Color.white;
	
    [SerializeField] int difficultyRamp = 1;
	[SerializeField] MeshRenderer mr;

    [Tooltip("Adds amount to maxHitPoints when enemy dies")]

    [SerializeField] int maxHitPoints = 5;
    int currentHitPoints;
    Enemy enemy;

    void Start()
    {
		mr = GetComponentInChildren<MeshRenderer>();
        enemy = GetComponent<Enemy>();
    }
    void OnEnable()
    {
        //set default health
		//set to originalColor onEnable 
		if(mr != null){
			mr.material.color = originalColor;
		}
        currentHitPoints = maxHitPoints;
    }
    void OnParticleCollision(GameObject hit)
    {
        int hitPoints = 1;
        ProcessHit(hitPoints);
    }
    void TriggerDeath()
    {
        gameObject.SetActive(false);
        enemy.RewardGold();
        maxHitPoints += difficultyRamp;
    }
    void ProcessHit(int hitpoints)
    {

		// cause color blink on the the mesh renderer
		StartCoroutine(EnemyFlash());
        currentHitPoints -= hitpoints;
        Debug.Log($"Taking a hit of {hitpoints}");
    }
    void Update()
    {
        if (currentHitPoints <= 0)
        {
            TriggerDeath();

        }
    }
 public IEnumerator EnemyFlash()
 
    {
        mr.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mr.material.color = originalColor; 
        StopCoroutine("EnemyFlash");
    } 
}
