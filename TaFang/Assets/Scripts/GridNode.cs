using UnityEngine;
using System.Collections;

//定义场景信息
[System.Serializable]
public class MapData
{
	public enum FieldTypeID
	{
		//可以放置防守单位
		GuardPosition,
		//不可放置防守单位
		CanNotStand,
	}
	//默认可以放置防守单位
	public FieldTypeID fieldtype = FieldTypeID.GuardPosition;
}

public class GridNode : MonoBehaviour {
	public MapData _mapData;

	//显示一个图标
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (this.transform.position, "gridnode.tif");
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
