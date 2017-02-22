using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchChatter;
using UnityEngine.UI;

public class TwitchManager : MonoBehaviour {
	// Name of the Twitch channel to join for the raffle
	public string _raffleChannelName;

	// List of users entered into the raffle
	private List<TwitchChatMessage> _chatMessage;

	private void Awake()
	{
		_chatMessage = new List<TwitchChatMessage>();
	}

	private void Start()
	{
		if (TwitchChatClient.singleton != null)
		{
			TwitchChatClient.singleton.AddChatListener(OnChatMessage);
		}

		if (!string.IsNullOrEmpty(_raffleChannelName))
		{
			TwitchChatClient.singleton.JoinChannel(_raffleChannelName);
		}
		else
		{
			Debug.LogWarning("No channel name entered! Enter a channel name and restart the scene.", this);
		}
	}

	private void OnDestroy()
	{
		if (TwitchChatClient.singleton != null)
		{
			TwitchChatClient.singleton.RemoveChatListener(OnChatMessage);
		}
	}

	private void OnChatMessage(ref TwitchChatMessage msg)
	{
		_chatMessage.Add (msg);

		if(_chatMessage.Count > 50){
			_chatMessage.RemoveRange (50, _chatMessage.Count - 50);
		}

	}

	public TwitchChatMessage GetRandomMessage(){
		return _chatMessage [Random.Range (0, _chatMessage.Count)];
	}
}
