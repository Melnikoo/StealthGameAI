using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStatusControl : MonoBehaviour
{
    AudioSource audio;
    public AudioClip clip1;
    [Range(0, 5)]
    public float interactDistance;
    public Camera cam;
    public SliderBar illuminationBar;
    public SliderBar noiseBar;
    public float noiseLevel = 0;
    [Range(0, 10)]
    public float maxIllumination;
    [Range(0, 100)]
    public float maxNoise;
    [Range(0, 10)]
    public float noiseDecreaseFactor;
    [Range(0, 20)]
    public float noiseIncreaseFactor;
    public KeyCode attackKey = KeyCode.Mouse0;
    Animator anim;
    bool onAlert = false;
    public GameObject Victory;

    FirstPersonController controller;

    public float currentIllumination;
    public float defaultTimeToSpot = 4f;
    public float currentTimeToSpot;


    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
        audio = GetComponent<AudioSource>();
        currentTimeToSpot = defaultTimeToSpot;
        illuminationBar.SetMaxValue(maxIllumination);
        noiseBar.SetMaxValue(maxNoise);
        controller = GetComponent<FirstPersonController>();
        anim = GetComponent<Animator>();
        
    }

    public void Alert()
    {
        onAlert = true;
    }

    public void illuminationChange(float illumination)
    {
        currentIllumination += illumination;
        if (currentIllumination > maxIllumination)
            currentIllumination = maxIllumination;
        if (currentIllumination < 0)
            currentIllumination = 0;
        illuminationBar.SetValue(currentIllumination);
    }

    public void RaiseNoiseLevel(float noiseAmount)
    {
        noiseLevel += noiseAmount;
        noiseBar.SetValue(noiseLevel);
    }
   /* void InteractWithObjects()
    {
        RaycastHit hit; //Create a raycast called 'hit'

        // Fires a raycast from the camera to the camera's forward position and outputs the data of what it hit in the 'hit' variable
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit)) //If it hit something:
        {
            if(hit.collider.gameObject.tag == "Guard" && hit.distance <= interactDistance)
            {
                Debug.Log("Success");
            }
        }
    }*/

    // Update is called once per frame
    void Update()
    {


        if (controller.isSprinting)
            noiseLevel += Time.deltaTime * noiseIncreaseFactor * 2;
        else if(controller.isCrouched)
        {
            
        }
        else if (controller.isWalking)
            noiseLevel += Time.deltaTime * noiseIncreaseFactor;

        if (noiseLevel > 0)
        {
            noiseLevel -= Time.deltaTime * noiseDecreaseFactor;
            noiseBar.SetValue(noiseLevel);
        }
        else if (noiseLevel < 0)
        {
            noiseLevel = 0;
            noiseBar.SetValue(noiseLevel);
        }
        
        if (noiseLevel > 100)
        {
            noiseLevel = 100;
            noiseBar.SetValue(noiseLevel);
        }
           
           

        if (Input.GetKeyDown(KeyCode.E))
        {
            audio.PlayOneShot(clip1);
            RaiseNoiseLevel(30f);
        }
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit)) //If it hit something:
        {
            if (hit.collider.gameObject.tag == "Guard" && hit.distance <= interactDistance)
            {
                Transform hitTransform = hit.collider.gameObject.transform;
                Vector3 toEnemy = (hitTransform.position - transform.position).normalized;

                controller.crosshairObject.color = Color.red;
                if (Input.GetKeyDown(attackKey) && Vector3.Dot(toEnemy, hitTransform.forward) > 0) //Check if the player is behind the enemy and the key is pressed to attack
                {                   
                    anim.SetTrigger("Attacks");
                    RaiseNoiseLevel(40f);
                }
            }
            else if (hit.collider.gameObject.tag == "Chest" && hit.distance <= interactDistance)
            {
                
                controller.crosshairObject.color = Color.green;
                if (Input.GetKeyDown(attackKey))
                {
                    Victory.SetActive(true);
                    GetComponent<FirstPersonController>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Time.timeScale = 0;
                }

            }
            else
                controller.crosshairObject.color = controller.crosshairColor;
        }
        else
            controller.crosshairObject.color = controller.crosshairColor;

        if (currentIllumination > 1)
        {
            currentTimeToSpot = (defaultTimeToSpot / currentIllumination) - noiseLevel/100;
        }
        else
            currentTimeToSpot = defaultTimeToSpot - noiseLevel/100;

        if (onAlert)
            currentTimeToSpot *= 0.8f;

        
    }

    void playSound()
    {
        audio.Play();
    }
}
