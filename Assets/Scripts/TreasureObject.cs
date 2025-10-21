using UnityEngine;

public class TreasureObject : BB_PhysicsObject
{
    public GameObject playerObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void ActorStart()
    {
        
    }

    // Update is called once per frame
    public override void ActorUpdate()
    {
        if (collisions.Touching(playerObj) && ScnManager.GetObject("ScnMain/LvlMan").GetComponent<LevelManager>().GotKey())
        {
            ScnManager.Next();
        }
    }
}
