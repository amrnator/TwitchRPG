using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using TwitchChatter;

public class InitialVoting : MonoBehaviour {

	public int countDownTime;

	public string channelName;

	public Text Question;

	public Text Entry1;

	public Text Entry2;

	public Text Entry3;

	public Text Entry4;

	public Text Entry5;

	public Text TimeDisplay;

	private List<VoteEntry> voteList;

	private System.Timers.Timer timekeeper;

	private bool countComplete;

	private bool questionActive = false;

	private int questionCount = 0;

	private bool refreshDelay = true;

	private VoteEntry newEntry;

	private CountComparer countComparer;

	void Start(){
		//start the listener
		if (TwitchChatClient.singleton != null)
		{
			TwitchChatClient.singleton.AddChatListener(OnChatMessage);
		}

		if (!string.IsNullOrEmpty(channelName))
		{
			TwitchChatClient.singleton.JoinChannel(channelName);
		}
		else
		{
			Debug.LogWarning("No channel name entered for poll! Enter a channel name and restart the scene.", this);
		}
			
		StartCoroutine ("Countdown", countDownTime);
	}

	void Awake(){
		voteList = new List<VoteEntry> ();

		countComplete = false;

		countComparer = new CountComparer ();
	}

	private void OnDestroy()
	{
		if (TwitchChatClient.singleton != null)
		{
			TwitchChatClient.singleton.RemoveChatListener(OnChatMessage);
		}
	}

	private void OnChatMessage(ref TwitchChatMessage msg){
		if(questionActive){

			string entry = msg.chatMessageMinusEmotes.ToUpper();

			if (entry.Length <= 1) {
				return;
			}

			newEntry = new VoteEntry (entry, 0);

			//check if votelist has this entry already
			if (voteList.Contains (newEntry)) {
				print ("incrementted");

				//this entry is already in the list, increment the appropriate entry
				int index;

				for(index = 0 ; index < voteList.Count; index ++){
					
					if(voteList[index].name.Equals(newEntry.name)){
						voteList [index].count++;
						break;
					}
				}

			} else {
				//add this entry to the list
				voteList.Add(newEntry);
			}
		}
	}

	void Update(){

		//pose a new question when count down completes
		if(countComplete){
			//reset var
			countComplete = false;
			//stop taking answers
			questionActive = false;

			//run a new question
			voteList.Clear ();

			if (questionCount == 0) {
				//ask for player's name
				Question.text = "What is our hero's name?";
				questionActive = true;
				StartCoroutine ("Countdown", 40);
				questionCount++; 
			}
		}

		//refresh list 
		if (refreshDelay && questionActive) {
			
			StartCoroutine ("RefreshList");
		}

		//print (voteList.Count);
	}

	private IEnumerator Countdown(int time){
		print ("Countdown started");
		while(time > 0){
			TimeDisplay.text = time.ToString ();
			yield return new WaitForSeconds(1);
			time--;
		}
		countComplete = true;
		Debug.Log("Countdown Complete!");
	}

	//refresh list of UI elements
	private IEnumerator RefreshList(){
		refreshDelay = false;
		print ("refreshing list");
		print (voteList.Count);

		if ( voteList.Count > 5) {
			//sort the list
			voteList.Sort (countComparer);

			//print highest values to the screen
			VoteEntry first = voteList [voteList.Count - 1];
			VoteEntry second = voteList [voteList.Count - 2];
			VoteEntry third = voteList [voteList.Count - 3];
			VoteEntry fourth = voteList [voteList.Count - 4];
			VoteEntry fifth = voteList [voteList.Count - 5];

			Entry1.text = first.count + "     " + first.name;
			Entry2.text = second.count + "     " + second.name;
			Entry3.text = third.count + "     " + third.name;
			Entry4.text = fourth.count + "     " + fourth.name;
			Entry5.text = fifth.count + "     " + fifth.name;
		}
		yield return new WaitForSeconds(2);
		refreshDelay = true;
	}
}



//class for holding unique vote entries and their number of votes
public class VoteEntry : System.IComparable<VoteEntry>, System.IEquatable<VoteEntry>
{
	public string name;

	public int count;

	public VoteEntry(string _name, int _count){
		name = _name;
		count = _count;
	}

	//equals is designed to only check if names are equivalent, this is for contains
	public bool Equals(VoteEntry b){
		
		if (b == null) {
			return false;
		}

		if (this.name.Equals (b.name)) {
			return true;
		} else {
			return false;
		}
	}


	//default comparator for use in search
	public int CompareTo (VoteEntry y)
	{
		if(this.name.Equals (y.name)){
			return 0;
		}

		if(this.count < y.count){
			return -1;
		}

		return 1;
	}
}

//count comaprer for use in sorting
public class CountComparer: IComparer<VoteEntry>
{
	public int Compare(VoteEntry x, VoteEntry y){
		
		if(x.count == y.count){
			return 0;
		}

		if(x.count < y.count){
			return -1;
		}

		return 1;
	}
}

