using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using BottleJson;
using System.Text.RegularExpressions;

namespace JuicyLauncher2
{
    public partial class ShowTask : Form
    {
        public delegate LauncherReturn onTaskCompleteEventHandler(MCLaunchArgs a, YggDrasilAuth b, int c);
        public event EventHandler onTaskComplete;
        public event onTaskCompleteEventHandler onTaskCompleteA;
        public MCLaunchArgs mcla;
        public YggDrasilAuth yda;
        public int wtr;
        WebClient wc = new WebClient();
        TaskToDo[] tasks;
        int i = 0;
        public ShowTask(TaskToDo[] ta)
        {
            InitializeComponent();
            tasks = ta;
        }

        public void curcd(object sender, DownloadProgressChangedEventArgs e)
        {
            curPG.Step = e.ProgressPercentage - curPG.Value;
            curPG.PerformStep();
            cpgPct.Text = curPG.Value + "%";
        }

        public void beginTask()
        {
            this.Show();
            doTask();
        }
        private void doTask()
        {
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(curcd);
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DoNext);
            foreach (TaskToDo t in tasks)
            {
                switch (t.Type)
                {
                    case "Download":
                        tl.Items.Add("下载" + t.param.Split(";".ToCharArray())[0]);
                        break;
                    case "InstallForge":
                        tl.Items.Add("安装Forge:" + t.param.Split(";".ToCharArray())[0] + "到版本" + t.param.Split(";".ToCharArray())[1]);
                        break;
                    case "InstallLiteLoader":
                        tl.Items.Add("安装LiteLoader:" + t.param.Split(";".ToCharArray())[0] + "到版本" + t.param.Split(";".ToCharArray())[1]);
                        break;
                    case "InstallOptiFine":
                        tl.Items.Add("安装OptiFine:" + t.param.Split(";".ToCharArray())[0] + "到版本" + t.param.Split(";".ToCharArray())[1]);
                        break;
                }
            }
            tl.SelectedIndex = 0;
            curPG.Value = 0;
            cpgPct.Text = "0%";
            totPG.Value = 0;
            tpgPct.Text = "0%";
            DoNext(null, null);
        }

