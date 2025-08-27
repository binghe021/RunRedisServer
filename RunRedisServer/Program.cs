using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RunRedisServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Redis服务运行助手V1.1");
            Console.WriteLine("冰河之刃 渡桥计划");
            Console.WriteLine("博客：https://www.cnblogs.com/binghe021");
            Console.WriteLine("运行效果等同于命令行: redis-server.exe redis.windows-service.conf");
            Console.WriteLine("允许传入1个conf文件名参数。若有传参数，则使用传入conf文件名参数替代默认值。");
            Console.WriteLine("添加计划任务的时候，在参数文本框中加上conf文件名参数的值，exe选本程序即可实现调用自定义conf文件的功能。");
            Console.WriteLine("可以添加多个计划任务，调用不同的conf文件实现启动多个不同端口的redis服务实例");
            Console.WriteLine("当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));


            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string exeFileName = "redis-server.exe";
            string confFileName = "redis.windows-service.conf";

            if (args.Length == 1)
            {
                Console.WriteLine("传入conf文件名参数：" + args[0]);

                if (string.IsNullOrEmpty(args[0]))
                {
                    Console.WriteLine("传入conf文件名参数为空，程序终止。");//不一定有机会执行到
                    return;
                }


                Console.WriteLine("使用传入conf文件名参数替代默认值：" + confFileName);
                confFileName = args[0];
            }
            else
            {
                Console.WriteLine("无传入conf文件名参数");
            }

            Console.WriteLine("当前文件夹：" + baseDirectory);
            Console.WriteLine("exe文件名：" + exeFileName);
            Console.WriteLine("conf文件名：" + confFileName);

            string exeFile = Path.Combine(baseDirectory, exeFileName);
            string confFile = Path.Combine(baseDirectory, confFileName);


            if (!File.Exists(exeFile))
            {

                Console.WriteLine("未发现" + exeFileName + "文件，无法继续运行。");
                return;
            }
            if (!File.Exists(confFile))
            {

                Console.WriteLine("未发现" + confFileName + "文件，无法继续运行。");
                return;
            }




            //没有传入参数，按默认流程走。
            Process[] processes = Process.GetProcessesByName("redis-server");
            if (processes.Length > 0)
            {
                Console.WriteLine("发现正在运行的redis-server进程");
                Console.WriteLine("不再重复启动Redis服务");
                //return;

                //结束进程，倒着循环。
                //for (int i = processes.Length; i >0 ; i--)
                //{
                //    processes[i].Kill();
                //}
            }
            else
            {
                Console.WriteLine("未发现正在运行的redis-server进程");

                if (args.Length == 1)
                {
                    Console.WriteLine("有传入参数");
                }
                else
                {
                    Console.WriteLine("无传入参数");
                }

                RunRedis(baseDirectory, exeFileName, confFileName);

            }

            Console.WriteLine("运行完成");

        }

        private static void RunRedis(string baseDirectory, string fileName, string arguments)
        {
            Console.WriteLine("启动Redis服务开始");

            ProcessStartInfo processStartInfo = new ProcessStartInfo();

            processStartInfo.WorkingDirectory = baseDirectory;// AppDomain.CurrentDomain.BaseDirectory;// 本程序最重要的参数
            processStartInfo.FileName = fileName;// "redis-server.exe";//
            processStartInfo.Arguments = arguments;//"redis.windows-service.conf";//
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;//模拟Windows服务的形式 隐藏运行

            Process.Start(processStartInfo);

            Console.WriteLine("启动Redis服务完成");
        }
    }
}
