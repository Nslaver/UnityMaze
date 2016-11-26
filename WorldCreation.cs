using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCreation : MonoBehaviour {

    /**
    * E = Enemigo V1 (Estrella)
    * V = Enemigo V2 (Vigila)
    * P = Player
    * W = Wall
    * H = Vidas / Objetivos
    **/

    //00
    //
    public static string[] level = {
    "WWWWWWWWWWWWWWWWWWWWWW",
    "WV                   W",
    "W    WWWW WWWWW WWWW W",
    "W  H W          WV   W",
    "W    W          W    W",
    "W WWWW     WWWWWW  H W",
    "W    WWW WWW    W    W",
    "W    W          W WWWW",
    "WWW  W     W         W",
    "W          W W       W",
    "W    W    PW     WWW W",
    "W    WWW WWWE  W     W",
    "W          W   W W   W",
    "WW WWWWW   WW  W W   W",
    "WV   W               W",
    "W    W               W",
    "W  H W          W   EW",
    "W    W          W    W",
    "WWWWWWWWWWWWWWWWWWWWWW",
    };
   //0123456789012345678901

    public TwoDObj[,] objLevel = new TwoDObj[level.Length, level[0].Length];
    public List<Node> nodeList = new List<Node>();
    public List<Wall> wallList = new List<Wall>();
    public List<Enemy> enemyList = new List<Enemy>();
    public Dictionary<Node, Health> healthDict = new Dictionary<Node, Health>(); 
    public Player playerInst;
        
    public GameObject wallPrefab;
    public GameObject enemyPrefab;
    public GameObject nodePrefab;
    public GameObject playerPrefab;
    public GameObject playerInstace;
    public GameObject enemyV2Prefab;
    public GameObject healthPrefab;
    private bool showNode;


    // Use this for initialization
    void Start () {
        int x = 0;
        TwoDObj temp;
        showNode = false;
        foreach( string e in level)
        {
            int y = 0;
            foreach(char col in e.ToCharArray())
            {
                //Debug.Log("X:" + x + "Y:" + y);
                GameObject clone;
                Node tempNode;
                switch (col)
                {
                    case 'P':
                        playerInst = new Player(playerPrefab, x, y);
                        playerInst.setInstace(playerInstace);
                        createNode(x, y);
                        break;
                    case 'W':
                        temp = new Wall(wallPrefab, x, y);
                        objLevel[x, y] = temp;
                        wallList.Add((Wall)temp);
                        clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
                        temp.setInstace(clone);
                        break;
                    case 'E':
                        temp = new Enemy(enemyPrefab, x, y);
                        enemyList.Add((Enemy)temp);
                        clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
                        temp.setInstace(clone);
                        createNode(x, y);
                        break;
                    case 'V':
                        temp = new Enemy(enemyV2Prefab, x, y);
                        enemyList.Add((Enemy)temp);
                        clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
                        temp.setInstace(clone);
                        createNode(x, y);
                        break;
                    case 'H':
                        temp = new Health(healthPrefab, x, y);
                        tempNode = createNode(x, y);
                        healthDict.Add(tempNode, (Health)temp);
                        clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
                        temp.setInstace(clone);
                        break;
                    default:
                        createNode(x, y);
                        break;
                }
                y++;                
            }
            x++;
        }
	}

    private Node createNode(int x, int y)
    {
        TwoDObj temp = new Node(nodePrefab, x, y);
        //Instancio mis nodos
        objLevel[x, y] = temp;
        nodeList.Add((Node)temp);
        GameObject clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
        temp.setInstace(clone);
        clone.SetActive(showNode);
        return (Node)temp;
    }

    public void switchNodes()
    {
        showNode = !showNode;
        foreach (Node a in nodeList)
        {
            a.getInstace().SetActive(showNode);
        }
    }

    public List<Node> getNeighbors(Node current)
    {
        List < Node > list = new List<Node>();
        if(objLevel[current.x + 1 , current.y] is Node)
        {
            list.Add((Node)objLevel[current.x + 1, current.y]);
        }
        if (objLevel[current.x, current.y + 1] is Node)
        {
            list.Add((Node)objLevel[current.x, current.y + 1]);
        }
        if (objLevel[current.x - 1, current.y] is Node)
        {
            list.Add((Node)objLevel[current.x - 1, current.y]);
        }
        if (objLevel[current.x, current.y -1] is Node)
        {
            list.Add((Node)objLevel[current.x, current.y - 1]);
        }
        return list;
    }

    // Update is called once per frame
    void Update() {
    }
}

public class TwoDObj
{
    public int x;
    public int y;
    public float z;
    public GameObject prefab;
    public GameObject instance;

    public TwoDObj(GameObject prefab, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.z = 1;
        this.prefab = prefab;
    }

    public virtual GameObject getPrefab()
    {
        return prefab;
    }

    public virtual Vector3 get3dLocation()
    {
        return new Vector3(x,z,y);
    }

    public virtual void setInstace(GameObject instance)
    {
        this.instance = instance;
        this.instance.transform.position = this.get3dLocation();
    }

    public virtual GameObject getInstace()
    {
        return instance;
    }

    public override string ToString() {
        return "(X:"+x+"Y:"+y+")";
    }
}

public class Wall : TwoDObj 
{
    public Wall(GameObject prefab, int x, int y) : base(prefab, x, y)
    {
    }

    public override Vector3 get3dLocation()
    {
        return new Vector3(x, this.getPrefab().transform.localScale.y/2, y);
    }
}

public class Enemy : TwoDObj
{
    public Enemy(GameObject prefab, int x, int y) : base(prefab, x, y)
    {
    }

}

public class Player : TwoDObj
{
    public Player(GameObject prefab, int x, int y) : base(prefab, x, y)
    {
    }
}

public class Health : TwoDObj
{
    public Health(GameObject prefab, int x, int y) : base(prefab, x, y)
    {
        this.z = 0;
    }
}