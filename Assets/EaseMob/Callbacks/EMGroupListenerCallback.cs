namespace EaseMob{
	public class EMGroupListenerCallback {

		public delegate void UserRemovedCallback(string groupId,string groupName);
		public delegate void InvitationReceived(string groupId,string groupName,string inviter,string reason);
		public delegate void InvitationDeclined(string groupId,string invitee,string reason);
		public delegate void InvitationAccepted(string groupId,string inviter,string reason);
		public delegate void GroupDestroyed(string groupId,string groupName);
		public delegate void AutoAcceptInvitationFromGroup(string groupId,string inviter,string inviteMessage);
		public delegate void ApplicationReceived(string groupId, string groupName, string applicant, string reason);
		public delegate void ApplicationDeclined(string groupId, string groupName, string decliner, string reason);
		public delegate void ApplicationAccept(string groupId, string groupName, string accepter);

		public UserRemovedCallback onUserRemovedCallback;
		public InvitationReceived onInvitationReceived;
		public InvitationDeclined onInvitationDeclined;
		public InvitationAccepted onInvitationAccepted;
		public GroupDestroyed onGroupDestroyed;
		public AutoAcceptInvitationFromGroup onAutoAcceptInvitationFromGroup;
		public ApplicationReceived onApplicationReceived;
		public ApplicationDeclined onApplicationDeclined;
		public ApplicationAccept onApplicationAccept;
	}
}