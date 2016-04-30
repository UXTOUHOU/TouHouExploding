using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChessboardObject : MonoBehaviour
{

	public GameObject ChessboardRect;
	public GameObject ChessboardCellPrefab;
	public GameObject CanvasObject;
	public GameObject Background;               // Background所在的Panel
	public GameObject ChessboardDialog;

	public GameObject BgLayer;
	public GameObject UnitLayer;
	public GameObject UILayer;

	private static ChessboardObject _instance;

	public static ChessboardObject GetInstance()
	{
		return _instance;
	}

	void Start()
	{
		_instance = this;
	}
}