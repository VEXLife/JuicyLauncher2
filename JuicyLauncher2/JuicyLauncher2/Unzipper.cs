using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace JuicyLauncher2
{
    public class Unzipper
    {
        public static void decompress(String inputFileName, String outputDirName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();//释放7z.exe和7z.dll部分
            Stream stream = assembly.GetManifestResourceStream("JuicyLauncher2.7z.exe");//释放7z.exe和7z.dll部分
            byte[] bytes = new byte[stream.Length];//释放7z.exe和7z.dll部分
            stream.Read(bytes, 0, int.Parse(stream.Length.ToString()));//释放7z.exe和7z.dll部分
            File.WriteAllBytes(Application.StartupPath + "\\7z.exe", bytes);//释放7z.exe和7z.dll部分
            assembly = Assembly.GetExecutingAssembly();//释放7z.exe和7z.dll部分
            stream = assembly.GetManifestResourceStream("JuicyLauncher2.7z.dll");//释放7z.exe和7z.dll部分
            bytes = new byte[stream.Length];//释放7z.exe和7z.dll部分
            stream.Read(bytes, 0, int.Parse(stream.Length.ToString()));//释放7z.exe和7z.dll部分
            File.WriteAllBytes(Application.StartupPath + "\\7z.dll", bytes); //释放7z.exe和7z.dll部分
            Process sz = new Process();//运行7z.exe解压部分
            ProcessStartInfo psi = new ProcessStartInfo(Application.StartupPath + "\\7z.exe", "x \"" + inputFileName + "\" -o\"" + outputDirName + "\" -y");//运行7z.exe解压部分
            psi.UseShellExecute = false;//运行7z.exe解压部分
            psi.WindowStyle = ProcessWindowStyle.Hidden;//设置不显示
            psi.CreateNoWindow = true;//设置不显示
            sz.StartInfo = psi;//运行7z.exe解压部分
            sz.Start();//运行7z.exe解压部分
            sz.WaitForExit();//等待退出
            File.Delete(Application.StartupPath + "\\7z.exe");//删除7z.exe
            File.Delete(Application.StartupPath + "\\7z.dll");//删除7z.dll
        }
    }
}
