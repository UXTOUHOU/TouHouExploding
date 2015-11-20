using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THE_Core
{
    public class IDList : IID//用于单项ID管理
    {
        public string name { get; set; }
        public int Id { get; set; }//无指定ID时为-1
        private List<Object> _objList = new List<Object>();
        private List<int> _idList = new List<int>();
        private int _currentID = 0;
        public IDList(string _name)//初始化一个空list
        {
            name = _name;
            Id = -1;
        }
        public IDList(List<Object> list)//由已有ID导入
        {
            name = list.GetType().ToString();
            Id = -1;
            SetID(list);
        }
        public IDList(Type type)//由类型生成
        {
            name = type.ToString();
            Id = -1;
        }
        public IDList(Object obj)//自动填补名称 输入的obj需要集成IID接口
        {
            name = obj.GetType().ToString();
            ApplyID(obj);
        }
        public int ApplyID(Object obj)//输入的obj需要集成IID接口，如已存在输入-1
        {
            if (IsExist(obj)) return -1;
            do
            {
                _currentID++;
            } while (IsExist(_currentID));
            _objList.Add(obj);
            _idList.Add(_currentID);
            _ChangeObjID(obj, _currentID);
            return _currentID;
        }
        public List<Object> SetID(List<Object> list)//为大量obj注册为其内置的ID，如果ID不可被注册（即已被占用）则返回其本身（如已存在会删除旧ID重新注册）
        {
            var temp = new List<Object>();
            foreach (Object a in list)
            {
                if (SetID(a) == false) temp.Add(a);
            }
            return temp;
        }
        public void ForcedSetID(List<Object> list)//同上，但是会为已占用ID自动新ID（如已存在会删除旧ID重新注册）
        {
            var temp = SetID(list);
            foreach (Object a in temp)
            {
                ApplyID(a);
            }
        }
        public bool SetID(Object obj)//为obj注册为其内置的ID，如果ID不可被注册（即已被占用）返回假（如已存在会删除旧ID重新注册）
        {
            var temp = (IID)obj;
            return SetID(obj, temp.Id);
        }
        public bool SetID(Object obj, int id)//为obj注册为某ID，如果ID不可被注册（即已被占用）返回假（如已存在会删除旧ID重新注册）
        {
            if (IsExist(id)) return false;
            if (IsExist(obj))
            {
                Del(obj);
            }
            _objList.Add(obj);
            _idList.Add(id);
            _ChangeObjID(obj, id);
            return true;
        }
        public bool Del(Object obj)//删除某个对象，并把该对象id值改为-1，不存在返回假
        {
            int index = _GetIndex(obj);
            if (index == -1) return false;
            _objList.RemoveAt(index);
            _idList.RemoveAt(index);
            _ChangeObjID(obj, -1);
            return true;
        }
        public bool Del(int id)//删除某个ID的对象，并把该对象id值改为-1，不存在返回假
        {
            int index = _GetIndex(id);
            if (index == -1) return false;
            _ChangeObjID(_objList[index], -1);
            _objList.RemoveAt(index);
            _idList.RemoveAt(index);
            return true;
        }
        private int _GetIndex(Object obj)//查找某对象表中索引值，如无则返回-1
        {
            int index = -1;
            foreach (Object a in _objList)
            {
                index++;
                if (a.Equals(obj)) return index;
            }
            return -1;
        }
        private int _GetIndex(int id)//查找某ID表中索引值，如无则返回-1
        {
            int index = 0;
            foreach (int a in _idList)
            {
                index++;
                if (a == id) return index;
            }
            return -1;
        }
        public int GetID(Object obj)//查找某对象ID，如无则返回-1
        {
            int index = _GetIndex(obj);
            if (index == -1)
            {
                return -1;
            }
            else
            {
                return _idList[index];
            }
        }
        public bool IsExist(Object obj)//查找某对象是否在表中
        {
            foreach (Object a in _objList)
            {
                if (a.Equals(obj)) return true;
            }
            return false;
        }
        private bool _IsIndexExist(int index)//查找某索引值是否在表中
        {
            return _objList.Count <= index;
        }
        private void _ChangeObjID(Object obj, int _id)//更改输入obj的id值
        {
            var a = (IID)obj;
            a.Id = _id;
        }
        public bool IsExist(int id)//查找某ID是否在表中
        {
            foreach (int a in _idList)
            {
                if (a == id) return true;
            }
            return false;
        }
        public Object GetObj(int id)//由ID获取对象，如果没有会返回default(T)
        {
            int index = _GetIndex(id);
            if (index == -1) return null;
            return _objList[index];
        }
        public int GetNumber()//获取现存对象数
        {
            return _objList.Count;
        }
        public int GetCurrent()//获取刚要分发的ID（即上一个分发的ID）
        {
            return _currentID;
        }
        public int SkipID()//跳过这个ID
        {
            return ++_currentID;
        }
        public int SkipID(int n)//跳过数个ID
        {
            _currentID += n;
            return _currentID;
        }

    }
}
