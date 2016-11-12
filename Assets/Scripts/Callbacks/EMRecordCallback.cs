namespace EaseMob{

	public class EMRecordCallback  {

		public delegate void StopRecordCallback(string path,int length);

		public StopRecordCallback onStopRecordCallback;

	}

}