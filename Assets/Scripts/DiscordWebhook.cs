using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DiscordWebhook : MonoBehaviour
{
    string webhook_link = "";

    public void SendMessageToServer(string sMessageForServer)
    {
        
        StartCoroutine(SendWebhook(webhook_link, sMessageForServer, (success) =>
        {
            if (success)
                Debug.Log("Message Sent");
        }));
    }

    IEnumerator SendWebhook(string link, string message, System.Action<bool> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("content", message);
        using (UnityWebRequest www = UnityWebRequest.Post(link, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                action(false);
            }
            else
                action(true);
        }
    }
}