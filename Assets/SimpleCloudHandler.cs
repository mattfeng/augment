using Vuforia;
using UnityEngine;

public class SimpleCloudHandler: MonoBehaviour, ICloudRecoEventHandler {
	private CloudRecoBehaviour mCloudRecoBehaviour;
	private bool mIsScanning = false;
	private string mTargetMetadata = "";

	void Start() {
		mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

		if (mCloudRecoBehaviour) {
			Debug.Log ("Working?");
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}
	}

	public void OnInitialized() {
		Debug.Log("Cloud Reco Initialized");
	}

	public void OnInitError(TargetFinder.InitState initError) {
		Debug.Log("Cloud reco init error " + initError.ToString());
	}

	public void OnUpdateError(TargetFinder.UpdateState updateError) {
		Debug.Log("Cloud Reco update error " + updateError.ToString());
	}

	public void OnStateChanged(bool scanning) {
		mIsScanning = scanning;

		if (scanning) {
			// clear all known trackables
			var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			tracker.TargetFinder.ClearTrackables(false);
		}
	}

	public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) {
		// do something with the target metadata
		mTargetMetadata = targetSearchResult.MetaData;
		Debug.Log(targetSearchResult.TargetName);
		Debug.Log(mTargetMetadata);

		GameObject rawImg = GameObject.Find("ARVideoPlayer");
		StartCoroutine(rawImg.GetComponent<StreamVideo>().playVideo());

		// stop scanning the cloud
		mCloudRecoBehaviour.CloudRecoEnabled = false;
	}

	void OnGUI() {
		// Display current 'scanning' status
		GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
		// Display metadata of latest detected cloud-target
		GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);

		// if not scanning, show button
		// so that user can restart cloud scanning
		if (!mIsScanning) {
			if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning")) {
				// restart TargetFinder
				mCloudRecoBehaviour.CloudRecoEnabled = true;
			}
		}
	}
}
