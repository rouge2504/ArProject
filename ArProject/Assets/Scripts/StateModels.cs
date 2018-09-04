using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateModels : MonoBehaviour {
	[SerializeField]
	private GameObject model;
	private Animator animator;

	[SerializeField]
	private AnimationClip walkingClip;
	[SerializeField]
    private AnimationClip dieClip;
	// Use this for initialization
	void Start () { 
		animator = model.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)){
			DieState();
		}
	}

	public void DieState(){
		animator.SetBool("shoot", true);
	}
}
