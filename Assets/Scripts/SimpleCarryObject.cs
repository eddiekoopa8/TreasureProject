using UnityEngine;

public class SimpleCarryObject : BB_PhysicsObject
{
    bool bThrown = false;
    static int speed = 5;
    public enum ThrowDir
    {
        LEFT = -1,
        RIGHT = 1,
    };
    ThrowDir mDir = ThrowDir.LEFT;
    void Start()
    {
        
    }

    public void Thrown(ThrowDir dir)
    {
        mDir = dir;
        bThrown = true;
    }

    void Update()
    {
        rigidbody.linearVelocityX = (speed * (int)mDir) * (bThrown?1:0);
    }
}
