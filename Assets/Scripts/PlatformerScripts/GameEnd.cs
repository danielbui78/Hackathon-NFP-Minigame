using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
	private GameObject PlayerInformation;
	public GameObject Lamp;
	public GameObject EndScreen;

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

			EndScreen.SetActive(true);
			Time.timeScale = 0;
		}
	}
}
