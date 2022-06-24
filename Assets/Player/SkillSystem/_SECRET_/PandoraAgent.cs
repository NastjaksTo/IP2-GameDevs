using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static PlayerSkillsystem;
using rRandom = System.Random;
using Random = UnityEngine.Random;
using TMPro;

public class PandoraAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private EnemyHealthHandler healthHandler;
    public float health;
    public float maxHealth;

    public Vector3 walkpoint;
    private bool walkPointSet;
    public float walkPointRange;
    private bool hasPatrollingCooldown;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public GameObject projectile;
    public GameObject beam;
    public GameObject orb;
    public GameObject aoeOne;
    public GameObject aoeTwo;
    public GameObject slowOne;
    
    public Transform spawner;
    public int shootingPower;
    
    private bool isDead;
    private bool isCurrentlyAttacking;
    private bool usedDebuff;

    private float savedmspeed;
    private float savedsspeed;
    
    private Animator anim;
    private ThirdPersonController controller;

    private bool isPhaseTwo;
    private bool isPhaseThree;
    private rRandom randomNumber = new rRandom();
    
    public Image healthBar;
    public TextMeshProUGUI textHealthPoints;     
    
    

    public float regenerationTimer;
    public GameObject potioneffect;

    public List<int> potionTickTimer = new List<int>();

    private void Awake()
    {
        maxHealth = health;
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spawner = GameObject.Find("PandoraAttackSpawner").transform;
        anim = GetComponent<Animator>();
        healthHandler = GetComponent<EnemyHealthHandler>();
        healthHandler.Health = (int)health;
        controller = player.GetComponent<ThirdPersonController>();
        savedmspeed = controller.moveSpeed;
        savedsspeed = controller.SprintSpeed;
    }

    private void Update()
    {
        healthBar.fillAmount = healthHandler.Health / maxHealth;
        textHealthPoints.text = healthHandler.Health.ToString();
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if(!playerInSightRange && !playerInAttackRange) Patrolling();
        if(playerInSightRange) anim.SetBool("inBattle", true); else anim.SetBool("inBattle", false);
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        GetDamage("hit", "die", 5000);
    }

    private void Patrolling()
    {
        if (isCurrentlyAttacking) return;
        if(isDead) return;
        if (hasPatrollingCooldown) return;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkpoint);
        anim.SetBool("walking", true);
        anim.SetBool("inBattle", false);
        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            StartCoroutine(patrollingCooldown());
        }
    }
    
    IEnumerator patrollingCooldown()
    {
        hasPatrollingCooldown = true;
        yield return new WaitForSeconds(.66f);
        anim.SetBool("walking", false);
        yield return new WaitForSecondsRealtime(3f);
        hasPatrollingCooldown = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        var position = transform.position;
        walkpoint = new Vector3(position.x + randomX, position.y, position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    
    private void ChasePlayer()
    {
        if(isCurrentlyAttacking) return;
        if(isDead) return;
        anim.SetBool("walking", true);
        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer()
    {
        anim.SetBool("walking", false);
        agent.SetDestination(transform.position);
        
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);

        if (!alreadyAttacked && !isDead && !isCurrentlyAttacking)
        {
            //ATTACK
            StartCoroutine(CurrentlyAttacking());
            
            if (!isPhaseTwo && !isPhaseThree)
            {
                int chooseAttack = randomNumber.Next(0, 5);
                if (chooseAttack < 4)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackOne", true);
                }
                else if (chooseAttack == 4 && !usedDebuff)
                {
                    if (alreadyAttacked || usedDebuff) return;
                    usedDebuff = true;
                    anim.SetBool("debuffOne", true);
                }
            }

            if (isPhaseTwo && !isPhaseThree && !alreadyAttacked)
            {
                int chooseAttack = randomNumber.Next(0, 9);
                if (chooseAttack > 1 && chooseAttack < 5)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackOne", true);
                }
                else if (chooseAttack == 1 && !usedDebuff)
                {
                    if (alreadyAttacked || usedDebuff) return;
                    usedDebuff = true;
                    anim.SetBool("debuffOne", true);
                }
                else if (chooseAttack > 4)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackTwo", true);
                }
            }
            
            if (isPhaseThree)
            {
                int chooseAttack = randomNumber.Next(0, 12);
                if (chooseAttack > 1 && chooseAttack < 5)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackOne", true);
                }
                else if (chooseAttack == 1 && !usedDebuff)
                {
                    if (alreadyAttacked || usedDebuff) return;
                    usedDebuff = true;
                    anim.SetBool("debuffOne", true);
                }
                else if(chooseAttack > 4 && chooseAttack < 9)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackTwo", true);

                }
                else if(chooseAttack > 9)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackThree", true);
                }
            }
        }
    }

    IEnumerator CurrentlyAttacking()
    {
        isCurrentlyAttacking = true;
        yield return new WaitForSecondsRealtime(1.65f);
        isCurrentlyAttacking = false;
    }
    
    IEnumerator SlowEffect()
    {
        controller.moveSpeed = 3.5f;
        controller.SprintSpeed = 5.5f;
        yield return new WaitForSecondsRealtime(5);
        controller.moveSpeed = savedmspeed;
        controller.SprintSpeed = savedsspeed;
        usedDebuff = false;
    }

    public void DebuffOne()
    {
        alreadyAttacked = true;
        var debuffOne = Instantiate(slowOne, player.position, Quaternion.identity);
        anim.SetBool("debuffOne", false);
        StartCoroutine(SlowEffect());
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    
    public void AttackOne()
    {
        alreadyAttacked = true;
        var attackOne = Instantiate(projectile, spawner.position, Quaternion.identity);
        attackOne.GetComponent<Rigidbody>().velocity = (player.position - attackOne.transform.position).normalized * shootingPower;
        anim.SetBool("attackOne", false);
        Destroy(attackOne, 5f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    
    public void AttackTwo()
    {
        alreadyAttacked = true;
        var attackTwo = Instantiate(beam, spawner.position, Quaternion.identity);
        Vector3 playerpos = player.position + transform.up*3f;
        attackTwo.transform.LookAt(playerpos);
        anim.SetBool("attackTwo", false);
        Destroy(attackTwo, 1.2f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    
    public void AttackThree()
    {
        alreadyAttacked = true;
        var attackThree = Instantiate(orb, player.position+transform.up*2.33f, Quaternion.identity);
        anim.SetBool("attackThree", false);
        Destroy(attackThree, 25f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    
    public void ScreamOne()
    {
        isPhaseTwo = true;
        agent.speed += 5;
        agent.SetDestination(transform.position);
        alreadyAttacked = true;
        var aoEOne = Instantiate(aoeOne, transform.position + transform.up * 2f, Quaternion.identity);
        Destroy(aoEOne, 3);
        anim.SetBool("screamingOne", false);
    }
    
    public void ScreamTwo()
    {
        isPhaseThree = true;
        agent.speed += 5;
        agent.SetDestination(transform.position);
        alreadyAttacked = true;
        applypotion(100);
        PlayPotionEffect();
        Instantiate(aoeTwo, transform.position + transform.up * 3.5f, Quaternion.identity);
        anim.SetBool("screamingTwo", false);
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
    
    //....
    public void GetDamage(string Hit, string Die, int Exp)
    {
        if (healthHandler.Hit)
        {
            if (healthHandler.Health <= health * 0.6f && !isPhaseTwo)
            {
                anim.SetBool("screamingOne", true);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            if (healthHandler.Health <= health * 0.3f && !isPhaseThree && isPhaseTwo)
            {
                anim.SetBool("screamingTwo", true);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            if (healthHandler.Dead && !isDead)
            {
                isDead = true;
                anim.Play(Die);
                agent.speed = 0;
                Destroy(healthBar, 0.25f);
                Destroy(textHealthPoints, 0.25f);
                playerskillsystem.playerlevel.AddExp(Exp);
                Destroy(GameObject.Find("PandoraAoETwo(Clone)"), 0.25f);
                Destroy(gameObject, 2.0f);
            }
        }
    }
    
    // REGENERATION EFFECT
    public IEnumerator regeneratingHealth()
    {
        regenerationTimer = 0.2f;
        while (potionTickTimer.Count > 0)
        {
            for (int i = 0; i < potionTickTimer.Count; i++)
            {
                potionTickTimer[i]--;
            }
            if (healthHandler.Health < maxHealth) healthHandler.Health += 2;
            else potionTickTimer.Clear();
            potionTickTimer.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(regenerationTimer);
        }
    }

    public void applypotion(int ticks)
    {
        if (potionTickTimer.Count <= 0)
        {
            potionTickTimer.Add(ticks);
            StartCoroutine(regeneratingHealth());
        }
        else potionTickTimer.Add(ticks);
    }
    
    public void PlayPotionEffect()
    {
        var newPotionEffect = Instantiate(potioneffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newPotionEffect.transform.parent = gameObject.transform;
        Destroy(newPotionEffect, 5f);
    }
}
