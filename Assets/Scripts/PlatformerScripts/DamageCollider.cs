using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageCollider : MonoBehaviour
{
	private GameObject PlayerInformation;
	private GameObject CurrentSpawnLoc;
	public Image healthCircle;
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
				StartCoroutine(Tangibility());
				DamageTick();
			}
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (PlayerInformation.GetComponent<PlayerAttributes>().Tangible == true)
			{
				StartCoroutine(Tangibility());
				DamageTick();
				t = 0;
			}
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
		healthCircle.enabled = true;
		if (PlayerInformation.GetComponent<PlayerAttributes>().PlayerHealth == 2)
		{
			healthCircle.fillAmount = .66f;
			healthCircle.color = Color.yellow;
		}

		if (PlayerInformation.GetComponent<PlayerAttributes>().PlayerHealth == 1)
		{
			healthCircle.fillAmount = .33f;
			healthCircle.color = Color.red;
		}
		if (PlayerInformation.GetComponent<PlayerAttributes>().PlayerHealth == 0)
		{
			healthCircle.fillAmount = 1;
			healthCircle.color = Color.green;
		}

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
			healthCircle.enabled = false;
		}
	}
	public IEnumerator Tangibility()
	{
		PlayerInformation.GetComponent<PlayerAttributes>().Tangible = false;
		yield return new WaitForSeconds(1.5f);
		PlayerInformation.GetComponent<PlayerAttributes>().Tangible = true;
	}
	}
