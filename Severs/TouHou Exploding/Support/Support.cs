using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using JsonFx.Json;
using System.Reflection;

namespace TouHou_Exploding
{
    public class IDProvider//用于分发临时ID，被分发对象需继承接口IID，未分配时为-1
    {
        //以下为动态ID分配
        public IDList CID { get; set; }//(CardID)-区别卡牌(召唤区少女也算卡牌)
        public IDList UID { get; set; }//(UnitID)-区别单位-由卡牌/技能召唤出的单位ID
        public IDList PID { get; set; }//(PlayerID)-区别玩家
        public IDList RID { get; set; }//(RegionID)-区别不同地区方块
        public IDList TID { get; set; }//(TeamID)-区别不同队
        public IDProvider()
        {
            CID = new IDList(typeof(Card));
            UID = new IDList(typeof(Unit));
            PID = new IDList(typeof(Player));
            RID = new IDList(typeof(Region));
            TID = new IDList(typeof(Team));
        }
        public interface IID
        {
            int id { get; set; }
        }
        public class IDList : IID//用于单项ID管理
        {
            public string name { get; set; }
            public int id { get; set; }//无指定ID时为-1
            private List<Object> _objList = new List<Object>();
            private List<int> _idList = new List<int>();
            private int _currentID = 0;
            public IDList (string _name)//初始化一个空list
            {
                name = _name;
                id = -1;
            }
            public IDList(List<Object> list)//由已有ID导入
            {
                name = list.GetType().ToString();
                id = -1;
                SetID(list);
            }
            public IDList(Type type)//由类型生成
            {
                name = type.ToString();
                id = -1;
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
                }while (IsExist(_currentID));
                _objList.Add(obj);
                _idList.Add(_currentID);
                _ChangeObjID (obj,_currentID);
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
                return SetID(obj, temp.id);
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
                int index = 0;
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
                int index = _GetIndex (obj);
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
                a.id = _id;
            }
            public bool IsExist(int id)//查找某ID是否在表中
            {
                foreach (int a in _idList)
                {
                    if (a==id) return true;
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
    public static class CommondHelper
    {
        public static void Commond(string commond)
        {
            ReadCommond(CommondHelper.Divide(commond));
        }
        public static void ReadCommond(string[] commond)
        {
            int length = commond.Length;

        }
        public static bool IsExist(string[] commond, int index)
        {
            return commond.Length > index;
        }
        public static string[] Divide(string commond)//用于分隔指令
        {
            string[] str = commond.Split (' ');
            return str;
        }
    }
    public class FileHelper//用于存储
    {
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                //fStream = new FileStream(path, FileMode.OpenOrCreate);//不注释掉此行会一直占用文件
            }
        }
        private string path;
        private FileStream fStream;
        private StreamWriter sw = null;
        private StreamReader sr = null;
        public FileHelper(string filePath)
        {
            Path = filePath;
        }
        public void ChangeFile(string filePath)
        {
            Path = filePath;
        }
        public bool IsExist()
        {
            return System.IO.File.Exists(Path);
        }
        public bool IsExist(string path)
        {
            Path = path;
            return IsExist();
        }
        public void WriteFile(string file)
        {
            try
            {
                if (sw == null)
                {
                    if (sr != null)
                    {
                        sr.Close();
                        sr = null;
                    }
                    if (System.IO.File.Exists(path))
                    {
                        fStream = new FileStream(path, FileMode.Truncate);
                    }
                    else
                    {
                        fStream = new FileStream(path, FileMode.OpenOrCreate);
                    }
                    sw = new StreamWriter(fStream);
                    
                }
                sw.Write(file);
                sw.Close();
                sw = null;
                fStream.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("An IOException has been thrown!\r\n" + ex.ToString());
                Console.WriteLine("An IOException has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return;
            }
        }
        public string ReadFile()//如果没有此文件返回null
        {
            string result;
            if (IsExist() == false) return null;
            try
            {
                if (sr == null)
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }

                    try
                    {
                        fStream = new FileStream(path, FileMode.Open);
                    }
                    catch (Exception IO)
                    {
                        Console.WriteLine("There is a IO Exception！\r\n{0}", IO.ToString());
                        return null;
                    }
                    sr = new StreamReader(fStream);
                }
                result = sr.ReadToEnd();
                sr.Close();
                sr = null;          
                fStream.Close();
                return result;
            }
            catch (IOException ex) 
            { 
                Console.WriteLine("An IOException has been thrown!"); 
                Console.WriteLine(ex.ToString()); 
                return null;
            }

        }
        public string ReadFile(string path)
        {
            Path = path;
            return ReadFile();
        }
        public void ObjToFile<T>(object obj)
        {
            string json = JsonHelper.GetJson<T>((T)obj);
            WriteFile(json);
        }
        public T FileToObj<T>()//如果没有此文件返回null
        {
            T result;
            var json = ReadFile();
            result = JsonHelper.ParseFromJson<T>(json);
            return result;
        }
        public T FileToObj<T>(string path)
        {
            Path = path;
            return FileToObj<T>();
        }
        public void Close()//释放文件
        {
            if (sw != null)
            {
                sw.Close();
                sw = null;
            }
            if (sr != null)
            {
                sr.Close();
                sr = null;
            }
            fStream.Close();
        }
    }

    //下面的程序是拷贝虫子菌的=。=
    public static class JsonHelper//欲序列化的对象应有[DataContract]属性,变量应带有[DataMember]
    {
        public static string GetJson<T>(T obj, IEnumerable<Type> knownTypes = null)//对象转json
        {
            var json = new DataContractJsonSerializer(obj.GetType(), knownTypes);

            using (var stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public static string GetJson(object obj, IEnumerable<Type> knownTypes = null)//对象转json（无泛型版）这个是自制的，不是虫子的这个是自制的，不是虫子的
        {
            var json = new DataContractJsonSerializer(obj.GetType(), knownTypes);

            using (var stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public static Dictionary<string, object> GetDictionaryFromJson(string JSON)
        {
            return (Dictionary<string, object>)new JavaScriptSerializer().DeserializeObject(JSON);
        }

        public static T ParseFromJson<T>(string JSON, IEnumerable<Type> knownTypes = null)//json转对象
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(JSON)))
            {
                return (T)new DataContractJsonSerializer(typeof(T), knownTypes).ReadObject(ms);
            }
        }
        public static object ParseFromJsonWithType(string JSON, Type t, IEnumerable<Type> knownTypes = null)//json转对象（使用Type作为依据）这个是自制的，不是虫子的
        {
            if (t != null)
            {
                //以下的代码就是曲折的实现这句伪代码（使用反射实现）
                //object obj = JsonHelper.ParseFromJson<t>(json);
                Type jh = typeof(JsonHelper);
                MethodInfo mi = jh.GetMethod("ParseFromJson").MakeGenericMethod(t);
                object obj = mi.Invoke(null, new object[] { JSON, null });

                return obj;
            }
            return null;
        }

    }
}
