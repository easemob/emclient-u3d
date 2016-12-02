namespace EaseMob{
	
	public class EMGroup {

		public string mGroupId{ set; get; }
		public string mGroupName{ set; get; }
		public string mDescription{ set; get; }
		public string mOwner{ set; get; }
		public string mMembers{ set; get; }
		public bool mIsPublic{ set; get; }
		public bool mIsMsgBlocked{ set; get; }
		public bool mIsAllowInvites{ set; get; }
		public bool mIsNeedApproval{ set; get; }
	}
}
