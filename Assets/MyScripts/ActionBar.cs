using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour
{
    public GameObject sword;
    public GameObject shield;
    //public GameObject shield;

    void start()
    {
        sword = GameObject.FindGameObjectWithTag("Sword");
        shield = GameObject.FindGameObjectWithTag("Shield");
    }
    public void action1()
    {
        if (sword.activeSelf)
        {
            sword.SetActive(false);
        }
        else
            sword.SetActive(true);

    }
    public void action2()
    {
        if (shield.activeSelf)
        {
            shield.SetActive(false);
        }
        else
            shield.SetActive(true);

    }
}
