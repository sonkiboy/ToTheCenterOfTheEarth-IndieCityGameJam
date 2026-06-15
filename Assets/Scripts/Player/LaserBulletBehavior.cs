using System.Collections;
using UnityEngine;

public class LaserBulletBehavior : Bullet
{
    BasicCurvedLineController lineController;
    Transform hitPoint;

    public LayerMask HitMask;
    public Vector2 HitArea;
    public float DamageRate = .25f;

    [SerializeField] Transform target;

    
    private void Start()
    {
        lineController = GetComponent<BasicCurvedLineController>();
        hitPoint = transform.Find("HitPoint");
        StartCoroutine(FireLazer());
        StartCoroutine(DamageTarget());
    }

    private void Update()
    {
        lineController.Origin = this.transform.position;

        if(target != null )
        {
            lineController.Target = (Vector2)target.transform.position + (Vector2.one / 2);
            lineController.RelativeBezPoint = lineController.Origin + ((Vector2)this.transform.right * Vector2.Distance(lineController.Origin, lineController.Target));
        }

        hitPoint.transform.position = lineController.Line.GetPosition(lineController.Line.positionCount-1) ;


    }

    // shoop de woop
    IEnumerator FireLazer()
    {
        Collider2D[] foundColliders;
        Transform closestObj;


        while (GameManager.Instance.InputManager.FireInput.IsPressed())
        {
            //Debug.Log($"Starting Laser Fire, Pos: {this.transform.position} Rot: {this.transform.eulerAngles.z}");

            // cast the area check
            foundColliders = Physics2D.OverlapBoxAll((Vector2)this.transform.position + ((HitArea / 2) * this.transform.right), HitArea, this.transform.eulerAngles.z, HitMask);

            // if there were objects, loop through and find the closest object
            if (foundColliders.Length > 0)
            {
                //Debug.Log("Found colliders");

                closestObj = foundColliders[0].transform;
                for (int i = 1; i < foundColliders.Length; i++)
                {
                    if (Vector2.Distance(this.transform.position, closestObj.position) > Vector2.Distance(this.transform.position, foundColliders[i].transform.position))
                    {
                        closestObj = foundColliders[i].transform;
                    }
                }

                if (target != closestObj)
                {
                    Debug.Log($"setting target object to {closestObj.name}");
                    target = closestObj;

                }


            }

            // if there was nothing found, just send the laser straight forward to max distance
            else
            {
                if (target != null)
                {
                    Debug.Log("No target found");
                    target = null;
                }

            }



            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }


    IEnumerator DamageTarget()
    {
        while (GameManager.Instance.InputManager.FireInput.IsPressed())
        {
            lineController.Target = this.transform.position + (this.transform.right * (HitArea.x - 1));
            lineController.RelativeBezPoint = this.transform.position + (this.transform.right * (HitArea.x - 1));

            while (target != null)
            {
                //Debug.Log($"Damagable Target Detected as : {target.name}");
                switch (target.tag)
                {
                    case ("Tile"):
                        TileBehavior tile = target.GetComponent<TileBehavior>();
                        if (tile != null)
                        {
                            //'Debug.Log($"Locked into mining tile {tile.name}");
                            //Debug.Log($"Initial variables read as: {tile.Health > 0} {GameManager.Instance.InputManager.FireInput.IsPressed()} && {target == tile.transform}");

                            while (tile.Health > 0 && GameManager.Instance.InputManager.FireInput.IsPressed() && target == tile.transform)
                            {
                                //Debug.Log($"Frame check variables read as: {tile.Health > 0} {GameManager.Instance.InputManager.FireInput.IsPressed()} && {target == tile.transform}");
                               // Debug.Log($"Tile health is {tile.Health}");

                                
                                tile.Health -= Damage;

                                yield return new WaitForSeconds(DamageRate);

                            }
                            Debug.Log($"No More Tile");
                        }


                        break;

                    case ("Enemy"):
                        Enemy enemyComp = target.GetComponent<Enemy>();
                        if (enemyComp != null)
                        {
                            while (enemyComp.Health > 0 && GameManager.Instance.InputManager.FireInput.IsPressed() && target == enemyComp.transform)
                            {
                                //lineController.Target = (Vector2)enemyComp.transform.position + (Vector2.one / 2);
                                //lineController.RelativeBezPoint = lineController.Origin + ((Vector2)this.transform.right * Vector2.Distance(lineController.Origin, lineController.Target));

                                enemyComp.Health -= Damage;
                                yield return new WaitForSeconds(DamageRate);

                            }
                        }
                        break;

                }
                yield return new WaitForFixedUpdate();

            }

            yield return new WaitForFixedUpdate();
        }
    }
}
