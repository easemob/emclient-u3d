namespace EaseMob{
	public class EMGroupCallback : EMBaseCallback {
		
		public delegate void SuccessCallback(string data);

		public SuccessCallback onSuccessCallback;
	}
}
