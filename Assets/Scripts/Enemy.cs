using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Random = System.Random;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject[] players;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    
    public float UpAttack;
    public float pushForce;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Transform spawnBullet;
    public GameObject projectile;

    
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    
    public GameObject EffectBloode;
   
    public float radius = 5;
    
    private Animator _animator;
    private float _dist;
    
    void Start()
    {
        players = GameObject.FindWithTag("Player").GetComponent<Weapons>().GetGameWeapons();
        EffectBloode.GetComponent<ParticleSystem>().Stop(false);
        _animator = GetComponent<Animator>();
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private Transform player;
    private float rateDameg = 0;
    
    void Update()
    {
        if (health < 0)
        {
            _animator.SetTrigger("Die");
            if (Time.time > rateDameg -4.5)
                if (health <= 0) 
                    Destroy(gameObject);
        }
        if (players[0].activeSelf)
            player = players[0].transform;
        if (players[1].activeSelf)
            player = players[1].transform;
        if (Time.time > rateDameg)
        {
            EffectBloode.GetComponent<ParticleSystem>().Stop(false);
        }
        _dist = Vector3.Distance(player.position, transform.position);
        Follow();
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Follow()
    {
        if (radius > _dist && health > 0)
        {
            
            
            float x2, y2, z2;
            x2 = 2 * transform.position.x - player.position.x;
            y2 = 2 * transform.position.y - player.position.y;
            z2 = 2 * transform.position.z - player.position.z;
            //_navMeshAgent.SetDestination(new Vector3(x2,y2,z2));
            //_navMeshAgent.SetDestination(player.position);
        }
        else
        {
            _animator.SetBool("Run Forward",false);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            _animator.SetTrigger("Take Damage");
            EffectBloode.GetComponent<ParticleSystem>().Play();
            rateDameg = Time.time + 5;
            health -= 9;
        }
    }
    private void Patroling()
    {
        
        if (!walkPointSet) {SearchWalkPoint(); _animator.SetBool("Walk Forward",true);}

        if (walkPointSet)
        {
            if(agent.Warp(walkPoint))
            agent.SetDestination(walkPoint); 
            _animator.SetBool("Walk Forward",true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        _animator.SetBool("Walk Forward", false);
        _animator.SetBool("Run Forward",true);
        Random random = new Random();
        random.Next(-int.Parse(walkPointRange.ToString()), int.Parse(walkPointRange.ToString()));
        random.NextDouble();
        //Calculate random point in range
        float randomZ = random.Next(-int.Parse(walkPointRange.ToString()), int.Parse(walkPointRange.ToString()));
        float randomX = random.Next(-int.Parse(walkPointRange.ToString()), int.Parse(walkPointRange.ToString()));

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _animator.SetBool("Walk Forward", false);
        _animator.SetBool("Run Forward",true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        if(agent.Warp(transform.position))
            agent.SetDestination(transform.position);
        
        
        

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            _animator.SetBool("Run Forward",false);
            _animator.SetTrigger("Stab Attack");
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, spawnBullet.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
            rb.AddForce(transform.up * UpAttack, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}