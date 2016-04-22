using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface ILuaUserData
{
    /// <summary>
    /// 获取lua注册表中的引用位置
    /// </summary>
    /// <returns></returns>
    int getRef();

    /// <summary>
    /// 设置lua注册表中的引用位置
    /// </summary>
    /// <param name="value"></param>
    void setRef(int value);
}

