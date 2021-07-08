using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class EnemyAI : MonoBehaviour
{

    NavMeshAgent agent;
    Animator anim;
    GameObject player;
    PlayerStatusControl playerStatus;

    SpotBar spotBar;
    public GameObject spotBarUI;
    
    private Light UIlight;//cone to indicate guard's vision
    public GameObject sightpoint;//Guard's sight ray begins from here
    public GameObject head;//UILight follows the head
    Patrol patrol;

    public GameObject torch;//enables when alarm rings
    public bool isDead = false;
    public bool isChasing = false;
    public bool heardNoise;

    AudioSource audio;
    public AudioClip clip1;
    public AudioClip clip2;


    public GameObject[] bodyList;//continuosly updated array of corpses
    public bool onAlert;
    public float coneWidth;
    public float coneRange;
    public float noiseHearDistance;

    bool isInSight = false;
    float timeToSpot = 3f;//This is the main fuzzy variable
    float timer = 0f;

    RaycastHit hit;
    Ray ray;


    // Start is called before the first frame update
    void Start()
    {
        patrol = GetComponent<Patrol>();
        audio = GetComponent<AudioSource>();
        onAlert = false;

        spotBar = spotBarUI.GetComponent<SpotBar>();
        UIlight = GetComponentInChildren<Light>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatusControl>();
        
     
    }

    // Update is called once per frame
    void Update()
    {

        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        if (!isDead)
        {


            if (!agent.pathPending && agent.remainingDistance < 0.1f && heardNoise)
            {
                heardNoise = false;
                
            }

            if((transform.position - player.transform.position).magnitude <= noiseHearDistance)
            {
                if(playerStatus.noiseLevel >= 99 && !heardNoise)
                {
                    HeardNoise(player.transform);
                    
                }
            }
            //------------------------------------------LOOKING FOR PLAYER---------------------------------------------------
            UIlight.color = Color.white;

            

            if (Vector3.Distance(head.transform.position, player.transform.position) <= coneRange)
            {
                
                if (Vector3.Angle(head.transform.forward, player.transform.position - transform.position) <= coneWidth) //Check if the player is within the view cone
                {
                    Debug.DrawLine(sightpoint.transform.position, player.transform.position, Color.green);
                    Vector3 distance = player.transform.position - sightpoint.transform.position;
                    ray = new Ray(sightpoint.transform.position, distance);


                    if (Physics.Raycast(ray, out hit, coneRange, layerMask)) //Check if Nothing blocks the eyesight
                    {
                        Debug.Log(hit.collider.name);
                        if (hit.collider.gameObject.tag == "Player")
                        {

                            UIlight.color = Color.red;
                            isInSight = true;//The player is being seen by the guard and if this state does not break for the "timeToSpot" amount of time, he will be chased by the guards
                            //HasSight();
                        }
                        else
                            isInSight = false;
                    }
                    else
                        isInSight = false;
                }
                else
                    isInSight = false;
            }
            //-------------------------------------------------Checking if the guard sees the player for long enough-------------------------------------
            
            if (isInSight)
            {
                timer += Time.deltaTime;
                spotBar.SetCurrentTime(timer);
         

                if (timer >= playerStatus.currentTimeToSpot)
                {
                    HasSight();
                    GameObject[] allGuards = GameObject.FindGameObjectsWithTag("Guard");
                    foreach (GameObject guard in allGuards)
                    {

                        guard.GetComponent<EnemyAI>().Alerted();
                        audio.PlayOneShot(clip1);
                    }               
                    audio.PlayOneShot(clip2);
                    Debug.Log("Spotted by the guard");
                }
            }
            else if (timer > 0)
            {

                timer -= Time.deltaTime;
                spotBar.SetCurrentTime(timer);
            }
            else
                timer = 0;
            //-------------------------------------------------Checking for bodies---------------------------------
            bodyList = GameObject.FindGameObjectsWithTag("Body");
            if (bodyList != null && !onAlert)
            {
                foreach (GameObject body in bodyList)
                {
                    if ((body.transform.position - sightpoint.transform.position).magnitude < coneRange)
                    {
                        Debug.DrawLine(sightpoint.transform.position, body.transform.position , Color.green);
                        ray = new Ray(sightpoint.transform.position, body.transform.position - sightpoint.transform.position);
                        if (Physics.Raycast(ray, out hit, coneRange, layerMask))
                        {
                            Debug.Log("CastRay");
                            Debug.Log(hit.collider.name);
                            if (hit.collider.gameObject.tag == "Body")//If the guard does in fact see the dead body every guard will change state to alarmed
                            {
                                playerStatus.Alert();
                                Debug.Log("Detected Body");
                                GameObject[] allGuards = GameObject.FindGameObjectsWithTag("Guard");
                                foreach(GameObject guard in allGuards)
                                {
                                    
                                    guard.GetComponent<EnemyAI>().Alerted();
                                    audio.PlayOneShot(clip1);
                                }
                                                           
                            }
                        }
                    }
                }
            }

        }


    }

    void HeardNoise(Transform noiseSource)
    {
        heardNoise = true;

        agent.destination = noiseSource.position;

        audio.Play();

    }

    public void Alerted()
    {
        if (!isChasing)
        {
            onAlert = true;
            anim.SetBool("onAlert", true);
            agent.speed = 3f;
            StartCoroutine(waitFor(3.6f));
            
            torch.SetActive(true);
        }
    }
    public void HasSight()
    {

        anim.SetBool("isChasing", true);
        agent.speed = 4f;
        isChasing = true;
        StartCoroutine(SetTarget());
        
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Player" && isChasing == true)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator SetTarget()
    {
        yield return new WaitForSeconds(1f);
        agent.SetDestination(player.transform.position);
        StartCoroutine(SetTarget());
    }

    IEnumerator waitFor(float secondsAmount)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(secondsAmount);
        agent.isStopped = false;
        
    }

    public void Die()
    {
        agent.isStopped = true;

     

        anim.SetBool("Dead", true);
        UIlight.enabled = false;
        spotBarUI.SetActive(false);
        isDead = true;
        gameObject.tag = "Body";
        
     
        
    }
}
