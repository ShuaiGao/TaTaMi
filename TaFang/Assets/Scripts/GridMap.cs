using UnityEngine;
using System.Collections;

public class GridMap : MonoBehaviour {
	static public GridMap Instance = null;

	//是否显示场景信息
	public bool m_debug = false;

	//场景大小
	public int MapSizeX = 32;
	public int MapSizeZ = 32;

	//一个二维数组用于保存场景信息
	public MapData[,] m_map;

	void Awake(){
		Instance = this;
		//初始化场景信息
		this.BuildMap ();
	}

	//创建地图
	[ContextMenu("BuildMap")]
	public void BuildMap(){
		//创建二维数组
		m_map = new MapData[MapSizeX, MapSizeZ];
		for (int i = 0; i < MapSizeX; i++) {
			for (int j = 0; j < MapSizeX; j++) {
				m_map[i,j] = new MapData();
			}
		}

		//获得所有Tag为gridnode的节点
		GameObject[] nodes = (GameObject[])GameObject.FindGameObjectsWithTag ("gridenode");

		foreach (GameObject nodeobj in nodes) {
			//获得节点
			GridNode node = nodeobj.GetComponent<GridNode>();

			Vector3 pos = nodeobj.transform.position;

			//如果节点位置超出了场景的范围，则忽略
			if((int)pos.x >= MapSizeX || (int)pos.z >= MapSizeZ)
				continue;

			//设置格子的属性
			m_map[(int)pos.x, (int)pos.z].fieldtype = node._mapData.fieldtype;
		}


	}

	//绘制场景信息
	void OnDrawGizmos(){
		if (!m_debug || m_map == null)
			return;
		//线条的颜色
		Gizmos.color = Color.blue;
		//绘制线条的高度
		float height = 0;

		//绘制网格
		for (int i = 0; i < MapSizeX; i++) {
			Gizmos.DrawLine(new Vector3(i, height,0), new Vector3(i, height, MapSizeZ));
		}
		for (int i = 0; i < MapSizeZ; i++) {
			Gizmos.DrawLine(new Vector3(0, height,i), new Vector3(MapSizeX, height, i));
		}
		//改为红色
		Gizmos.color = Color.red;
		
		for (int i = 0; i < MapSizeX; i++) {
			for (int j = 0; j < MapSizeZ; j++) {
				//在不能防止防守区域的方格内绘制红色的方块
				if(m_map[i,j].fieldtype == MapData.FieldTypeID.CanNotStand){
					Gizmos.color = new Color(1,0,0,0.5f);
					
					//绘制红色的方块
					Gizmos.DrawCube (new Vector3(i+0.5f, height, j+0.5f), new Vector3(1, height + 0.1f,1));
				}
				
			}
		}
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
