using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
	public GameObject Platform;
	public GameObject Entrance;
	public GameObject Exit;
	public Animator platformAnim;
	private GameObject Player;
	public GameObject PlayerPos;
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
			StartCoroutine(AnimPlay());
		}
	}
	IEnumerator AnimPlay()
	{
		this.gameObject.GetComponent<BoxCollider>().enabled = false;
		platformAnim.Play("PlatformMovement");
		Entrance.SetActive(true);
		Player.GetComponent<CharacterController>().enabled = false;
		Player.transform.parent = PlayerPos.transform;

		yield return new WaitForSeconds(4);

		Exit.SetActive(false);
		Player.GetComponent<CharacterController>().enabled = true;
		Player.transform.parent = null;
	}
}
