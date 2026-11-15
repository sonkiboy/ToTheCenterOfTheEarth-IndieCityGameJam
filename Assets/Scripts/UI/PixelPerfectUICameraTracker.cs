using UnityEngine;

public class PixelPerfectUICameraTracker : MonoBehaviour
{

    public Transform TargetCamera;
    public float ZDistance = 100;
    private float incriment = .0625f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetCamera != null)
        {
            this.transform.position = new Vector3(Mathf.RoundToInt(TargetCamera.position.x / incriment) * incriment, Mathf.RoundToInt(TargetCamera.position.y / incriment) * incriment, Mathf.RoundToInt(TargetCamera.position.z / .0625f) * .0625f + ZDistance);
        }
    }
}
