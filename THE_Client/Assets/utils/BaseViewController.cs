using System;
using UnityEngine;

public class BaseViewController : MonoBehaviour
{
    protected string _windowName;

    public virtual void initController()
    {

    }

    public virtual void onPopUp(object[] args)
    {
        PopUpManager.getInstance().addPopUp(this._windowName,"");
    }

    public virtual void onClose()
    {
    }
}