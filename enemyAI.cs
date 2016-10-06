using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyAI : MonoBehaviour {

    WorldCreation temp;
    //public static enum states = {BUSCAR,PERSEGUIR};
    enum states : byte {BUSCAR= 0, PERSEGUIR = 1};
    // Use this for initialization
    GameObject player;
    states selfState = states.BUSCAR;
    RaycastHit hit;
    Vector3 rayDirection;
    public float movSpeed = 10.0f;
    public float rotSpeed = 2.0f;
    public List<Node> listaAbierta;
    public List<Node> listaCerrada;


    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        listaAbierta = new List<Node>();
        listaCerrada = new List<Node>();
    }
	
	// Update is called once per frame
	void Update () {
        if(lineaVista()){
            selfState = states.PERSEGUIR;
        }
        else
        {
            selfState = states.BUSCAR;
        }
        switch (selfState){
            case states.BUSCAR:
                buscar();
                break;
            case states.PERSEGUIR:
                perseguir();
                break;
        }
	    
	}
    
    // Vertificar linea de vista entre el este enemigo y el jugador
    public bool lineaVista()
    {
        rayDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                // DEDBUG: print("Vista");
                return true;
            }
            else {
                // DEDBUG: print("No Vista");
                return false;
            }
        }
        return false;
    }

    // Mover el el AI en perseguir
    public void perseguir()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movSpeed / 50);
        float step = rotSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, player.transform.position.normalized, step, 0.0f);
    }

    // Mover el el AI en modo busqueda
    public void buscar()
    {
        
    }
}
