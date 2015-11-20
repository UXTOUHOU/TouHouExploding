using System;
using System.IO;

namespace THE_Core.Support
{
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
                //MessageBox.Show("An IOException has been thrown!\r\n" + ex.ToString());
                //Console.WriteLine("An IOException has been thrown!");
                //Console.WriteLine(ex.ToString());
                //Console.ReadLine();
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
}
