using UnityEngine;

public class LevelManager : MonoBehaviour
{
    bool bGotKey = false;
    public GameObject keyObject;
    void Start()
    {
        
    }
    public bool GotKey()
    {
        return bGotKey;
    }
    public void GetKey()
    {
        if (bGotKey)
        {
            return;
        }
        bGotKey = true;
        keyObject.SetActive(false);
        Destroy(keyObject);
    }
    void Update()
    {
        
    }
}
