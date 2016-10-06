using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldCreation : MonoBehaviour {

    public static string[] level = {
    "WWWWWWWWWWWWWWWWWWWWWW",
    "W    W       w       W",
    "W    W               W",
    "W    W     WWWWWW    W",
    "W    WWW WWW    W    W",
    "W    W          WWw wW",
    "WWW  W     W         W",
    "W    W     W W       W",
    "W    W    PW     WWW W",
    "W    WWW WWWE  W     W",
    "W          W   W W   W",
    "WW WWwWW   WW  W W   W",
    "W    W               W",
    "W    W          W    W",
    "W               W    W",
    "WWWWWWWWWWWWWWWWWWWWWW",
    };

    public TwoDObj[,] objLevel = new TwoDObj[level.Length, level[0].Length];
    public List<Node> nodeList = new List<Node>();
    public List<Wall> wallList = new List<Wall>();
    public List<Enemy> enemyList = new List<Enemy>();
    public Player playerInst;
        
    public GameObject wallPrefab;
    public GameObject enemyPrefab;
    public GameObject nodePrefab;
    public GameObject playerPrefab;
    public GameObject playerInstace;
    private Camera mainCamera;


    // Use this for initialization
    void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        int x = 0;
        TwoDObj temp;
        foreach( string e in level)
        {
            int y = 0;
            foreach(char col in e.ToCharArray())
            {
                Debug.Log("X:" + x + "Y:" + y);
                if (col == 'P')
                {
                    playerInst = new Player(playerPrefab, x, y);
                    playerInst.setInstace(playerInstace);                    
                    Debug.Log("P" + playerInst.getPrefab());
                }
                if (col == 'W')
                {
                    temp = new Wall(wallPrefab,x, y);
                    objLevel[x,y] = temp; 
                    wallList.Add((Wall)temp);
                    GameObject clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;

                    temp.setInstace(clone);
                    Debug.Log("P" + temp.getPrefab());
                }
                if (col == 'E')
                {
                    temp = new Enemy(enemyPrefab,x, y);
                    enemyList.Add((Enemy)temp);
                    GameObject clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
                    temp.setInstace(clone);
                    Debug.Log("P" + temp.getPrefab());
                }
                // Instanciar lista de nodos
                if (col == ' ' || col == 'E' || col == 'P'){
                    temp = new Node(nodePrefab, x, y);
                    //Instancio mis nodos
                    objLevel[x,y] = temp;
                    nodeList.Add((Node)temp);
                    GameObject clone = Instantiate(temp.getPrefab(), temp.get3dLocation(), Quaternion.identity) as GameObject;
                    temp.setInstace(clone);
                    Debug.Log("P" + temp.getPrefab());
                }
                y++;                
            }
            x++;
        }
	}

    // Update is called once per frame
    void Update() {
    }
}

public class TwoDObj
{
    public int x;
    public int y;
    public GameObject prefab;
    public GameObject instance;

    public TwoDObj(GameObject prefab, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.prefab = prefab;
    }

    public virtual GameObject getPrefab()
    {
        return prefab;
    }

    public virtual Vector3 get3dLocation()
    {
        return new Vector3(x,1,y);
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