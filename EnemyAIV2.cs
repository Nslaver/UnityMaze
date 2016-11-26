using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIV2 : MonoBehaviour {

    WorldCreation worldData;
    enum states : byte { PATROL = 0, WAIT = 1};
    GameObject player;
    states selfState = states.PATROL;
    public List<Node> patrol;
    private int objective;
    public Health toGuard;
    public float movSpeed;
    public float rotSpeed;

    // Use this for initialization
    void Start () {
        objective = 0;
        worldData = GameObject.FindGameObjectWithTag("WorldCreator").gameObject.GetComponent<WorldCreation>();
        patrol = new List<Node>();
        patrol.Add(getClosestNode());
        patrol.Add((Node)worldData.objLevel[patrol[0].x, patrol[0].y + 3]);
        patrol.Add((Node)worldData.objLevel[patrol[0].x + 3, patrol[0].y + 3]);
        patrol.Add((Node)worldData.objLevel[patrol[0].x + 3, patrol[0].y]);
        toGuard = worldData.healthDict[(Node)worldData.objLevel[patrol[0].x + 2, patrol[0].y + 2]];        
    }
	
	// Update is called once per frame
	void Update () {
        Node nextNode;
        nextNode = patrol[objective];
        if (toGuard.getInstace() == null)
        { 
            Destroy(gameObject);
            Destroy(this);
        }
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
                objective++;
                if (objective > 3)
                {
                    objective = 0;
                }
            }
        }
    }

    private Node getClosestNode()
    {
        Node closestNode = (Node)worldData.objLevel[Mathf.FloorToInt(transform.position.x),
                    Mathf.FloorToInt(transform.position.z)];
        return closestNode;
    }
}
