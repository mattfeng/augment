# Other notes during development

# ``ImageTargetTemplate``

The idea behind the template is a GameObject that can be programmatically instantiated during runtime so that multiple copies of the same generic ImageTarget can be made. This is particularly good for CloudRecognition, where it doesn't make sense to make a GameObject by hand for every CloudRecognition target.

Thus, by making a ``CloudRecoTarget`` that is generic, we can instantiate versions of it for each different cloud recognition target we upload.

See the following code (original link: https://developer.vuforia.com/forum/cloud-recognition/object-type-imagetargetbehaviour-has-been-destroyed-you-are-still-trying-access-it):

```csharp
public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
{
    // duplicate the referenced image target
    GameObject newImageTarget = Instantiate(ImageTargetTemplate.gameObject) as GameObject;
    GameObject augmentation = null;
        
    string model_name = targetSearchResult.MetaData;
        
    if(augmentation != null)
        augmentation.transform.parent = newImageTarget.transform;

    // enable the new result with the same ImageTargetBehaviour:
    ImageTargetBehaviour imageTargetBehaviour = mImageTracker.TargetFinder.EnableTracking(targetSearchResult, newImageTarget);
        
    Debug.Log("Metadata value is " + model_name);
        
    switch(model_name) {
        case "teapot":
            Destroy( imageTargetBehaviour.gameObject.transform.Find("Dragon").gameObject );
            break;
            
        case "dragon":
            Destroy( imageTargetBehaviour.gameObject.transform.Find("teapot").gameObject );
            break;
    }

    if (imageTargetBehaviour != null) {
        // stop the target finder
        mCloudRecoBehaviour.CloudRecoEnabled = false;
    }
}
```