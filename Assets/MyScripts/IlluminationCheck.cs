using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminationCheck : MonoBehaviour
{
    Light pointlight;
    GameObject player;
    PlayerStatusControl playerStatus;
    private bool inRange = false;
    private float playerIllumination;
    float temp = 0;

    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        pointlight = GetComponent<Light>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStatus = player.GetComponent<PlayerStatusControl>();
    }


    private void OnTriggerEnter(Collider other) //Player in range
    {
        if(other.tag == "Player")
        {
            inRange = true;
        }
       
    }

    private void OnTriggerExit(Collider other) // if player leaves range
    {
        if (other.tag == "Player")
        {
            
            
            inRange = false;
            playerStatus.illuminationChange(-temp);
            temp = 0;
        }

    }

    void Update()
    {
        if(inRange) /* If Player is in range of the light source we check if he is not behind any obstacle then continiously update his illumination level depending on the distance and the intensity
            of the light source*/
        {
            ray = new Ray(gameObject.transform.position, player.transform.position - gameObject.transform.position);

            if(Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.red);
                if(hit.collider.gameObject.tag == "Player")
                {
                    playerIllumination = (pointlight.intensity * 2) / hit.distance;
                    if (playerIllumination > playerStatus.maxIllumination)
                        playerIllumination = playerStatus.maxIllumination;
                   
                   
                    playerStatus.illuminationChange(playerIllumination - temp);
                    temp = playerIllumination;
                    
                }
                else
                {
                    playerStatus.illuminationChange(-temp);
                    temp = 0;
                }
            }
           
        }
    }
}
