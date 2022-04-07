using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
	private GameObject PlayerInformation;
	private GameObject CurrentSpawnLoc;
	private float t = 0;

	public void Start()
	{
		PlayerInformation = GameObject.FindGameObjectWithTag("PlayerInfo");
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			t += Time.deltaTime;
			if (t > 2)
			{
				t = 0;
				DamageTick();
			}
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			DamageTick();
			t = 0;
		}
	}

	private void DamageTick()
	{
		var Player = GameObject.FindGameObjectWithTag("Player");
		PlayerInformation.GetComponent<PlayerAttributes>().PlayerHealth--;
		StartCoroutine(DamageFlash(.2f, .33f));
		Debug.Log("Damage");
	}

	IEnumerator DamageFlash(float time, float intervalTime)
	{
		float elapsedTime = 0f;
		while (elapsedTime < time)
		{
			var Player = GameObject.FindGameObjectWithTag("Player");
			Renderer[] RendererArray = Player.GetComponentsInChildren<Renderer>();

			foreach (Renderer r in RendererArray)
			{
				r.enabled = false;
				elapsedTime += Time.deltaTime;
			}

			yield return new WaitForSeconds(intervalTime);

			foreach (Renderer r in RendererArray)
			{
				r.enabled = true;
				elapsedTime += Time.deltaTime;
			}

			yield return new WaitForSeconds(intervalTime);
		}
	}
	}
