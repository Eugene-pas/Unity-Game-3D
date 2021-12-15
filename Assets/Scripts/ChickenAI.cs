using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ChickenAI : MonoBehaviour
{
    public GameObject player;
    public float radius = 5;
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private float _dist;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _dist = Vector3.Distance(player.transform.position, transform.position);
        Follow();
    }

    private void Follow()
    {
        if (radius > _dist)
        {
            _animator.SetBool("Run",true);
            _navMeshAgent.enabled = true;
            float x2, y2, z2;
            x2 = 2 * transform.position.x - player.transform.position.x;
            y2 = 2 * transform.position.y - player.transform.position.y;
            z2 = 2 * transform.position.z - player.transform.position.z;
            //_navMeshAgent.SetDestination(new Vector3(x2,y2,z2));
            _navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            _animator.SetBool("Run",false);
            _navMeshAgent.enabled = false;
        }
        
    }
    
}
