using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{ 
	public RawImage image;
	private VideoPlayer videoPlayer;
    private Canvas canvas;

	// Use this for initialization
	void Start()
	{
		Application.runInBackground = true;
		StartCoroutine(setupVideo());
	}

	IEnumerator setupVideo()
	{
		//Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
		 
		//Disable Play on Awake for both Video and Audio
		videoPlayer.playOnAwake = false;

		// Video clip from Url
		videoPlayer.source = VideoSource.Url;
		videoPlayer.url = "http://mattfeng.scripts.mit.edu/faces.mp4";

		yield return playVideo();
	}

	public IEnumerator playVideo()
	{
		videoPlayer.Prepare ();

		//Wait until video is prepared
		WaitForSeconds waitTime = new WaitForSeconds(1);
		while (!videoPlayer.isPrepared) {
			Debug.Log ("Preparing Video");
			//Prepare/Wait for 5 sceonds only
			yield return waitTime;
			//Break out of the while loop after 5 seconds wait
			break;
		}

		Debug.Log ("Done Preparing Video");

		//Assign the Texture from Video to RawImage to be displayed
		image.texture = videoPlayer.texture;

		//Play Video
		videoPlayer.Play();

		Debug.Log ("Playing Video");
		while (videoPlayer.isPlaying) {
			Debug.LogWarning ("Video Time: " + Mathf.FloorToInt ((float)videoPlayer.time));
			yield return null;
		}
		Debug.Log("Done Playing Video");
	}
}