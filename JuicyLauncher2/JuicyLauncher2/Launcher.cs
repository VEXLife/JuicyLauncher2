using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BottleJson;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;

namespace JuicyLauncher2
{
    public class LauncherReturn
    {
        public String rtv;
        public Process MCMain;
        public string[] rtvs;
    }

    public class MCLaunchArgs
    {
        public short MaxMem;
        public string JrePath;
        public string ExtraMCArgs;
        public string ExtraJreArgs;
        public string Nick;
        public string VerName;
        public string CurrentDir;
    }

    public class YggDrasilAuth
    {
        public string Nick = "";
        public bool Yggauth;
        public string UserName;
        public string Password;
        public string uuid;
        public string accessToken;
        public string session_id;
        public string UserType;
        public JSONObject userProperties = new JSONObject();
        public YggDrasilAuth()
        {

        }

        public void Login()
        {
            if (Yggauth == false)
            {
                UserType = "Legacy";
            }
            else
            {
                try
                {
                    UserType = "Mojang";
                    WebRequest wr = WebRequest.Create("https://authserver.mojang.com/authenticate");
                    wr.ContentType = "application/json";
                    wr.Method = "POST";
                    StreamWriter sw = new StreamWriter(wr.GetRequestStream());
                    JSONObject ua = new JSONObject();
                    ua.putJSONObject("agent", new JSONObject("{\"name\": \"Minecraft\",\"version\": 1}"));
                    ua.putString("username", UserName);
                    ua.putString("password", new Encryptor().EncryptStr(Password, true));
                    ua.putString("clientToken", uuid);
                    ua.putObject("requestUser", "true");
                    sw.Write(ua.toString());
                    sw.Flush();
                    WebResponse rp = null;
                    try
                    {
                        rp = wr.GetResponse();
                    }
                    catch (WebException we)
                    {
                        wr = WebRequest.Create("https://authserver.mojang.com/signout");
                        wr.ContentType = "application/json";
                        wr.Method = "POST";
                        sw = new StreamWriter(wr.GetRequestStream());
                        ua = new JSONObject();
                        ua.putString("username", UserName);
                        ua.putString("password", new Encryptor().EncryptStr(Password, true));
                        sw.Write(ua.toString());
                        sw.Flush();
                        Login();
                        return;
                    }
                    StreamReader sr = new StreamReader(rp.GetResponseStream());
                    ua = new JSONObject(sr.ReadToEnd());
                    uuid = ua.getJSONObject("selectedProfile").getString("id");
                    Nick = ua.getJSONObject("selectedProfile").getString("name");
                    if (ua.getJSONObject("selectedProfile").Contains("legacy"))
                    {
                        UserType = "Legacy";
                    }
                    accessToken = ua.getString("accessToken");
                    userProperties = ua.getJSONObject("user");
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("登录失败！可能是网络不佳或用户名、密码不正确。是否重试？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Login();
                    }
                }
            }
        }
    }

