using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Invoke : MonoBehaviour
{
	private static Invoke instance;
	
	public delegate void DelegateFunc();

	private static Mutex mutex = new Mutex();
	private static List<DelegateFunc> invokeList = new List<DelegateFunc>();

	private static Invoke getInstance()
	{
		return instance;
	}

	void Start()
	{
		instance = this;
	}

	void Update()
	{
		mutex.WaitOne();
		foreach (var delegateFunc in invokeList)
			delegateFunc();
		invokeList.Clear();
		mutex.ReleaseMutex();
	}


	public static void Add(DelegateFunc delegateFunc)
	{
		mutex.WaitOne();
		invokeList.Add(delegateFunc);
		mutex.ReleaseMutex();
	}
}
