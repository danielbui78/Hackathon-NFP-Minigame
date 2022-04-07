using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private GameObject PlayerInformation;
	private Color Active;
	public GameObject Lamp;
	public GameObject OldLight;
	public GameObject NewLight;

	private void Start()
	{
		PlayerInformation = GameObject.FindGameObjectWithTag("PlayerInfo");
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Lamp.GetComponent<MeshRenderer>().material.color = Color.green;
			PlayerInformation.GetComponent<PlayerAttributes>().CheckpointPos = this.gameObject.transform.position;
			this.gameObject.SetActive(false);
			OldLight.SetActive(false);
			NewLight.SetActive(true);
		}
	}
}
