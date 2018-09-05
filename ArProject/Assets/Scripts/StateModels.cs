using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateModels : MonoBehaviour {

	public GameObject model;
	private Animator animator;

	public GameObject[] _materials;

	private Renderer[] renderers;

	private bool startDissolve;

	private float speed;

	private float dissolve;
    
	// Use this for initialization
	void Start () {
		dissolve = 0;
		speed = 0;
		animator = model.GetComponent<Animator>();
		startDissolve = false;

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
		InitDissolve();
	}

	public void DieState(){
		animator.SetBool("shoot", true);
	}

	public void Dissolve(){
		startDissolve = true;
	}

	private void InitDissolve(){
		if (startDissolve){
			float dissolve = Shader.GetGlobalFloat("dissolve");
			dissolve += Time.deltaTime;
			for (int i = 0; i < _materials.Length; i++)
            {
				//dissolve = renderers[i].material.
            }
		}
	}
}
