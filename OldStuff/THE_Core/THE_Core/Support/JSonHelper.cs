using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using JsonFx.Json;
using System.IO;
using System.Reflection;

namespace THE_Core.Support
{
    //下面的程序是拷贝虫子菌的=。=
    /// <summary>
    /// 欲序列化的对象应有[DataContract]属性,变量应带有[DataMember]
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 对象转json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="knownTypes"></param>
        /// <returns></returns>
        public static string GetJson<T>(T obj, IEnumerable<Type> knownTypes = null)
        {
            var json = new DataContractJsonSerializer(obj.GetType(), knownTypes);

            using (var stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        /// <summary>
        /// 对象转json（无泛型版）这个是自制的，不是虫子的这个是自制的，不是虫子的
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="knownTypes"></param>
        /// <returns></returns>
        public static string GetJson(object obj, IEnumerable<Type> knownTypes = null)
        {
            var json = new DataContractJsonSerializer(obj.GetType(), knownTypes);

            using (var stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        /// <summary>
        /// json转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JSON"></param>
        /// <param name="knownTypes"></param>
        /// <returns></returns>
        public static T ParseFromJson<T>(string JSON, IEnumerable<Type> knownTypes = null)
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
