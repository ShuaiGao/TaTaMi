using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    //敌人的标识
    [System.Serializable]
    public class EnemyTable
    {
        public string enemyName = "";
        public Transform enemyPrefab;
    }

    //XMl数据
    public class SpawnData
    {
        //波数
        public int wave = 1;
        public string enemyname = "";
        public int level = 1;
        public float wait = 1.0f;
    }

    // 敌人
    public EnemyTable[] m_enemies;

    //起始路点
    public PathNode m_startNode;

    //存储敌人出场顺序的xml
    public TextAsset xmldata;

    // 保存所有的从xml读取的数据
    ArrayList m_enemylist;

    // 距离下一个敌人出场的时间
    float m_timer = 0;

    //出场敌人的序列号
    int m_index = 0;

    //当前波数的敌人数量，只有销毁当前波内所有的敌人，才进入下一波
    public int m_liveEnemy = 0;

	// Use this for initialization
	void Start () {
        //读取xml
        ReadXml();

        // 获取下一个敌人
        SpawnData data = (SpawnData)m_enemylist[m_index];
        m_timer = data.wait;
	}
	
	// Update is called once per frame
	void Update () {
        SpawnEnemy();
	}

    void ReadXml()
    {
        m_enemylist = new ArrayList();

        XMLParser xmlparse = new XMLParser();
        XMLNode node = xmlparse.Parse(xmldata.text);

        XMLNodeList list = node.GetNodeList("ROOT>0>table");
        for(int i = 0; i < list.Count; i++)
        {
            string wave = node.GetValue("ROOT>0>table>" + i + ">@wave");
            string enemyname= node.GetValue("ROOT>0>table>" + i + ">@enemyname");
            string level = node.GetValue("ROOT>0>table>" + i + ">@level");
            string wait = node.GetValue("ROOT>0>table>" + i + ">@wait");

            SpawnData data = new SpawnData();
            data.wait = int.Parse(wait);
            data.wave = int.Parse(wave);
            data.level = int.Parse(level);
            data.enemyname = enemyname;
            m_enemylist.Add(data);
        }
    }

    void SpawnEnemy()
    {
        //如果已经生成所有敌人
        if (m_index >= m_enemylist.Count)
            return;

        //获取下一个敌人
        SpawnData data = (SpawnData)m_enemylist[m_index];

        //如果下一个敌人是下一波需要等待前一波的敌人全部消亡
        if (GameManager.Instance.m_wave < data.wave && m_liveEnemy > 0)
            return;

        //等待
        m_timer -= Time.deltaTime;
        if (m_timer > 0)
            return;

        if(GameManager.Instance.m_wave < data.wave )
        {
            //增加一波
            GameManager.Instance.SetWave(data.wave);
        }
        //查找敌人
        Transform enemyprefab = FindEnemy(data.enemyname);

        //生成敌人
        if(enemyprefab != null)
        {
            Transform trans = (Transform)Instantiate(enemyprefab, this.transform.position, Quaternion.identity);
            Enemy enemy = trans.GetComponent<Enemy>();

            //设置敌人的出发点
            enemy.m_currentNode = m_startNode;

            //设置生成点
            enemy.m_spawn = this;

            //设置敌人出事旋转方向
            enemy.transform.LookAt(m_startNode.transform);
            float ry = enemy.transform.eulerAngles.y;
            enemy.transform.eulerAngles = new Vector3(0, ry, 0);

            //根据data.level设置敌人等级，本实例中略
        }

        //下一个
        m_index++;
        if (m_index >= m_enemylist.Count)
            return;

        //获取下一个敌人的数据
        SpawnData nextdata = (SpawnData)m_enemylist[m_index];

        //生成下一个敌人需要等待的时间
        m_timer = data.wait;
    }

    //在EnemyTable查找enemy的prefab
    Transform FindEnemy(string enemyname)
    {
        foreach (EnemyTable enemy in m_enemies)
        {
            if (enemy.enemyName.CompareTo(enemyname) == 0)
                return enemy.enemyPrefab;
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawerner.tif");
    }
}
