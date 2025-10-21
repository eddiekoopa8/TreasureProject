using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using UnityEngine;
// UPDATE FOR TREASURE: Add collision handling (FINALLY)
public class BB_PhysicsObject : MonoBehaviour
{
    protected class CollisionList
    {
        public CollisionList()
        {
            entries = new Dictionary<int, GameObject>();
        }
        public bool IsGood(GameObject obj)
        {
            return obj != null && obj.name != null && obj.GetInstanceID() > 0 /*I guess?????*/;
        }
        public bool IsGood(Collision2D col)
        {
            return IsGood(col.gameObject);
        }
        public void AddCollision(GameObject obj)
        {
            Debug.Log("added " + obj.name + " (" + obj.GetInstanceID() + ")");
            entries.Add(obj.GetInstanceID(), obj);
        }
        public void AddCollision(Collision2D col)
        {
            AddCollision(col.gameObject);
        }
        public void RemoveCollision(GameObject obj)
        {
            Debug.Log("removed " + obj.name + " (" + obj.GetInstanceID() + ")");
            entries.Remove(obj.GetInstanceID());
        }
        public void RemoveCollision(Collision2D col)
        {
            RemoveCollision(col.gameObject);
        }
        public bool Touching(GameObject obj, out GameObject collideID)
        {
            foreach (KeyValuePair<int, GameObject> entry in entries)
            {
                if (entry.Key == obj.GetInstanceID())
                {
                    collideID = entry.Value;
                    return true;
                }
            }
            collideID = null;
            return false;
        }
        public bool Touching(GameObject obj)
        {
            GameObject dummy = null;
            return Touching(obj, out dummy);
        }
        Dictionary<int, GameObject> entries;
    };

    protected bool isGrounded = false;
    protected bool isLeft = false;
    protected bool isRight = false;

    protected bool alwaysActive = false;

    protected bool offsetOnTop = false;

    Vector3 groundLeft = Vector3.zero;
    Vector3 groundRight = Vector3.zero;

    Vector3 leftUp = Vector3.zero;
    Vector3 leftDown = Vector3.zero;

    Vector3 rightUp = Vector3.zero;
    Vector3 rightDown = Vector3.zero;

    protected Rigidbody2D rigidbody = new Rigidbody2D();
    protected SpriteRenderer renderer = new SpriteRenderer();
    protected BoxCollider2D collide = new BoxCollider2D();

    protected Vector2 previousVelocity;

    protected CollisionList collisions;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        collide = GetComponent<BoxCollider2D>();
        collisions = new CollisionList();

        ActorStart();

        if (alwaysActive == false)
        {
            enabled = false;
        }

        Debug.Log("BB_PhysicsObject(" + name + ") is ready.");
    }

    public virtual void ActorStart()
    {
        // Debug.Log("BB_PhysicsObject(" + name + ") : no init available");
    }

    private static float sBonkBound = 1.05f;

    private static bool sBonkLine(Vector3 s, Vector3 e, float bound = 0.0f)
    {
        Collider2D[] start;
        Collider2D[] end;

        start = Physics2D.OverlapCircleAll(s, bound == 0.0f ? sBonkBound : bound);
        end   = Physics2D.OverlapCircleAll(e, bound == 0.0f ? sBonkBound : bound);

        return start.Length > 1 || end.Length > 1;
    }

    private void Update()
    {
        if (isActiveAndEnabled == false)
        {
            return;
        }

        /*
        // THIS IS VERY VERY VERY VERY **VERY** BAD CODE!!!!!

         For ground
        groundLeft = rigidbody.position + collide.offset;
        groundLeft.x -= (collide.size.x / 2) - 0.13f;
        groundLeft.y -= collide.size.y;

        groundRight = groundLeft;
        groundRight.x += collide.size.x - 0.25f;

        // For left side collision
        leftUp = rigidbody.position + collide.offset;
        leftUp.x -= collide.size.x / 2;
        leftUp.y += collide.size.y;
        leftDown = rigidbody.position + collide.offset;
        leftDown.x -= collide.size.x / 2;
        leftDown.y -= collide.size.y;

        // For right side collision
        rightUp = rigidbody.position;
        rightUp.x += collide.size.x / 2;
        rightUp.y += collide.size.y;
        rightDown = rightUp;
        rightDown.y -= collide.size.y * 2f;
        */
        
        Vector2 collideCenter = collide.bounds.center;
        Vector2 collideExtents = collide.bounds.extents;
        groundLeft = new Vector2(collideCenter.x - collideExtents.x, collideCenter.y - collideExtents.y);
        groundRight = new Vector2(collideCenter.x + collideExtents.x, collideCenter.y - collideExtents.y);
        
        leftUp = new Vector2(collideCenter.x - collideExtents.x, (collideCenter.y + collideExtents.y) - 1);
        leftDown = new Vector2(collideCenter.x - collideExtents.x, (collideCenter.y - collideExtents.y) + 1);
        
        rightUp = new Vector2(collideCenter.x + collideExtents.x, (collideCenter.y + collideExtents.y) - 1);
        rightDown = new Vector2(collideCenter.x + collideExtents.x, (collideCenter.y - collideExtents.y) + 1);

        isGrounded = sBonkLine(groundLeft, groundRight) && (rigidbody.linearVelocityY <= 0.1f && rigidbody.linearVelocityY >= -0.01f);
        isLeft = sBonkLine(leftUp, leftDown, 0.095f);
        isRight = sBonkLine(rightUp, rightDown, 0.095f);

        // DEBUGGING PURPOSES
        //if (Input.GetKeyDown(KeyCode.Space))
        {
            /*Debug.Log("INFO");
            Debug.Log("========================");
            Debug.Log("rigid " + rigidbody.position);
            Debug.Log("groundLeft " + groundLeft);
            Debug.Log("groundRight " + groundRight);
            Debug.Log("collide.offset " + collide.offset);
            Debug.Log("collide.size " + collide.size);
            Debug.Log("isGrounded " + isGrounded);
            Debug.Log("rigidbody.velocity " + rigidbody.velocity);
            Debug.Log("========================");*/
        }

        ActorUpdate();

        previousVelocity = rigidbody.linearVelocity;
    }

    public virtual void ActorUpdate()
    {
        // Debug.Log("uppdate!!!");
    }

    void OnBecameVisible()
    {
        if (alwaysActive == false)
        {
            enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collisions.IsGood(collision))
        {
            return;
        }
        collisions.AddCollision(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collisions.IsGood(collision))
        {
            return;
        }
        collisions.RemoveCollision(collision);
    }
}