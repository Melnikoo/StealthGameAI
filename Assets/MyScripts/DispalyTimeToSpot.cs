using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DispalyTimeToSpot : MonoBehaviour
{
    PlayerStatusControl playerStatus;
    private TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatusControl>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        textMesh.text = playerStatus.currentTimeToSpot.ToString("F2");
    }
}
