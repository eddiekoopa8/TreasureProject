using UnityEngine;

public class PlayerMain : BB_PhysicsObject
{
    bool hitWall = false;
    bool isHit = false;
    public override void ActorStart()
    {
        
    }
    public override void ActorUpdate()
    {
        hitWall = isLeft && isRight;
        isHit = hitWall && isGrounded;

        if (isGrounded && Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.linearVelocityX = 3;
        }


    }
}
