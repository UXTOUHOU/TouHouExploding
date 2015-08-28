using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//空端口


namespace NetWork_Client
{
    public class Client
    {
        public string SeverHost { get; set; }//用于输入服务器地址
        public int SeverPort { get; set; }//用于输入服务器端口号
        public Client()
        {
            SeverHost = "";//自设服务器
            SeverPort = 00000;//自设端口号
        }
        public bool Login(string player,string password)
        {
            return true;
        }
        public class Sender
        {

        }
    }
}
