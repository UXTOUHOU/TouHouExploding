using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetWork;
using System.Runtime.Serialization;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            Girl g = new Girl();
            g.Name = "Alice";
            //People p = (People)g;
            People p = new People();
            Console.WriteLine("g:{0},P:{1}",g.GetType (),p.GetType());
            string gjson = JsonHelper.GetJson(g);
            string pjson = JsonHelper.GetJson(p);
            Console.WriteLine(gjson);
            Console.WriteLine(pjson);
            Console.WriteLine("\n-----------------------------------");
            Object gobj = JsonHelper.ParseFromJson(gjson);
            Object pobj = JsonHelper.ParseFromJson(pjson);
            gjson = JsonHelper.GetJson(g);
            pjson = JsonHelper.GetJson(p);
            Console.WriteLine(gjson);
            Console.WriteLine(pjson);
            Console.WriteLine("\n------Finsh------");
            Console.Read();
        }
        [DataContract]
        public class Girl : People
        {
            [DataMember]
            public string Sex = "Girl";
            [DataMember]
            private string Character = "Calm and Generals";
            [DataMember]
            public People Husband = new People();
        }
        [DataContract]
        public class People
        {
            [DataMember]
            public bool IsAdult;
            [DataMember]
            public bool IsPeople=true;
            [DataMember]
            public int ID = 10000;
            [DataMember]
            public string Name;
            [DataMember]
            public int Age { get; set; }
            [DataMember]
            public Nationality Nation = Nationality.Chinese;
            [DataMember]
            private string NickName;
            [DataMember]
            private string SecretName="HIM";
            [DataMember]
            public List<People> relates;
            public void SetNickName(string name)
            {
                NickName = name;
            }
            public enum Nationality { Chinese, English };
        }
    }
}
