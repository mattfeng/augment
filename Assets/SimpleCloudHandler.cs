using Vuforia;
using UnityEngine;
using UnityEngine.Video;

public class SimpleCloudHandler: MonoBehaviour, ICloudRecoEventHandler {
    public ImageTargetBehaviour ImageTargetTemplate;
    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";

    void Start() {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour) {
            Debug.Log("Working?");
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
            tracker.TargetFinder.ClearTrackables(true);
        }
    }

    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) {
        // do something with the target metadata
        mTargetMetadata = targetSearchResult.MetaData;
        Debug.Log(targetSearchResult.TargetName);
        Debug.Log(mTargetMetadata);

        // GameObject rawImg = GameObject.Find("ARVideoPlayer");
        // StartCoroutine(rawImg.GetComponent<StreamVideo>().playVideo());


        if (ImageTargetTemplate) {
            GameObject newImageTarget = Instantiate(ImageTargetTemplate.gameObject) as GameObject;
            // Change the video URL based on the recognized object
            newImageTarget.GetComponentInChildren<VideoPlayer>().url = mTargetMetadata;
            newImageTarget.GetComponentInChildren<VideoPlayer>().Play();
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            ImageTargetBehaviour imageTargetBehaviour = 
                (ImageTargetBehaviour) tracker.TargetFinder.EnableTracking(targetSearchResult, newImageTarget);
        }

        // stop scanning the cloud
        // mCloudRecoBehaviour.CloudRecoEnabled = false;
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
                if (mCloudRecoBehaviour.CloudRecoInitialized && !mCloudRecoBehaviour.CloudRecoEnabled) {
                    mCloudRecoBehaviour.CloudRecoEnabled = true;
                }
            }
        }
    }
}
