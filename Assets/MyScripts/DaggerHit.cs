using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DaggerHit : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audio;
    Animator anim;
    AnimatorStateInfo info;
    private int _playerAttackStateHash;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponentInParent<Animator>();
        _playerAttackStateHash = Animator.StringToHash("Attacks");
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Guard" && info.tagHash == _playerAttackStateHash)
        {
            Debug.Log("Collided");
            audio.Play();
            other.GetComponent<EnemyAI>().Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
    }
}
