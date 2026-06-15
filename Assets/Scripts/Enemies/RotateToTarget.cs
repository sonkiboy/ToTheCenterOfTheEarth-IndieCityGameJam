using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public Transform Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward,(Target.position-this.transform.position).normalized);
    }
}
