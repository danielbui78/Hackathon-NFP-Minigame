using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEndTrigger : MonoBehaviour
{
	public GameObject Platform;
	public GameObject Exit;
	private GameObject Player;
	// Start is called before the first frame update
	void Start()
	{
		//platformAnim = Platform.GetComponent<Animator>();
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			this.gameObject.GetComponent<BoxCollider>().enabled = false;
			Exit.SetActive(false);
			Player.GetComponent<CharacterController>().enabled = true;
			Player.transform.parent = null;
		}
	}


}