        public void DoNext(object sender, AsyncCompletedEventArgs e)
        {
            string cd = new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json")).getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\");
            if (i == tasks.Length)
            {
                wc.Dispose();
                if (onTaskComplete != null)
                {
                    onTaskComplete(null, null);
                }
                if (onTaskCompleteA != null)
                {
                    onTaskCompleteA(mcla, yda, wtr);
                }
                this.Close();
                return;
            }
            tl.SelectedIndex = i;
            if (tl.SelectedItem.ToString().StartsWith("下载"))
            {
                if (!Directory.GetParent(tasks[i].param.Split(";".ToCharArray())[1]).Exists)
                {
                    Directory.GetParent(tasks[i].param.Split(";".ToCharArray())[1]).Create();
                }
                wc.DownloadFileAsync(new Uri(tasks[i].param.Split(";".ToCharArray())[0]), tasks[i].param.Split(";".ToCharArray())[1]);
            }
            if (tl.SelectedItem.ToString().StartsWith("安装Forge"))
            {
                Unzipper.decompress(tasks[i].param.Split(";".ToCharArray())[0], Application.StartupPath + "\\ftmp");
                JSONObject ipf = new JSONObject(File.ReadAllText(Application.StartupPath + "\\ftmp\\install_profile.json"));
                string vername = ipf.getJSONObject("install").getString("target");
                string installPathBase = cd + "\\versions\\" + vername + "\\" + vername;
                if (!Directory.GetParent(installPathBase).Exists)
                {
                    Directory.GetParent(installPathBase).Create();
                }
                string dist = ipf.getJSONObject("install").getString("path");
                String[] libname = dist.Split(":".ToCharArray());
                string dp = libname[1] + "-" + libname[2];
                dist = cd + "\\libraries\\" + libname[0].Replace(".", "\\") + "\\" + libname[1] + "\\" + libname[2] + "\\" + dp + ".jar";
                if (!Directory.GetParent(dist).Exists)
                {
                    Directory.GetParent(dist).Create();
                }
                if (!ipf.getJSONObject("versionInfo").Contains("inheritsFrom"))
                {
                    File.Copy(cd + "\\versions\\" + tasks[i].param.Split(";".ToCharArray())[1] + "\\" + tasks[i].param.Split(";".ToCharArray())[1] + ".jar", installPathBase + ".jar");
                }
                File.WriteAllText(installPathBase + ".json", ipf.getJSONObject("versionInfo").toString());
                File.Copy(Application.StartupPath + "\\ftmp\\" + dp + "-universal.jar", dist, true);
                i++;
                DoNext(null, null);
                return;
            }
            if (tl.SelectedItem.ToString().StartsWith("安装LiteLoader"))
            {
                string sv = tasks[i].param.Split(";".ToCharArray())[1] + "-";
                sv = sv.Substring(0, sv.IndexOf("-"));
                JSONObject jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\lvl.json")).getJSONObject("versions").getJSONObject(sv);
                JSONObject sjo = new JSONObject();
                if (jo.Contains("artefacts"))
                {
                    JSONObject stable = jo.getJSONObject("artefacts").getJSONObject("com.mumfrey:liteloader");
                    var keys = stable.getKeys();
                    foreach (string key in keys)
                    {
                        if (key == "latest")
                        {
                            continue;
                        }
                        if (stable.getJSONObject(key).getString("version") == tasks[i].param.Split(";".ToCharArray())[0])
                        {
                            sjo = stable.getJSONObject(key);
                        }
                    }
                }
                if (jo.Contains("snapshots"))
                {
                    JSONObject stable = jo.getJSONObject("snapshots").getJSONObject("com.mumfrey:liteloader");
                    var keys = stable.getKeys();
                    foreach (string key in keys)
                    {
                        if (key == "latest")
                        {
                            continue;
                        }
                        if (stable.getJSONObject(key).getString("version") == tasks[i].param.Split(";".ToCharArray())[0])
                        {
                            sjo = stable.getJSONObject(key);
                        }
                    }
                }
                if (!Directory.Exists(cd + "\\libraries\\com\\mumfrey\\liteloader\\" + sv))
                {
                    Directory.CreateDirectory(cd + "\\libraries\\com\\mumfrey\\liteloader\\" + sv);
                }
                if (!File.Exists(Application.StartupPath + "\\liteloader-" + tasks[i].param.Split(";".ToCharArray())[0] + ".jar"))
                {
                    i++;
                    DoNext(null, null);
                    return;
                }
                File.Copy(Application.StartupPath + "\\liteloader-" + tasks[i].param.Split(";".ToCharArray())[0] + ".jar", cd + "\\libraries\\com\\mumfrey\\liteloader\\" + sv + "\\liteloader-" + sv + ".jar", true);
                JSONObject verInfo = new JSONObject(File.ReadAllText(cd + "\\versions\\" + tasks[i].param.Split(";".ToCharArray())[1] + "\\" + tasks[i].param.Split(";".ToCharArray())[1] + ".json"));
                var vl = sjo.getJSONArray("libraries");
                vl.putJSONObject(vl.length(), new JSONObject("{\"name\": \"com.mumfrey:liteloader:" + sv + "\",\"url\": \"http://dl.liteloader.com/versions/\"}"));
                verInfo.putJSONArray("libraries", vl);
                verInfo.putString("inheritsFrom", tasks[i].param.Split(";".ToCharArray())[1]);
                verInfo.putString("jar", sv);
                verInfo.putString("mainClass", "net.minecraft.launchwrapper.Launch");
                verInfo.putString("minecraftArguments", verInfo.getString("minecraftArguments") + " --tweakClass " + sjo.getString("tweakClass"));
                string dist = cd + "\\versions\\" + tasks[i].param.Split(";".ToCharArray())[1] + "-Liteloader" + tasks[i].param.Split(";".ToCharArray())[0] + "\\" + tasks[i].param.Split(";".ToCharArray())[1] + "-Liteloader" + tasks[i].param.Split(";".ToCharArray())[0] + ".json";
                if (!Directory.GetParent(dist).Exists)
                {
                    Directory.GetParent(dist).Create();
                }
                File.WriteAllText(dist, verInfo.toString());
                i++;
                DoNext(null, null);
                return;
            }
            if (tl.SelectedItem.ToString().StartsWith("安装OptiFine"))
            {
                string sv = tasks[i].param.Split(";".ToCharArray())[1] + "-";
                sv = sv.Substring(0, sv.IndexOf("-"));
                string ov = tasks[i].param.Split(";".ToCharArray())[0];
                string targ = tasks[i].param.Split(";".ToCharArray())[1];
                if (!Directory.Exists(cd + "\\libraries\\optifine\\OptiFine\\" + sv + "_" + ov))
                {
                    Directory.CreateDirectory(cd + "\\libraries\\optifine\\OptiFine\\" + sv + "_" + ov);
                }
                File.Copy(cd + "\\mods\\Optifine_" + sv + "_" + ov + ".jar", cd + "\\libraries\\optifine\\OptiFine\\" + sv + "_" + ov + "\\OptiFine-" + sv + "_" + ov + ".jar", true);
                JSONObject verInfo = new JSONObject(File.ReadAllText(cd + "\\versions\\" + tasks[i].param.Split(";".ToCharArray())[1] + "\\" + tasks[i].param.Split(";".ToCharArray())[1] + ".json"));
                var vl = new JSONArray();
                vl.putJSONObject(0, new JSONObject("{      \"name\": \"optifine:OptiFine:" + sv + "_" + ov + "\"    }"));
                vl.putJSONObject(1, new JSONObject("{\"name\": \"net.minecraft:launchwrapper:1.12\"}"));
                verInfo.putJSONArray("libraries", vl);
                verInfo.putString("inheritsFrom", targ);
                verInfo.putString("jar", sv);
                verInfo.putString("mainClass", "net.minecraft.launchwrapper.Launch");
                verInfo.putString("minecraftArguments", verInfo.getString("minecraftArguments") + " --tweakClass optifine.OptiFineTweaker");
                string dist = cd + "\\versions\\" + targ + "-OptiFine_" + ov + "\\" + targ + "-OptiFine_" + ov + ".json";
                if (!Directory.GetParent(dist).Exists)
                {
                    Directory.GetParent(dist).Create();
                }
                File.WriteAllText(dist, verInfo.toString());
                i++;
                DoNext(null, null);
                return;
            }
            totPG.Step = i * 100 / tl.Items.Count - totPG.Value;
            totPG.PerformStep();
            tpgPct.Text = totPG.Value + "%";
            i++;
        }
    }

    public class TaskToDo
    {
        public TaskToDo()
        {

        }
        public string Type;
        public string param;
    }
}
