using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UnitOperationViewController : BaseViewController
{
    public override void initController()
    {
        base.initController();
        this._windowName = WindowName.UnitOperationView;
    }

    public override void onPopUp(object[] args)
    {
        base.onPopUp(args);
        // 设置位置
        Cell cell = BattleGlobal.SelectCell;
        this.transform.localPosition = BattleUtils.getCellPosByLocation(cell.location.y, cell.location.x);
        // 添加事件监听
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CommandManager.getInstance().runCommand(CommandConsts.CommandConsts_RemoveWindow, this._windowName);
        }
    }
}
