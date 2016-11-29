using System.Collections.Generic;

namespace EaseMob{
	public class EMGroupCallback : EMBaseCallback {

		public delegate void SuccessGetBlockedUsers(List<string> users);
		
		public delegate void SuccessCreateGroupCallback(EMGroup group);

		public delegate void SuccessGetGroupListCallback(List<EMGroup> groups);

		public delegate void SuccessJoinGroupCallback(EMGroup group);

		public SuccessGetBlockedUsers onSuccessGetBlockedUsers;
		public SuccessCreateGroupCallback onSuccessCreateGroupCallback;
		public SuccessGetGroupListCallback onSuccessGetGroupListCallback;
		public SuccessJoinGroupCallback onSuccessJoinGroupCallback;
	}
}
