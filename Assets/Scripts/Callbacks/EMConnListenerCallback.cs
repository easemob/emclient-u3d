public class EMConnListenerCallback {

	public delegate void ConnectionCallback();
	public delegate void DisconnectedCallback(string code);

	public ConnectionCallback onConnectionCallback;
	public DisconnectedCallback onDisconnectedCallback;
}
