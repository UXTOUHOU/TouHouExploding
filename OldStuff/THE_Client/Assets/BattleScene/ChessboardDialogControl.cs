using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;

public static class ChessboardDialogControl
{
	private static bool clickedButton;

	private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

	/// <summary>
	/// 等待玩家点击一个按钮
	/// </summary>
	/// <returns></returns>
	public static bool GetClickedButton()
	{
		Invoke.Add(ShowChessboardDialog);
		autoResetEvent.WaitOne();
		return clickedButton;
	}

	public static void SetDialogString(string message)
	{
		var dialog = ChessboardObject.GetInstance().ChessboardDialog;
		dialog.transform.Find("Text").GetComponent<Text>().text = message;
	}

	/// <summary>
	/// 显示对话框。实际是修改了DialogObject的Active属性
	/// </summary>
	private static void ShowChessboardDialog()
	{
		var dialog = ChessboardObject.GetInstance().ChessboardDialog;
		dialog.SetActive(true);
	}

	private static void HideChessboardDialog()
	{
		var dialog = ChessboardObject.GetInstance().ChessboardDialog;
		dialog.SetActive(false);
	}

	/// <summary>
	/// 点击对话框的“是”按钮时调用。
	/// </summary>
	public static void OnClickButtonDialogYes()
	{
		Invoke.Add(HideChessboardDialog);
		clickedButton = true;
		autoResetEvent.Set();
	}

	/// <summary>
	/// 点击对话框的“否”按钮时调用。
	/// </summary>
	public static void OnClickButtonDialogNo()
	{
		Invoke.Add(HideChessboardDialog);
		clickedButton = false;
		autoResetEvent.Set();
	}

	public static void SetString(string message)
	{
		newMessage = message;
		Invoke.Add(setStringDelegate);
	}

	private static string newMessage;

	private static void setStringDelegate()
	{
		var dialog = ChessboardObject.GetInstance().ChessboardDialog;
		dialog.GetComponent<Text>().text = newMessage;
	}
}