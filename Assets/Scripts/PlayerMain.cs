using UnityEditor;
using UnityEngine;

public class PlayerMain : BB_PhysicsObject
{
    // typez
    enum Dir
    {
        LEFT = -1,
        RIGHT = 1
    };

    // collision
    bool hitWall = false;
    bool isHit = false;

    // moving
    bool isMoving = false;
    Dir direction = Dir.RIGHT;

    // jumping
    bool requestJump = false;
    bool isJumping = false;

    static int mSpeed = 4;
    static int jHeight = 12;

    public override void ActorStart()
    {
        
    }
    public override void ActorUpdate()
    {
        bool INPUT_RIGHT = Input.GetKey(KeyCode.RightArrow);
        bool INPUT_LEFT = Input.GetKey(KeyCode.LeftArrow);
        bool INPUT_JUMP = Input.GetKeyDown(KeyCode.Space);

        hitWall = isLeft && isRight;
        isHit = hitWall && isGrounded;

        // Input reading
        // TODO: make it not use hardcoded keys
        if (INPUT_RIGHT)
        {
            isMoving = true;
            direction = Dir.RIGHT;
        }
        else if (INPUT_LEFT)
        {
            isMoving = true;
            direction = Dir.LEFT;
        }
        
        if (!INPUT_LEFT && !INPUT_RIGHT)
        {
            isMoving = false;
        }

        if (INPUT_JUMP && isGrounded)
        {
            Debug.Log("requestJump !!!!");
            requestJump = true;
        }

        // Movement
        if (isMoving)
        {
            rigidbody.linearVelocityX = mSpeed * (int)direction;
        }
        else
        {
            rigidbody.linearVelocityX = 0;
        }

        // Jumping
        if (requestJump)
        {
            rigidbody.linearVelocityY = jHeight;
            requestJump = false;
            isJumping = true;
        }

        if (isGrounded && isJumping)
        {
            isJumping = false;
        }
    }
}
