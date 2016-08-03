﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetWork;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace TEST
{
    class Program
    {
        //[DllImport("Connect.dll", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        //private extern static void _Enc7481_Set_Encoder(UInt16 intDirection, Int32 lngValue);
        static void Main(string[] args)
        {
            //Error u = new PwdError();
            //var l = new LoginR(new Login("haha", "1234567890"), u);
            //string s = JsonHelper.GetJson(l);
            //Log(s);
            //var o = JsonHelper.ParseFromJson<LoginR>(s);
            //Log(JsonHelper.GetJson(o));

            Log("Community:");
            Community c = new Community();
            Log(c.ToJson());

            Log("Notice:");
            c = new Notice();
            Log(c.ToJson());

            Log("Enquire:");
            c = new Enquire();
            Log(c.ToJson());

            Log("Respond:");
            c = new Respond((Enquire)c);
            Log(c.ToJson());

            Log("Ack:");
            c = new Ack(c);
            Log(c.ToJson());

            Console.WriteLine("\n------Finsh------");
            Console.ReadKey();
            Console.ReadKey();
        }
        public static void Log(object s)
        {
            Console.WriteLine(s);
        }

        
        public static void TheDiffenceBetweenNewAndVirtual()
        {
            A a = new A();
            A b = new A1();
            A c = new A2();
            A2 d = new A2();
            a.a1();
            b.a1();
            c.a1();
            d.a1();
        }
        public static void JsonTest()//测试json的一系列语句
        {
            ////Girl g = new Girl();
            ////g.Name = "Alice";
            //////People p = (People)g;
            ////People p = new People();
            ////Console.WriteLine("g:{0},P:{1}", g.GetType(), p.GetType());
            ////g.relates[0].ID = 8888888;
            ////string gjson = JsonHelper.GetJson(g);
            ////string pjson = JsonHelper.GetJson(p);
            ////Console.WriteLine(gjson);
            ////Console.WriteLine(pjson);
            ////Console.WriteLine("\n-----------------------------------");
            ////Girl gobj = JsonHelper.ParseFromJson<Girl>(gjson);
            ////Object pobj = JsonHelper.ParseFromJson<People>(pjson);
            //////Girl gobjg = (Girl)gobj;
            ////gobj.relates[0].ID = 111111;
            ////gjson = JsonHelper.GetJson(gobj);
            ////pjson = JsonHelper.GetJson(pobj);
            ////Console.WriteLine(gjson);
            ////Console.WriteLine(pjson);
            ////Console.WriteLine("\n-----------------------------------");
            //////gjson = JsonHelper.GetJson<Girl>(gobj);
            ////////pjson = JsonHelper.GetJson<People>(pobj);
            //////Console.WriteLine(gjson);
            //////Console.WriteLine(pjson);
            ////Console.WriteLine("g:{0},P:{1}", gobj.GetType(), pobj.GetType());

        }
       
    }
}
