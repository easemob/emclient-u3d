using UnityEngine;
using System.Collections;
using EaseMob;

public class GlobalListener : MonoBehaviour {

	public GameObject obj;

	public string listenerInfo = "";

	public static GlobalListener Instance = null;

	// Use this for initialization
	void Start () {
	

		UnityEngine.Object.DontDestroyOnLoad(obj);

		setConnectionListener ();

		setGroupListener ();

		Instance = this;
	}


	private void setConnectionListener()
	{
		EMConnListenerCallback connCb = new EMConnListenerCallback ();
		connCb.onConnectionCallback = () => {
			addInfo("connected");
		};
		connCb.onDisconnectedCallback = (code) => {
			addInfo("disconnect with code="+code);
		};
		EMClient.Instance.connListenerCallback = connCb;
	}

	private void setGroupListener()
	{
		EMGroupListenerCallback groupListenerCallback = new EMGroupListenerCallback ();
		groupListenerCallback.onUserRemovedCallback = (groupId,groupName) => {
			addInfo("received onUserRemovedCallback:[groupId="+groupId+",groupName="+groupName);
		};
		groupListenerCallback.onInvitationReceived = (groupId, groupName, inviter, reason) => {
			addInfo("received onInvitationReceived:[groupId="+groupId+",inviter="+inviter+",reason="+reason);
		};
		groupListenerCallback.onInvitationDeclined = (groupId, invitee, reason) => {
			addInfo("received onInvitationDeclined:[groupId="+groupId+",invitee="+invitee+",reason="+reason);
		};
		groupListenerCallback.onInvitationAccepted = (groupId, inviter, reason) => {
			addInfo("received onInvitationAccepted:[groupId="+groupId+",inviter="+inviter+",reason="+reason);
		};
		groupListenerCallback.onGroupDestroyed = (groupId, groupName) => {
			addInfo("received onGroupDestroyed:[groupId="+groupId+",groupName="+groupName);
		};
		groupListenerCallback.onAutoAcceptInvitationFromGroup = (groupId, inviter, inviteMessage) => {
			addInfo("received onAutoAcceptInvitationFromGroup:[groupId="+groupId+",inviter="+inviter+",inviteMessage="+inviteMessage);
		};
		groupListenerCallback.onApplicationReceived = (groupId, groupName, applicant, reason) => {
			addInfo("received onApplicationReceived:[groupId="+groupId+",groupName="+groupName+",applicant="+applicant+",reason="+reason);
		};
		groupListenerCallback.onApplicationDeclined = (groupId, groupName, decliner, reason) => {
			addInfo("received onApplicationDeclined:[groupId="+groupId+",groupName="+groupName+",decliner="+decliner+",reason="+reason);
		};
		groupListenerCallback.onApplicationAccept = (groupId, groupName, accepter) => {
			addInfo("received onApplicationAccept:[groupId="+groupId+",groupName="+groupName+",accepter="+accepter);
		};
		EMClient.Instance.groupListenerCallback = groupListenerCallback;

	}

	private void addInfo(string info)
	{
		listenerInfo += info + "\n";
	}
}
