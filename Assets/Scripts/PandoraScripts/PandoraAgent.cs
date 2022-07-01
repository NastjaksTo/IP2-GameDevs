using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static PlayerSkillsystem;
using rRandom = System.Random;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;
using static CombatSystem;

public class PandoraAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsSpell, whatIsWeapon;

    private EnemyHealthHandler healthHandler;
    public float health;
    public float maxHealth;
    
    private bool walkPointSet;
    private bool hasPatrollingCooldown;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    public float sightRange, attackRange, dodgeRange, blockRange;
    public bool playerInSightRange, playerInAttackRange, spellInDodgeRange, weaponInBlockRange;

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
    
    private Image healthBar;
    private TextMeshProUGUI textHealthPoints;     
    
    public AudioClip[] spellsounds;
    [Range(0, 1)] public float SpellAudioVolume = 0.5f;

    public float regenerationTimer;
    public GameObject potioneffect;

    public List<int> potionTickTimer = new List<int>();

    public bool isInvincible;
    private bool hasDodgeCooldown;

    private bool isStunned;
    
    private bool hasBlockCooldown;
    public GameObject shield;
    
    private void Awake()
    {
        healthBar = GameObject.Find("RayaHealthRepresentation").GetComponent<Image>();
        textHealthPoints = GameObject.Find("RayahealthValue").GetComponent<TextMeshProUGUI>();
        maxHealth = health;
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        spawner = GameObject.Find("PandoraAttackSpawner").transform;
        anim = GetComponent<Animator>();
        healthHandler = GetComponent<EnemyHealthHandler>();
        healthHandler.Health = health;
        controller = player.GetComponent<ThirdPersonController>();
        savedmspeed = controller.moveSpeed;
        savedsspeed = controller.SprintSpeed;
    }

    private void Update()
    {
        healthBar.fillAmount = healthHandler.Health / maxHealth;
        textHealthPoints.text = healthHandler.Health.ToString();
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        spellInDodgeRange = Physics.CheckSphere(transform.position, dodgeRange, whatIsSpell);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        weaponInBlockRange = Physics.CheckSphere(transform.position, blockRange, whatIsWeapon);
        if(spellInDodgeRange && !isInvincible && !hasDodgeCooldown) anim.SetTrigger("dodging");
        if(weaponInBlockRange && combatSystem.shouldPandoraBlock && !isInvincible && !hasBlockCooldown && !isStunned) anim.SetTrigger("blocking");
        if(playerInSightRange) anim.SetBool("inBattle", true); else anim.SetBool("inBattle", false);
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInAttackRange && playerInSightRange) AttackPlayer();
        GetDamage("hit", "die", 5000);
    }

    public void Dodge()
    {
        if (!isInvincible)
        {
            isInvincible = true;
        }
        StartCoroutine(dodgeCooldown());
    }
    
    
    public void StopDodging()
    {
        anim.ResetTrigger("dodging");
        if (isInvincible)
        {
            isInvincible = false;
        }
    }
    
    public void Block()
    {
        shield.SetActive(true);
        if (!isInvincible)
        {
            isInvincible = true;
        }
        StartCoroutine(blockCooldown());
    }
    
    
    public void StopBlocking()
    {
        shield.SetActive(false);
        anim.ResetTrigger("dodging");
        if (isInvincible)
        {
            isInvincible = false;
        }
    }
    
    IEnumerator blockCooldown()
    {
        hasBlockCooldown = true;
        yield return new WaitForSeconds(5f);
        hasBlockCooldown = false;
    }
    
    IEnumerator dodgeCooldown()
    {
        hasDodgeCooldown = true;
        yield return new WaitForSeconds(30f);
        hasDodgeCooldown = false;
    }
    
    private void ChasePlayer()
    {
        if (isStunned) return;
        if(isCurrentlyAttacking) return;
        if(isDead) return;
        anim.SetBool("walking", true);
        agent.SetDestination(player.position);
    }
    
    private void AttackPlayer()
    {
        if (isStunned) return;
        anim.SetBool("walking", false);
        agent.SetDestination(transform.position);
        
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);

        if (!alreadyAttacked && !isDead && !isCurrentlyAttacking)
        {
            //ATTACK
            StartCoroutine(CurrentlyAttacking());
            
            if (!isPhaseTwo && !isPhaseThree && !alreadyAttacked)
            {
                int chooseAttack = randomNumber.Next(1, 101);
                if (chooseAttack <= 80)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackOne", true);
                }
                else if (!usedDebuff && !alreadyAttacked)
                {
                    if (alreadyAttacked || usedDebuff) return;
                    usedDebuff = true;
                    anim.SetTrigger("debuffOne");
                }
            }

            if (isPhaseTwo && !isPhaseThree && !alreadyAttacked)
            {
                int chooseAttack = randomNumber.Next(1, 101);
                if (chooseAttack >= 0 && chooseAttack <= 15)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackOne", true);
                }
                else if (chooseAttack >= 16 && chooseAttack <= 26 && !usedDebuff)
                {
                    if (alreadyAttacked || usedDebuff) return;
                    usedDebuff = true;
                    anim.SetTrigger("debuffOne");
                }
                else
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackTwo", true);
                }
            }
            
            if (isPhaseThree && !alreadyAttacked)
            {
                int chooseAttack = randomNumber.Next(1, 111);
                if (chooseAttack >= 0 && chooseAttack <= 15)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackOne", true);
                }
                else if (chooseAttack >= 16 && chooseAttack <= 26 && !usedDebuff)
                {
                    if (alreadyAttacked || usedDebuff) return;
                    usedDebuff = true;
                    anim.SetTrigger("debuffOne");
                }
                else if(chooseAttack >= 27 && chooseAttack <= 80)
                {
                    if (alreadyAttacked) return;
                    anim.SetBool("attackTwo", true);

                }
                else
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
        yield return new WaitForSeconds(2f);
        isCurrentlyAttacking = false;
    }
    
    IEnumerator SlowEffect()
    {
        controller.moveSpeed = 3.5f;
        controller.SprintSpeed = 5.5f;
        yield return new WaitForSeconds(10f);
        controller.moveSpeed = savedmspeed;
        controller.SprintSpeed = savedsspeed;
        usedDebuff = false;
    }

    public void DebuffOne()
    {
        alreadyAttacked = true;
        Instantiate(slowOne, player.position, Quaternion.identity);
        StartCoroutine(SlowEffect());
        Invoke(nameof(ResetAttack), timeBetweenAttacks/2);
        anim.ResetTrigger("debuffOne");
    }
    
    public void AttackOne()
    {
        alreadyAttacked = true;
        AudioSource.PlayClipAtPoint(spellsounds[0],spawner.position, SpellAudioVolume);
        var attackOne = Instantiate(projectile, spawner.position, Quaternion.identity);
        attackOne.GetComponent<Rigidbody>().velocity = (player.position - attackOne.transform.position).normalized * shootingPower;
        Destroy(attackOne, 5f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        anim.SetBool("attackOne", false);
    }
    
    public void AttackTwo()
    {
        alreadyAttacked = true;
        AudioSource.PlayClipAtPoint(spellsounds[1],spawner.position, SpellAudioVolume);
        var attackTwo = Instantiate(beam, spawner.position, Quaternion.identity);
        Vector3 playerpos = player.position + transform.up*3f;
        attackTwo.transform.LookAt(playerpos);
        Destroy(attackTwo, 1.2f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        anim.SetBool("attackTwo", false);
    }
    
    public void AttackThree()
    {
        alreadyAttacked = true;
        var attackThree = Instantiate(orb, player.position+transform.up*2.33f, Quaternion.identity);
        Destroy(attackThree, 25f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        anim.SetBool("attackThree", false);
    }
    
    public void ScreamOne()
    {
        isPhaseTwo = true;
        agent.speed += 5;
        agent.SetDestination(transform.position);
        alreadyAttacked = true;
        AudioSource.PlayClipAtPoint(spellsounds[2],transform.position, SpellAudioVolume);
        AudioSource.PlayClipAtPoint(spellsounds[3],transform.position, SpellAudioVolume);
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
        AudioSource.PlayClipAtPoint(spellsounds[3],transform.position, SpellAudioVolume);
        Instantiate(aoeTwo, transform.position + transform.up * 3.5f, Quaternion.identity);
        anim.SetBool("screamingTwo", false);
    }
    
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    public void GetStunned(float Duration)
    {
        agent.SetDestination(transform.position);
        isStunned = true;
        anim.SetBool("Stunned", true);
        StartCoroutine(Stunned(Duration));
    }
    public IEnumerator Stunned(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("Stunned", false);
        isStunned = false;
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dodgeRange);
    }
    
    
    //....
    public void GetDamage(string Hit, string Die, int Exp)
    {
        if (isInvincible) return;
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
                Invoke(nameof(EndGame), 3.9f);
                Destroy(gameObject, 4f);
            }
        }
    }

    private void EndGame()
    {
        SceneManager.LoadScene("ScrollingCredits");
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
