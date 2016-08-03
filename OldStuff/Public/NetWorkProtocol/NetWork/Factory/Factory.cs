using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace NetWork
{
    /// <summary>
    /// 由json变为具体类的工厂
    /// </summary>
    public class JsonFactory
    {
        public List<Type> types=new List<Type>();
        public JsonFactory()
        {
            InitTypes();
        }
        /// <summary>
        /// 添加检测类型
        /// </summary>
        /// <param name="t"></param>
        public void AddType(Type t)
        {
            types.Add(t);
        }
        /// <summary>
        /// 初始检测类型
        /// </summary>
        public void InitTypes()
        {
            types = new List<Type> {

                typeof(Community),

                typeof(Notice),
                typeof(Enquire),
                typeof(Respond),
                typeof(Ack),
                

            };
        }


        /// <summary>
        /// 获取json代表的对象类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Type GetType(string json)
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

        /// <summary>
        /// 获取json的对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Community GetObj(string json)
        {
            Type t = GetType(json);
            return (Community)JsonHelper.ParseFromJsonWithType(json, t);
        }
        /// <summary>
        /// 检测A开头到某一部分是不是含有B。
        /// 如果含有且在开头部分则返回1，
        /// 如果A==B则返回2，
        /// 不含则为-1，
        /// 如果B的长度大于A返回-2，
        /// 包含但不在开头返回0
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        protected int Include(string A, string B)
        {
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
