public class EMMessage {
	public string mMsgId { set; get; }
	public string mFrom { set; get; }
	public string mTo  { set; get; }
	public bool mIsUnread { set; get; }
	public bool mIsListened { set; get; }
	public bool mIsAcked { set; get; }
	public bool mIsDelivered { set; get; }
	public long mLocalTime { set; get; }
	public long mServerTime { set; get; }
	public int mChatType { set; get; }
	public int mType { set; get; }
	public int mStatus { set; get; }
	public int mDirection { set; get; }
	public string mDisplayName { set; get; }
	public string mSecretKey { set; get; }
	public string mLocalPath { set; get; }
	public string mRemotePath { set; get; }
	public string mTxt { set; get; }
	public string mThumbnailLocalPath { set; get; }
	public string mThumbnailRemotePath { set; get; }
	public string mThumbnailSecretKey { set; get; }
	public int mWidth { set; get; }
	public int mHeight { set; get; }
	public int mDuration { set; get; }
}
