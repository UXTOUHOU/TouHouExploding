using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetWork;
using System.Runtime.Serialization;

namespace TEST
{
    [DataContract]
    public class Girl : People
    {
        [DataMember]
        public string Sex = "Girl";
        //[DataMember]
        //private string Character = "Calm and Generals";
        [DataMember]
        public People Husband = new People();
        [DataMember]
        public List<People> relates;
        public Girl()
        {
            relates = new List<People>();
            People p1 = new People();
            People p2 = new People();
            relates.Add(p1);
            relates.Add(p1);
            relates.Add(p2);
           // Character = "Calm and Generals";
        }
    }
    [DataContract]
    public class People : CanTran
    {
        [DataMember]
        public string ClassName { get; set; }
        [DataMember]
        public bool IsAdult;
        [DataMember]
        public bool IsPeople = true;
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
        //[DataMember]
        //private string SecretName = "HIM";
        [DataMember]
        public int[] Position = new int[] { 1, 2, 3 };

        public void SetNickName(string name)
        {
            NickName = name;
        }
        public People()
        {
            ClassName = this.GetType().ToString();
            ID = 66666;
        }
        public enum Nationality { Chinese, English };
    }
    public interface CanTran
    {
        string ClassName { get; set; }
    }
}
