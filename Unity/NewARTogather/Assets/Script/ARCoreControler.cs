using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class ARCoreControler : MonoBehaviour {
    /// <summary>
    /// A prefab for visualizing an AugmentedImage.
    /// </summary>
    public GameObject AugmentedImageVisualizerPrefab;

    /// <summary>
    /// The overlay containing the fit to scan user guide.
    /// </summary>
    public GameObject FitToScanOverlay;
    public Anchor imagess;

    private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

    /// <summary>
    /// The Unity Update method.
    /// </summary>
    public void Update()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Check that motion tracking is tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

        // Create visualizers and anchors for updated augmented images that are tracking and do not previously
        // have a visualizer. Remove visualizers for stopped images.
        foreach (var image in m_TempAugmentedImages)
        {
            if (image.TrackingState == TrackingState.Tracking && AugmentedImageVisualizerPrefab.activeSelf == false)
            {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                AugmentedImageVisualizerPrefab.SetActive(true);
                AugmentedImageVisualizerPrefab.transform.parent = anchor.transform;
                AugmentedImageVisualizerPrefab.transform.localPosition = new Vector3(0, 0, 0);
                AugmentedImageVisualizerPrefab.transform.localRotation = new Quaternion(0, 0, 0, 0);
                GameObject.Find("basePut").transform.parent = anchor.transform;
                GameObject.Find("basePut").transform.localPosition = new Vector3(0, 0, 0);
                GameObject.Find("basePut").transform.localRotation = new Quaternion(0, 0, 0, 0);

                float half = image.ExtentX / 2 > image.ExtentZ / 2 ? image.ExtentX / 2 : image.ExtentZ / 2;
                anchor.transform.localScale = new Vector3(half, half, half);
                GameObject.Find("basePut").transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else if (image.TrackingState == TrackingState.Stopped && AugmentedImageVisualizerPrefab.activeSelf == true)
            {
                AugmentedImageVisualizerPrefab.SetActive(false);
            }
        }

    }
}
