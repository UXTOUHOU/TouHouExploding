﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using JsonFx.Json;

namespace TouHou_Exploding
{
    public class IDProvider//用于分发临时ID，被分发对象需继承接口IID，未分配时为-1
    {

        public interface IID
        {
            int id { get; set; }
        }
        class IDList<T> : IID//用于单项ID管理 T最好别是int
        {
            public string name { get; set; }
            public int id { get; set; }//无指定ID时为-1
            private List<T> _objList = new List<T>();
            private List<int> _idList = new List<int>();
            private int _currentID = 0;
            public IDList (string _name)
            {
                name = _name;
                id = -1;
            }
            public IDList (T obj)//输入的obj需要集成IID接口
            {
                name = obj.GetType().ToString();
                ApplyID(obj);
            }
            public int ApplyID(T obj)//输入的obj需要集成IID接口，如已存在输入-1
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
            public bool SetID(T obj, int id)//为obj注册为某ID
            {
                if (IsExist(id)) return false;
                _objList.Add(obj);
                _idList.Add(id);
                _ChangeObjID(obj, id);
                return true;
            }
            public bool Del(T obj)//删除某个对象，并把该对象id值改为-1，不存在返回假
            {
                int index = _GetIndex(obj);
                if (index == -1) return false;
                _objList.RemoveAt(index);
                _idList.RemoveAt(index);
                _ChangeObjID(obj, -1);
                return true;
            }
            private int _GetIndex(T obj)//查找某对象表中索引值，如无则返回-1
            {
                int index = 0;
                foreach (T a in _objList)
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
            public int GetID(T obj)//查找某对象ID，如无则返回-1
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
            public bool IsExist(T obj)//查找某对象是否在表中
            {
                foreach (T a in _objList)
                {
                    if (a.Equals(obj)) return true;
                }
                return false;
            }
            private bool _IsIndexExist(int index)//查找某索引值是否在表中
            {
                return _objList.Count <= index;
            }
            private void _ChangeObjID(T obj,int _id)//更改输入obj的id值
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
            public T GetObj(int id)//由ID获取对象，如果没有会返回default(T)
            {
                int index = _GetIndex(id);
                if (index == -1) return default(T);
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
    }
}
