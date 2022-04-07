using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private string currentState = "Idle";
	public float chaseRange = 5;
	public float speed = 3;
	private Transform Player;
	private Transform target;
	UnityEngine.AI.NavMeshAgent nav;

	// Start is called before the first frame update
	void Start()
    {
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
    {
		float distance = Vector3.Distance(transform.position, Player.position);
		Vector3 targetPos = new Vector3(Player.position.x, this.transform.position.y, Player.position.z);
		if (distance < chaseRange)
		{
			nav.enabled = true;
			nav.SetDestination(Player.position);
			transform.LookAt(targetPos);
		}
		else		
		{
			nav.enabled = false;
		}
		/*
		float distance = Vector3.Distance(transform.position, target.position);
		//Debug.Log("Distance: " + Vector3.Distance(transform.position, target.position));
		if (currentState == "Idle")
		{
			if(distance<chaseRange)
			{
				
				currentState = "Chase";
			}
		}
		else if (currentState == "Chase")
		{
			Debug.Log("Chase");
			//Animation; Run
			if (target.position.x > transform.position.x)
			{
				Debug.Log("RIGHT");
				transform.Translate(transform.right * speed * Time.deltaTime);
			}
			else
			{
				Debug.Log("LEFT");
				transform.Translate(-transform.right * speed * Time.deltaTime);
			}
		}
		*/
	} 
}
