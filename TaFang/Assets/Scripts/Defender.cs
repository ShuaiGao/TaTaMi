using UnityEngine;
using System.Collections;

public class Defender : MonoBehaviour {


    //目标敌人
    Enemy m_targetEnemy;

    // 攻击范围
    public float m_attackArea = 4.0f;

    //攻击力
    public int m_power = 1;
    //攻击间隔
    public float m_attackTime = 1.0f;

    //攻击时间间隔
    public float m_timer = 0.0f;


	// Use this for initialization
	void Start () {
	    //设置当前所有的单元格为CanNotStand状态
        GridMap.Instance.m_map[(int)this.transform.position.x, (int)this.transform.position.z].fieldtype = MapData.FieldTypeID.CanNotStand;

	}
	
	// Update is called once per frame
	void Update () {
        FindEnemy();
        RotateTo();
        Attack();
	}

    //转向敌人
    public void RotateTo()
    {
        if (m_targetEnemy == null)
            return;

        Vector3 current = this.transform.eulerAngles;

        this.transform.LookAt(m_targetEnemy.transform);

        Vector3 target = this.transform.eulerAngles;

        float next = Mathf.MoveTowardsAngle(current.y, target.y, 120 * Time.deltaTime);

        this.transform.eulerAngles = new Vector3(current.x, next, current.z);
    }

    //查找敌人
    void FindEnemy()
    {
        //将目标敌人清空
        m_targetEnemy = null;

        //用于比较敌人的生命值
        int lastlife = 0;

        //在敌人列表中遍历所有的敌人
        foreach(Enemy enemy in GameManager.Instance.m_EnemyList)
        {
            //忽略生命值为0的敌人
            if (enemy.m_life == 0)
                continue;

            Vector3 pos1 = this.transform.position;
            Vector3 pos2 = enemy.transform.position;

            //与敌人的平面距离
            float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));

            //忽略在攻击范围之外的敌人
            if (dist > m_attackArea)
                continue;

            //找到生命值最低的敌人
            if(lastlife == 0 || lastlife > enemy.m_life)
            {
                m_targetEnemy = enemy;
                lastlife = enemy.m_life;
            }
        }

    }

    // 攻击敌人
    public void Attack()
    {
        //更新攻击间隔时间
        m_timer -= Time.deltaTime;

        if (m_targetEnemy == null)
            return;

        if (m_timer > 0)
            return;

        //伤害敌人
        m_targetEnemy.SetDamage(m_power);
        //初始化攻击间隔时间
        m_timer = m_attackTime;
    }
}
