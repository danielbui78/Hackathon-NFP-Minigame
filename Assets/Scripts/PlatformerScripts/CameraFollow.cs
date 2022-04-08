using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	GameObject player;
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (this.gameObject.tag == "MainCamera")
		{
			transform.position = new Vector3(player.transform.position.x + 1f, player.transform.position.y + 2f, transform.position.z);
		}
		else
		{
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2.3f, transform.position.z);
		}
	}
}
