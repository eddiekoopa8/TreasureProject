using UnityEngine;

public class NewMonoBehaviourScript1 : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = (anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f) >= 0.5f;
    }
}
