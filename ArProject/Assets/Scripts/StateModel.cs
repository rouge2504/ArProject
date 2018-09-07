using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateModel : MonoBehaviour {
	[SerializeField]
	private GameObject model;

	private Animator _animator;
	private bool move;

	[SerializeField]
	private float speed;
	// Use this for initialization
	void Start () {
		_animator = model.GetComponent<Animator>();
		move = false;
	}
	
	// Update is called once per frame
	void Update () {
		Dissolve();
	}

	public void Die(){
		_animator.SetBool("die", true);
	}

	public void InitDissolve(){
		move = true;
	}

	private void Dissolve(){
		if (move)
        {
            model.transform.Translate(0, -Time.deltaTime * speed, 0);
        }

	}
}
