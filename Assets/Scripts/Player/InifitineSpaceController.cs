using UnityEngine;

public class InifitineSpaceController : MonoBehaviour
{

    public Transform Anchor;
    public Vector2 Bounds = Vector2.one * 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"Evaluating infitite space: X threshold read as {Mathf.Abs(this.transform.position.x - Anchor.position.x)} (bounds {Bounds.x / 2}), and Y as {Mathf.Abs(this.transform.position.y - Anchor.position.y)} (bounds {Bounds.y / 2})");

        // check if the x position is out of x bounds
        if(Mathf.Abs(this.transform.position.x - Anchor.position.x) > Bounds.x / 2)
        {
            // if the x position is on the negative bounds, bring this entity to the positive side (with a -1 buffer to not cause constant triggering) 
            if (this.transform.position.x < Anchor.position.x - (Bounds.x / 2))
            {
                this.transform.position = new Vector2(Anchor.position.x + (Bounds.x / 2) - 1, this.transform.position.y);
            }

            // else, if the x position is outside of the positive bounds bring it to the negative side (with a +1 buffer to not cause constant triggering) 
            if (this.transform.position.x > Anchor.position.x + (Bounds.x / 2))
            {
                this.transform.position = new Vector2(Anchor.position.x - (Bounds.x / 2) + 1, this.transform.position.y);

            }

        }

        // check if the y position is out of x bounds
        if (Mathf.Abs(this.transform.position.y - Anchor.position.y) > Bounds.y / 2)
        {
            // if the y position is on the negative bounds, bring this entity to the positive side (with a -1 buffer to not cause constant triggering) 
            if (this.transform.position.y < Anchor.position.y - (Bounds.y / 2))
            {
                this.transform.position = new Vector2( this.transform.position.x,Anchor.position.y + (Bounds.y / 2) - 1);
            }

            // else, if the y position is outside of the positive bounds bring it to the negative side (with a +1 buffer to not cause constant triggering) 
            if (this.transform.position.y > Anchor.position.y + (Bounds.y / 2))
            {
                this.transform.position = new Vector2( this.transform.position.x,Anchor.position.y - (Bounds.y / 2) + 1);

            }

        }
    }
}
