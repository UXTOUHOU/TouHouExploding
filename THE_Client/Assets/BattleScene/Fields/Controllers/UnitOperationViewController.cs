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
        // 设置位置
        Cell cell = BattleGlobal.SelectedCell;
        // 添加到UILayer
        Chessboard.addChildOnLayer(this.gameObject, BattleConsts.BattleFieldLayer_UI, cell.location.y, cell.location.x);
        this.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            this.gameObject.SetActive(false);
        }
    }
}
