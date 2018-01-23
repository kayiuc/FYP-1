using UnityEngine;
using System.Collections;

public class player_attack : MonoBehaviour
{
    private Animator anim; 

    // Use this for initialization
    void Awake()
    {
        // Set up references.
        anim = GetComponent<Animator>();
    }

    void attack(){
        anim.Play("attack_1");
    }

}
