using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
	public Material DamageFlash;
	public GameObject Player;
	public int PlayerHealth = 3;
	public int CurrentCheckpoint = 0;
	public Vector3 CheckpointPos;
	public GameObject Checkpoint0;
	public GameObject Checkpoint1;
	private Color ogColor;
    // Start is called before the first frame update
    void Start()
    {
		CheckpointPos = Checkpoint0.transform.position;
		Player = GameObject.FindGameObjectWithTag("Player");


	}

    // Update is called once per frame
    void Update()
    {
		if(Player.transform.position.y < -5)
		{
			PlayerHealth--;
			Player.transform.position = CheckpointPos;
		}
		if (PlayerHealth == 0)
		{
			Player.GetComponent<CharacterController>().enabled = false;
			Player.transform.position = CheckpointPos;
			Player.GetComponent<CharacterController>().enabled = true;
			Debug.Log("Dead");
			PlayerHealth = 3;
		}
    }
}
