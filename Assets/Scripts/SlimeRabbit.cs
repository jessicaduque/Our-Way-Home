using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRabbit : MonoBehaviour
{
    // Animador
    private Animator ControlAnim;

    // Stats
    public int hp = 5;
    float expDada = 4;

    private void Start()
    {
        // Animador
        ControlAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Attack")
        {
            TomeiDano();
        }
    }

    public void TomeiDano()
    {
        hp--;
        if (hp <= 0)
        {
            ControlAnim.SetTrigger("Death");
            Destroy(this.gameObject, 1f);
        }
    }
}