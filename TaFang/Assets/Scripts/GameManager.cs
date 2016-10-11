using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public bool m_debug = false;
    public ArrayList m_PathNodes;

    public ArrayList m_EnemyList = new ArrayList();
    //波数 不在Inspection窗口显示
    [HideInInspector]
    public int m_wave = 1;

    //生命
    public int m_life = 10;

    //点数
    public int m_point = 10;

    //文字
    Text m_txt_wave;
    Text m_txt_life;
    Text m_txt_point;
    
    void Awake()
    {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
	    //获取文字
        m_txt_wave = this.transform.Find("Canvas/txt_wave").GetComponent<Text>();
        m_txt_life = this.transform.Find("Canvas/txt_life").GetComponent<Text>();
        m_txt_point = this.transform.Find("Canvas/txt_point").GetComponent<Text>();

        ////初始化文字
        m_txt_wave.text = "<color=red>Wave</color> " + m_wave;
        m_txt_life.text = "<color=red>Life</color> " + m_life;
        m_txt_point.text = "<color=red>Point</color> " + m_point;

        BuildPath();
	}
	
	// Update is called once per frame
	void Update () {
	    //鼠标操作
        bool press = Input.GetMouseButton(0);

        //获得鼠标移动距离
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //移动摄像机
        GameCamera.Inst.Control(press, mx, my);
	}

    //更新波数
    public void SetWave(int wave)
    {
        m_wave = wave;
        m_txt_wave.text = "<color=red>Wave</color>" + m_wave;
    }

    //更新生命
    public void SetDamage(int life)
    {
        m_life -= life;
        m_txt_life.text = "<color=red>Life</color>" + m_life;
    }

    //更新点数
    public void SetPoint(int point)
    {
        m_point += point;
        m_txt_point.text = "<color=red>Point</color>" + m_point;
    }

    [ContextMenu("BuildPath")]
    void BuildPath()
    {
        m_PathNodes = new ArrayList();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("pathnode");

        for(int i = 0; i < objs.Length; i++)
        {
            PathNode node = objs[i].GetComponent<PathNode>();
            m_PathNodes.Add(node);
        }
    }

    void OnDrawGizmos()
    {
        if (!m_debug || m_PathNodes == null)
            return;

        Gizmos.color = Color.red;

        foreach(PathNode node in m_PathNodes)
        {
            if(node.m_next != null)
            {
                Gizmos.DrawLine(node.transform.position, node.m_next.transform.position);
            }
        }
    }
}
