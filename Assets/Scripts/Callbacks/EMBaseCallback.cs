public class EMBaseCallback {

	public delegate void SuccessCallback();
	public delegate void ProgressCallback(int progress,string status);
	public delegate void ErrorCallback(int code,string message);

	public SuccessCallback onSuccessCallback;
	public ProgressCallback onProgressCallback;
	public ErrorCallback onErrorCallback;

	public int CallbackId{ get; set;}

}
