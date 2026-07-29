using UnityEngine;

public class FollowingEye : MonoBehaviour
{

    [SerializeField]Vector2 origin;
    public GameObject TrackingTarget;
    public float MovementRange = 1f;
    public bool IsLocalScale = true;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsLocalScale)
        {
            origin = transform.localPosition;
        }
        else
        {
            origin = transform.position;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TrackingTarget != null)
        {
            if (IsLocalScale)
            {
                Vector2 dir = ((Vector2)TrackingTarget.transform.position - (Vector2)this.transform.TransformPoint(origin)).normalized;

                this.transform.localPosition = origin + (dir * MovementRange);
            }
            else
            {
                Vector2 dir = ((Vector2)TrackingTarget.transform.position - origin).normalized;

                this.transform.position = origin + (dir * MovementRange);
            }
            
        }
    }
}

