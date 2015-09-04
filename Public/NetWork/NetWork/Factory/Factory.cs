using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace NetWork
{
    public class JsonFactory//由json变为具体类的工厂
    {
        public List<Type> types=new List<Type>();
        public JsonFactory()
        {
            InitTypes();
        }
        public void AddType(Type t)//添加检测类型
        {
            types.Add(t);
        }
        public void InitTypes()//初始检测类型
        {
            types = new List<Type> {

                typeof(Community),

                typeof(Notice),
                typeof(Enquire),
                typeof(Respond),
                typeof(Ack),
                

            };
        }


        public Type GetType(string json)//获取json代表的对象类型
        {
            Community objC = JsonHelper.ParseFromJson<Community>(json);
            foreach (Type t in types)
            {
                if (objC.ClassName == t.ToString())
                {
                    return t;
                }
            }
            return null;
        }

        public Community GetObj(string json)//获取json的对象
        {
            Type t = GetType(json);
            return (Community)JsonHelper.ParseFromJsonWithType(json, t);
        }
        protected int Include(string A, string B)
        {
            /*
             *检测A开头到某一部分是不是含有B。
             *如果含有且在开头部分则返回1，
             *如果A==B则返回2，
             *不含则为-1，
             *如果B的长度大于A返回-2，
             *包含但不在开头返回0 
             */
            if (A.Length < B.Length)
            {
                return -2;
            }else if (A.Substring(0,B.Length)==B)
            {
                if(A==B)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }else if (A.Contains(B))
            {
                return 0;
            }
            else
            {
                return -1;
            }
            
        }
    }
}
