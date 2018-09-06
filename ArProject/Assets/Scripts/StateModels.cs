using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StateModels : MonoBehaviour {

	public GameObject model;
	private Animator animator;

	public GameObject[] _materials;

	private Renderer[] renderers;

	private bool move;

	public float speed;
    
	public Text debug;
    
	// Use this for initialization
	void Start () {
		move = false;
		animator = model.GetComponent<Animator>();

		renderers = new Renderer[_materials.Length];
		for (int i = 0; i < _materials.Length; i++){
			renderers[i] = _materials[i].GetComponent<Renderer>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.A)){
			DieState();
		}*/
		debug.text = "YPosition: " + model.transform.position.y;
		if (move){
			model.transform.Translate(0, -Time.deltaTime * speed, 0);
		}
	}

	public void DieState(){
		animator.SetBool("shoot", true);
	}

	public void Dissolve(){
		move = true;
	}


}
