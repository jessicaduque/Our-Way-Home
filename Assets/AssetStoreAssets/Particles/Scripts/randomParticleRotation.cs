using UnityEngine;
using System.Collections;
//randomize object rotation when particle system is inactive (which requires "Looping" unchecked as well).
public class randomParticleRotation : MonoBehaviour {
	public bool x=false;
	public bool y=false;
	public bool z=false;

	public GameObject InimigoCriador;

	void OnEnable() {
		if (x) {
			this.transform.localEulerAngles += new Vector3 (Random.value * 360f,0f,0f);
		}
		if (y) {
			this.transform.localEulerAngles += new Vector3 (0f,Random.value * 360f,0f);
		}
		if (z) {
			this.transform.localEulerAngles += new Vector3 (0f,0f,Random.value * 360f);
		}
	}

    private void Update()
    {
		if(InimigoCriador != null)
        {
            if (InimigoCriador.GetComponent<SlimeRabbit>().ControlAnim.GetBool("Attacking"))
            {
				transform.position = InimigoCriador.transform.position;
				Destroy(this.gameObject, 4f);
			}
            else
            {
				Destroy(this.gameObject);
			}
			
		}
        else
        {
			Destroy(this.gameObject);
		}
    }
}