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
    public EnemyHealthHandler healthHandler;
    public float health;
    public float maxHealth = 10000;
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
    public List<float> pandoraHealtTimer = new List<float>();
    public bool isInvincible;
    private bool hasDodgeCooldown;
    private bool isStunned;
    private bool hasBlockCooldown;
    public GameObject shield;
    
    /// <summary>
    /// Gets all references and assigns the values of pandora.
    /// </summary>
    private void Awake()
    {
        healthBar = GameObject.Find("RayaHealthRepresentation").GetComponent<Image>();
        textHealthPoints = GameObject.Find("RayahealthValue").GetComponent<TextMeshProUGUI>();
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

    /// <summary>
    /// Resets all values and bools of pandora.
    /// </summary>
    public void ResetRaya()
    {
        healthHandler.Health = maxHealth;
        savedmspeed = controller.moveSpeed;
        savedsspeed = controller.SprintSpeed;
        isInvincible = false;
        hasDodgeCooldown = false;
        hasBlockCooldown = false;
        isStunned = false;
        combatSystem.shouldPandoraBlock = false;
        isCurrentlyAttacking = false;
        alreadyAttacked = false;
        isPhaseTwo = false;
        usedDebuff = false;
        isPhaseThree = false;
        isDead = false;
        hasPatrollingCooldown = false;
        ResetAttack();
    }

    /// <summary>
    /// Creates spheres around pandora which check for certain objects inside them.
    /// If an player is inside the sightrange, pandora starts to chase him.
    /// If an Player is inside the attackrange, pandora starts to attack him.
    /// If an Spell is inside the spellrange, pandora dodges the spell.
    /// If the player is attacking inside the blockrange, pandora starts to block.
    /// </summary>
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
        GetDamage("hit", "die", 1001);
    }

    /// <summary>
    /// Animation Event, which gets called when pandora dodges a spell.
    /// Starts the cooldown of dodging.
    /// Makes her immune to damage while dodging.
    /// </summary>
    public void Dodge()
    {
        if (!isInvincible)
        {
            isInvincible = true;
        }
        StartCoroutine(dodgeCooldown());
    }
    
    /// <summary>
    /// Animation Event, which gets called at the end of the dodge animation.
    /// Resets the dodging values.
    /// </summary>
    public void StopDodging()
    {
        anim.ResetTrigger("dodging");
        if (isInvincible)
        {
            isInvincible = false;
        }
    }
    
    /// <summary>
    /// Animation Event, which gets called when pandora start blocking.
    /// Starts the cooldown for blocking.
    /// Makes her immune to damage while blocking.
    /// </summary>
    public void Block()
    {
        shield.SetActive(true);
        if (!isInvincible)
        {
            isInvincible = true;
        }
        StartCoroutine(blockCooldown());
        Invoke(nameof(StopBlocking), 2f);
    }
    
    /// <summary>
    /// Animation Event, which gets called at the end of the block animation.
    /// Resets the blocking values.
    /// </summary>
    public void StopBlocking()
    {
        shield.SetActive(false);
        anim.ResetTrigger("dodging");
        if (isInvincible)
        {
            isInvincible = false;
        }
    }
    
    /// <summary>
    /// Starts the block cooldown.
    /// </summary>
    /// <returns></returns>
    IEnumerator blockCooldown()
    {
        hasBlockCooldown = true;
        yield return new WaitForSeconds(10f);
        hasBlockCooldown = false;
    }
    
    /// <summary>
    /// Starts the dodge cooldown.
    /// </summary>
    /// <returns></returns>
    IEnumerator dodgeCooldown()
    {
        hasDodgeCooldown = true;
        yield return new WaitForSeconds(30f);
        hasDodgeCooldown = false;
    }
    
    /// <summary>
    /// Sets the destination to the players location. So Pandora chases after him.
    /// </summary>
    private void ChasePlayer()
    {
        if (isStunned) return;
        if(isCurrentlyAttacking) return;
        if(isDead) return;
        anim.SetBool("walking", true);
        agent.SetDestination(player.position);
    }
    
    /// <summary>
    /// Starts attacking the player.
    /// Checks for the current phase pandora is in and casts spells randomly.
    /// </summary>
    private void AttackPlayer()
    {
        if (isStunned) return;
        anim.SetBool("walking", false);
        agent.SetDestination(transform.position);
        Vector3 position = new Vector3 (player.position.x, transform.position.y, player.position.z);
        transform.LookAt(position);
        if (!alreadyAttacked && !isDead && !isCurrentlyAttacking)
        {
            StartCoroutine(CurrentlyAttacking());
            // PHASE 1
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
            // PHASE 2
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
            // PHASE 3
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

    /// <summary>
    /// Starts the cooldown for attacks.
    /// </summary>
    /// <returns></returns>
    IEnumerator CurrentlyAttacking()
    {
        isCurrentlyAttacking = true;
        yield return new WaitForSeconds(2f);
        isCurrentlyAttacking = false;
    }
    
    /// <summary>
    /// Starts the duration for the slow effect.
    /// </summary>
    /// <returns></returns>
    IEnumerator SlowEffect()
    {
        controller.moveSpeed = 3.5f;
        controller.SprintSpeed = 5.5f;
        yield return new WaitForSeconds(10f);
        controller.moveSpeed = savedmspeed;
        controller.SprintSpeed = savedsspeed;
        usedDebuff = false;
    }

    /// <summary>
    /// Animation Event, which gets called when pandora uses her debuff.
    /// Starts the SlowEffect.
    /// </summary>
    public void DebuffOne()
    {
        alreadyAttacked = true;
        Instantiate(slowOne, player.position, Quaternion.identity);
        StartCoroutine(SlowEffect());
        Invoke(nameof(ResetAttack), timeBetweenAttacks/2);
        anim.ResetTrigger("debuffOne");
    }
    
    /// <summary>
    /// Animation Event, which gets called when pandora uses her first attack.
    /// Instantiates the gameobject (fireball).
    /// Destroys it after 5 seconds.
    /// </summary>
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
    
    /// <summary>
    /// Animation Event, which gets called when pandora uses her second attack.
    /// Instantiates the gameobject (lightning beam).
    /// Destroys it after 1.2seconds
    /// </summary>
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
    
    /// <summary>
    /// Animation Event, which gets called when pandora uses her third attack.
    /// Instantiates the gameobject (damage orbs).
    /// Destroys them after 20 seconds.
    /// </summary>
    public void AttackThree()
    {
        alreadyAttacked = true;
        var attackThree = Instantiate(orb, player.position+transform.up*2.33f, Quaternion.identity);
        Destroy(attackThree, 20f);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
        anim.SetBool("attackThree", false);
    }
    
    /// <summary>
    /// Animation Event, which gets called when pandora reaches phase 2.
    /// Play Sounds.
    /// Instantiate the gameobject (AoE damage spell).
    /// Destroys the gameobject after 3 seconds.
    /// </summary>
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
    
    /// <summary>
    /// Animation Event, which gets called when pandora reaches phase 3.
    /// Applies potiontick to heal pandora.
    /// Instantiates the gameobject (Healeffect).
    /// Plays Sound.
    /// Destroys the gameobject after 11 seconds.
    /// </summary>
    public void ScreamTwo()
    {
        isPhaseThree = true;
        agent.speed += 5;
        agent.SetDestination(transform.position);
        alreadyAttacked = true;
        applypotion(100);
        PlayPotionEffect();
        AudioSource.PlayClipAtPoint(spellsounds[3],transform.position, SpellAudioVolume);
        var aoetwo = Instantiate(aoeTwo, transform.position + transform.up * 3.5f, Quaternion.identity);
        Destroy(aoetwo, 11f);
        anim.SetBool("screamingTwo", false);
    }
    
    /// <summary>
    /// Resets the bool alreadyAttacked. So pandora can start attacking again.
    /// </summary>
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    
    /// <summary>
    /// Stuns pandora, making her do nothing for a set amount of time.
    /// </summary>
    /// <param name="Duration">Duration of the stun.</param>
    public void GetStunned(float Duration)
    {
        agent.SetDestination(transform.position);
        isStunned = true;
        anim.SetBool("Stunned", true);
        StartCoroutine(Stunned(Duration));
    }
    
    /// <summary>
    /// Starts the duration of the stun.
    /// </summary>
    /// <param name="time">Duration of the stun.</param>
    /// <returns></returns>
    public IEnumerator Stunned(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("Stunned", false);
        isStunned = false;
    }

    /// <summary>
    /// Draws gizmos for each sphere (attackrange, sightrange, dodgerange);
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dodgeRange);
    }
    
    
    /// <summary>
    /// Check if pandora is getting hit.
    /// If thats the case check how much HP pandora has and set her new phase at 66% HP to Phase 2, and to Phase 3 at 33%.
    /// If Pandora reaches 0 HP, she dies.
    /// </summary>
    /// <param name="Hit">Animation name GetHit</param>
    /// <param name="Die">Animation name of the die animation</param>
    /// <param name="Exp">The amount of experience the player gets for defeating pandora.</param>
    public void GetDamage(string Hit, string Die, int Exp)
    {
        if (isInvincible) return;
        if (healthHandler.Hit)
        {
            if (healthHandler.Health <= maxHealth * 0.6f && !isPhaseTwo)
            {
                anim.SetBool("screamingOne", true);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            
            if (healthHandler.Health <= maxHealth * 0.3f && !isPhaseThree && isPhaseTwo)
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

    /// <summary>
    /// When pandora dies stop the game and load the credit scene.
    /// </summary>
    private void EndGame()
    {
        SceneManager.LoadScene("ScrollingCredits");
    }
    
    /// <summary>
    /// Increases the health of the player over time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator regeneratingHealth()
    {
        regenerationTimer = 0.1f;
        while (pandoraHealtTimer.Count > 0)
        {
            for (int i = 0; i < pandoraHealtTimer.Count; i++)
            {
                pandoraHealtTimer[i]--;
            }
            if (healthHandler.Health < maxHealth) healthHandler.Health += 20;
            else pandoraHealtTimer.Clear();
            pandoraHealtTimer.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(regenerationTimer);
        }
    }
    
    /// <summary>
    /// Calls the coroutine to regenerate health over time, for each tick.
    /// </summary>
    /// <param name="ticks">The amount of ticks.</param>
    public void applypotion(float ticks)
    {
        if (pandoraHealtTimer.Count <= 0)
        {
            pandoraHealtTimer.Add(ticks);
            StartCoroutine(regeneratingHealth());
        }
        else pandoraHealtTimer.Add(ticks);
    }
    
    /// <summary>
    /// Plays the potion effect when a potion is used.
    /// </summary>
    public void PlayPotionEffect()
    {
        var newPotionEffect = Instantiate(potioneffect, transform.position + (Vector3.up * 0.35f), transform.rotation * Quaternion.Euler (-90f, 0f, 0f));
        newPotionEffect.transform.parent = gameObject.transform;
        Destroy(newPotionEffect, 5f);
    }
}
