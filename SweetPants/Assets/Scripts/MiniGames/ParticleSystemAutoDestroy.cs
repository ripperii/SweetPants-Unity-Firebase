using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]

public class ParticleSystemAutoDestroy : MonoBehaviour {

    
	// Use this for initialization
	void Start () {

         Destroy(gameObject, this.GetComponent<ParticleSystem>().main.duration); 
     
	}
}