    public class Launcher
    {
        public LauncherReturn LaunchMC(MCLaunchArgs argument, YggDrasilAuth auth, int whatToReturn = 0)
        {
            auth.Login();
            LauncherReturn lr = new LauncherReturn();
            JSONObject jo = new JSONObject(File.ReadAllText(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions\\" + argument.VerName + "\\" + argument.VerName + ".json"));
            String Libcmd = "";
            List<JSONObject> libs = new List<JSONObject>();
            libs.AddRange(jo.getJSONArray("libraries").toJSONObjects());
            JSONObject ji = jo;
            string jar = argument.VerName;
            #region 处理继承
            if (jo.Contains("inheritsFrom"))
            {
                jar = jo.getString("jar");
                ji = new JSONObject(File.ReadAllText(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions\\" + jo.getString("inheritsFrom") + "\\" + jo.getString("inheritsFrom") + ".json"));
                libs.AddRange(ji.getJSONArray("libraries").toJSONObjects());
                if (ji.Contains("inheritsFrom"))
                {
                    ji = new JSONObject(File.ReadAllText(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions\\" + ji.getString("inheritsFrom") + "\\" + ji.getString("inheritsFrom") + ".json"));
                    libs.AddRange(ji.getJSONArray("libraries").toJSONObjects());
                    if (ji.Contains("inheritsFrom"))
                    {
                        ji = new JSONObject(File.ReadAllText(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions\\" + ji.getString("inheritsFrom") + "\\" + ji.getString("inheritsFrom") + ".json"));
                        libs.AddRange(ji.getJSONArray("libraries").toJSONObjects());
                    }
                }
            }
            #endregion
            #region 处理库
            String lnsuffix = "";
            List<TaskToDo> tl = new List<TaskToDo>();
            foreach (JSONObject libdata in libs)
            {
                String[] libname = libdata.getString("name").Split(":".ToCharArray());
                if (libdata.Contains("natives") && libdata.getJSONObject("natives").Contains("windows"))
                {
                    lnsuffix = "-" + libdata.getJSONObject("natives").getString("windows");
                    lnsuffix = lnsuffix.Replace("${arch}", "32");
                }
                String libpath = argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\libraries\\" + libname[0].Replace(".", "\\") + "\\" + libname[1] + "\\" + libname[2] + "\\" + libname[1] + "-" + libname[2] + lnsuffix + ".jar";
                if (Libcmd.Contains(libpath))
                {
                    continue;
                }
                if (File.Exists(libpath))
                {
                    if (libdata.Contains("extract"))
                    {
                        Unzipper.decompress(libpath, argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\versions\\" + argument.VerName + "\\" + argument.VerName + "-natives");
                    }
                    else
                    {
                        Libcmd = Libcmd + libpath + ";";
                    }
                }
                else
                {
                    TaskToDo ttd = new TaskToDo();
                    ttd.Type = "Download";
                    ttd.param = "http://bmclapi2.bangbang93.com/libraries/" + libname[0].Replace(".", "/") + "/" + libname[1] + "/" + libname[2] + "/" + libname[1] + "-" + libname[2] + lnsuffix + ".jar;" + libpath;
                    tl.Add(ttd);
                }
                lnsuffix = "";
            }
            #endregion
            #region 处理资源文件
            bool virt = false;
            if (jo.Contains("assets") && !jo.Contains("assetIndex"))
            {
                var I = new JSONObject();
                I.putString("id", jo.getString("assets"));
                jo.putJSONObject("assetIndex", I);
                ji = jo;
            }
            if (!File.Exists(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\assets\\indexes\\" + ji.getJSONObject("assetIndex").getString("id") + ".json"))
            {
                TaskToDo tt = new TaskToDo();
                tt.Type = "Download";
                tt.param = ji.getJSONObject("assetIndex").getString("url") + ";" + argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\assets\\indexes\\" + ji.getJSONObject("assetIndex").getString("id") + ".json";
                tl.Add(tt);
            }
            else
            {
                JSONObject joa = new JSONObject(File.ReadAllText(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\assets\\indexes\\" + ji.getJSONObject("assetIndex").getString("id") + ".json"));
                List<string> files = joa.getJSONObject("objects").getKeys();
                List<string> hashes = new List<string>();
                virt = joa.Contains("virtual");
                JSONObject job = joa.getJSONObject("objects");
                foreach (string keyf in files)
                {
                    hashes.Add(job.getJSONObject(keyf).getString("hash"));
                }
                bool lackasset = false;
                string fp, nfp;
                nfp = argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\assets\\virtual\\" + ji.getJSONObject("assetIndex").getString("id");
                for (int j = 0; j < files.Count; j++)
                {
                    fp = argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\assets\\objects\\" + hashes[j].Substring(0, 2) + "\\" + hashes[j];
                    if (virt)
                    {
                        if (!File.Exists(nfp + "\\" + files[j].Replace("/", "\\")))
                        {
                            lackasset = true;
                            break;
                        }
                    }
                    else
                    {
                        if (!File.Exists(fp))
                        {
                            lackasset = true;
                            break;
                        }
                    }
                }
                if (lackasset)
                {
                    if (MessageBox.Show("资源文件不完整，是否补全？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        for (int j = 0; j < files.Count; j++)
                        {
                            if (virt)
                            {
                                if (!File.Exists(nfp + "\\" + files[j].Replace("/", "\\")))
                                {
                                    TaskToDo to = new TaskToDo();
                                    to.Type = "Download";
                                    to.param = "http://bmclapi2.bangbang93.com/assets/" + hashes[j].Substring(0, 2) + "/" + hashes[j] + ";" + nfp + "\\" + files[j].Replace("/", "\\");
                                    tl.Add(to);
                                }
                            }
                            else
                            {
                                fp = argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\assets\\objects\\" + hashes[j].Substring(0, 2) + "\\" + hashes[j];
                                if (!File.Exists(fp))
                                {
                                    TaskToDo to = new TaskToDo();
                                    to.Type = "Download";
                                    to.param = "http://bmclapi2.bangbang93.com/assets/" + hashes[j].Substring(0, 2) + "/" + hashes[j] + ";" + fp;
                                    tl.Add(to);
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            if (tl.Count != 0)
            {
                ShowTask st = new ShowTask(tl.ToArray());
                st.yda = auth;
                st.mcla = argument;
                st.wtr = whatToReturn;
                if (whatToReturn == 0)
                {
                    st.onTaskCompleteA += new ShowTask.onTaskCompleteEventHandler(LaunchMC);
                }
                st.beginTask();
                lr.rtv = "缺少文件，正在下载。";
                return lr;
            }
            try
            {
                Directory.Delete(argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\") + "\\versions\\" + argument.VerName + "\\" + argument.VerName + "-natives\\META-INF", true);
            }
            catch (DirectoryNotFoundException ex)
            {

            }
            Libcmd = Libcmd + argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions\\" + jar + "\\" + jar + ".jar";
            String RunCommand;
            if (auth.Nick != "")
            {
                argument.Nick = auth.Nick;
            }
            String minecraftArguments = "";
            if (jo.Contains("minecraftArguments"))
            {
                minecraftArguments = jo.getString("minecraftArguments");
            }
            else if (jo.Contains("arguments"))
            {
                JSONObject ag = jo.getJSONObject("arguments");
                var gmja = ag.getJSONArray("game");
                var gl = gmja.length();
                for (int l = 0; l < gl; l++)
                {
                    if (gmja.getString(l).Contains(","))
                    {
                        continue;
                    }
                    minecraftArguments = minecraftArguments + gmja.getString(l).Replace("\n", "").Replace(" ", "") + " ";
                }
                minecraftArguments = minecraftArguments.Substring(0, minecraftArguments.Length - 1);
            }
            minecraftArguments = minecraftArguments.Replace("${auth_player_name}", argument.Nick).Replace("${version_name}", "JuicyLauncher_2").Replace("${game_directory}", "\"" + argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\"").Replace("${assets_index_name}", ji.getJSONObject("assetIndex").getString("id")).Replace("${auth_uuid}", auth.uuid).Replace("${auth_access_token}", auth.accessToken).Replace("${user_type}", auth.UserType).Replace("${version_type}", "JuicyLauncher_2").Replace("${user_properties}", auth.userProperties.toString()).Replace("${game_assets}", "${assets_root}");

            if (virt == true)
            {
                minecraftArguments = minecraftArguments.Replace("${assets_root}", "\"" + argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\assets\\virtual\\" + ji.getJSONObject("assetIndex").getString("id") + "\"");
            }
            else
            {
                minecraftArguments = minecraftArguments.Replace("${assets_root}", "\"" + argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\assets\"");
            }
            RunCommand = "-Xmx" + argument.MaxMem.ToString() + "m -Djava.library.path=\"" + argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions\\" + argument.VerName + "\\" + argument.VerName + "-natives\" " + argument.ExtraJreArgs + " -cp \"" + Libcmd + "\" " + jo.getString("mainClass") + " " + minecraftArguments + " " + argument.ExtraMCArgs;
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo(argument.JrePath, RunCommand);
            psi.WorkingDirectory = argument.CurrentDir.Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/");
            psi.UseShellExecute = false;
            p.StartInfo = psi;
            if (whatToReturn == 1)
            {
                lr.MCMain = p;
                return lr;
            }
            p.Start();
            Application.Exit();
            return lr;
        }
    }
}