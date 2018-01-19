using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

    public float speed = 5;
    public float gravity = 100;
    public float rotateSpeed = 150;

    private Rigidbody rigid;

	void Start () {
		rigid = GetComponent<Rigidbody>();
	}
	

	void FixedUpdate () {

	}
}
