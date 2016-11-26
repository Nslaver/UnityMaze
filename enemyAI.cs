using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

    WorldCreation worldData;
    enum states : byte {FOLLOW= 0, SEARCH = 1, PATROL = 2};
    GameObject player;
    states selfState = states.FOLLOW;
    RaycastHit hit;
    Vector3 rayDirection;
    public float movSpeed;
    public float rotSpeed;
    public List<Node> openSet;
    public List<Node> closeSet;
    public Material selectedMaterial;
    public Material nodeMaterial;
    private int tick;
    public int maxTicks;
    private Node closestNode;
    private Node currentNode;
    private Node playerNode;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        worldData = GameObject.FindGameObjectWithTag("WorldCreator").gameObject.GetComponent<WorldCreation>();
        openSet = new List<Node>();
        closeSet = new List<Node>();
        closestNode = getClosestNode();
        playerNode = getPlayerNode();
    }
	

    private Node getClosestNode()
    {
        Node closestNode = (Node)worldData.objLevel[Mathf.FloorToInt(transform.position.x),
                    Mathf.FloorToInt(transform.position.z)];
        return closestNode;
    }
    private Node getPlayerNode()
    {
        Node playerNode = (Node)worldData.objLevel[Mathf.FloorToInt(player.transform.position.x),
                    Mathf.FloorToInt(player.transform.position.z)];
        return playerNode;
    }

    void Update () {
        if(lineSight()){
            selfState = states.SEARCH;
        }
        else
        {
            selfState = states.FOLLOW;
        }
        switch (selfState){
            case states.FOLLOW:
                // DEDBUG: print("pos:X" + transform.position.x + "Y:" +transform.position.z);
                // DEDBUG: print("ajs:X" + Mathf.Ceil(transform.position.x) + "Y:" + Mathf.Ceil(transform.position.z));
                buildPath();
                followPath();
                break;
            case states.SEARCH:
                search();
                break;
        }
	    
	}
    
    // Verificar linea de vista entre el este enemigo y el jugador
    public bool lineSight()
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
    public void search()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movSpeed / 50);
        float step = rotSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, player.transform.position, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void followPath()
    {
        Node nextNode;
        nextNode = closeSet[0];
        if (nextNode != null)
        {
            Vector3 target = nextNode.get3dLocation();
            transform.position = Vector3.MoveTowards(transform.position, target, movSpeed / 100);
            float step = rotSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, target, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
            //print("DISTANCE:" + Vector3.Distance(transform.position, target));
            if (Vector3.Distance(transform.position, target) < 1)
            {
                closeSet[0].getInstace().GetComponentInChildren<Renderer>().material = nodeMaterial;
                closeSet.RemoveAt(0);
            }
        }            
    }

    private void cleanNodes()
    {
        foreach (Node clearNode in closeSet)
        {
            clearNode.getInstace().GetComponentInChildren<Renderer>().material = nodeMaterial;
        }
    }

    // Mover el el AI en modo busqueda verificando el cambio en posiciones
    public void buildPath()
    {
        try
        {
            if (playerNode != getPlayerNode())
            {
            
                playerNode = getPlayerNode();
                closestNode = getClosestNode();
                cleanNodes();

                openSet = new List<Node>();
                closeSet = new List<Node>();                
            }
        }
        catch (Exception exc)
        {
            exc.GetType();
            //DEBUG print("NE:" + e.ToString());
        }

        openSet.Add(closestNode);
        while (tick < maxTicks && openSet.Count > 0 && (closeSet.Count == 0 || !closeSet.Contains(playerNode)) )
        {
            tick++;
            double[] FVal = new double[openSet.Count]; ;
            int low = 0;
            for (int i = 0; i < openSet.Count; i++)
            {
                //DEBUG print("PL" + player.transform.position.x + "Y" + player.transform.position.z);
                float xdis = (float)(openSet[i].x - playerNode.x);
                float ydis = (float)(openSet[i].y - playerNode.y);
                double h = (double)Mathf.Sqrt(xdis*xdis + ydis*ydis);
                FVal[i] = 10 + h;
                //DEBUG print("ND:" + listaAbierta.ToString() + "F" + F[i]);
                if (FVal[i] < FVal[low]) low = i;                
            }
                        
            currentNode = openSet[low];
            currentNode.getInstace().GetComponentInChildren<Renderer>().material = selectedMaterial;
            closeSet.Add(currentNode);
            openSet.Clear();
            List < Node > neighbors = worldData.getNeighbors(currentNode);
            //DEBUG print("NE:" + neighbors.Count);
            foreach (Node reviewNode in closeSet)
            {
                neighbors.Remove(reviewNode);
            }            
            openSet.AddRange(neighbors);
        }
        tick = 0;
    }
}
