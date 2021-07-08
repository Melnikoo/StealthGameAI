using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Patrol : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Animator anim;
    private float defaultSpeed;
    AnimatorStateInfo animInfo;
    

    EnemyAI enemy;


    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GotoNextPoint();
        defaultSpeed = agent.speed;
        enemy = GetComponent<EnemyAI>();
        StartCoroutine(UpdateCoroutine());
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        //agent.autoBraking = false;

        
    }


    
    void GotoNextPoint()
    {
        
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    void HeardNoise(Transform noiseSource)
    {
        agent.destination = noiseSource.position;
        

    }


    void Update()
    {
      /*  if (!agent.pathPending && agent.remainingDistance < 0.1f && enemy.heardNoise)
        {
            enemy.heardNoise = false;
        }
      */
    }

    IEnumerator UpdateCoroutine()
    {
        while(true)
        {         
            
                if (!agent.pathPending && agent.remainingDistance < 0.1f && !enemy.heardNoise)
                {


                    defaultSpeed = agent.speed;
                    anim.SetBool("PatrolLookAround", true);
                  
                    agent.speed = 0;
                    if(enemy.onAlert == false)
                        yield return new WaitForSeconds(4.45f);
                    else
                        yield return new WaitForSeconds(9.3f);
                    anim.SetBool("PatrolLookAround", false);
                    GotoNextPoint();
                    agent.speed = defaultSpeed;
                   
         
                  //  yield return new WaitForSeconds(0.5f);
                 
                }
                else
                    yield return null;          
        }
    }
}