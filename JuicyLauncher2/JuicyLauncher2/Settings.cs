using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BottleJson;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace JuicyLauncher2
{
    public partial class Settings : Form
    {
        delegate void UpdgOCallBack(string str);
        WebClient wc = new WebClient();
        JSONObject jo = new JSONObject();
        JSONObject[] vers;
        string cd = "";
        List<string> Passwords = new List<string>();
        bool loading = false;
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveChange();
            new MainForm().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "Java主程序（java.exe,javaw.exe）|java.exe;javaw.exe";
            ofd.Multiselect = false;
            ofd.Title = "选择Jre主程序文件";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                JrePath.Text = ofd.FileName;
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            loading = true;
            jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json"));
            JSONObject mca = jo.getJSONObject("MCLSettings");
            JSONArray ya = jo.getJSONArray("YggDrasilLogin");
            MaxMem.Value = decimal.Parse(mca.getString("MaxMemorySize"));
            Nick.Text = jo.getString("Nick");
            JSONObject[] yas = ya.toJSONObjects();
            JSONObject yt = new JSONObject();
            foreach (JSONObject yd in yas)
            {
                var v = yd.getObject("Selected");
                if (yd.getObject("Selected") == "True")
                {
                    yt = yd;
                }
                if (yd.getString("UserName") != "")
                {
                    UserName.Items.Add(yd.getString("UserName"));
                    Passwords.Add(new Encryptor().EncryptStr(yd.getString("Password"),true));
                }
            }
            UserName.Text = yt.getString("UserName");
            Password.Text = new Encryptor().EncryptStr(yt.getString("Password"),true);
            WindowWidth.Value = decimal.Parse(mca.getString("WindowWidth"));
            WindowHeight.Value = decimal.Parse(mca.getString("WindowHeight"));
            JrePath.Text = mca.getString("JrePath");
            ServerIp.Text = mca.getString("ServerIp");
            ExtraJre.Text = mca.getString("ExtraJreArgs");
            ExtraMC.Text = mca.getString("ExtraMCArgs");
            GamePath.Text = jo.getString("CurrentDir");
            doServerC.Checked = bool.Parse(mca.getObject("doServerC"));
            FullScreen.Checked = bool.Parse(mca.getObject("FullScreen"));
            Demo.Checked = bool.Parse(mca.getObject("Demo"));
            doYgg.Checked = bool.Parse(mca.getObject("doYgg"));
            doYgg_CheckedChanged(null, null);
            string[] fs = Directory.GetDirectories(jo.getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions");
            for (int i = 0; i < fs.Length; i++)
            {
                fs[i] = fs[i].Replace(Directory.GetParent(fs[i]) + "\\", "");
            }
            VerSel.Items.AddRange(fs);
            if (VerSel.Items.Contains(jo.getString("SelectedVersion")))
            {
                VerSel.SelectedItem = jo.getString("SelectedVersion");
            }
            else
            {
                if (VerSel.Items.Count != 0)
                {
                    VerSel.SelectedIndex = 0;
                }
            }
            lvs.Items.AddRange(fs);
            lvs.Text = VerSel.Text;
            ovs.Items.AddRange(fs);
            ovs.Text = VerSel.Text;
            cd = new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json")).getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\");
            button12_Click(null, null);
            loading = false;
        }

        private void doYgg_CheckedChanged(object s, EventArgs a)
        {
            if (doYgg.Checked)
            {
                Nick.Enabled = false;
                UserName.Enabled = true;
                Password.Enabled = true;
            }
            else
            {
                Nick.Enabled = true;
                UserName.Enabled = false;
                Password.Enabled = false;
            }
        }

        private void UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Password.Text = Passwords[UserName.SelectedIndex];
        }

        private void saveChange()
        {
            JSONObject jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json"));
            JSONObject mca = new JSONObject();
            mca.putString("JrePath", JrePath.Text);
            mca.putString("MaxMemorySize", MaxMem.Value.ToString());
            mca.putString("WindowWidth", WindowWidth.Value.ToString());
            mca.putString("WindowHeight", WindowHeight.Value.ToString());
            mca.putString("ServerIp", ServerIp.Text);
            mca.putString("ExtraJreArgs", ExtraJre.Text);
            mca.putString("ExtraMCArgs", ExtraMC.Text);
            mca.putObject("doServerC", doServerC.Checked.ToString());
            mca.putObject("FullScreen", FullScreen.Checked.ToString());
            mca.putObject("Demo", Demo.Checked.ToString());
            mca.putObject("doYgg", doYgg.Checked.ToString());
            JSONArray YggDatas = new JSONArray();
            for (int i = 0; i < UserName.Items.Count; i++)
            {
                JSONObject YggData = new JSONObject();
                YggData.putString("UserName", UserName.Items[i].ToString());
                YggData.putString("Password", new Encryptor().EncryptStr(Passwords[i]));
                YggData.putObject("Selected", "False");
                YggDatas.putJSONObject(i, YggData);
            }
            JSONObject YggSData = new JSONObject();
            YggSData.putString("UserName", UserName.Text);
            YggSData.putString("Password", new Encryptor().EncryptStr(Password.Text));
            YggSData.putObject("Selected", "True");
            if (UserName.SelectedIndex != -1)
            {
                YggDatas.putJSONObject(UserName.SelectedIndex, YggSData);
            }
            else
            {
                YggDatas.putJSONObject(YggDatas.length(), YggSData);
            }
            jo.putJSONObject("MCLSettings", mca);
            jo.putJSONArray("YggDrasilLogin", YggDatas);
            jo.putString("Nick", Nick.Text);
            jo.putString("CurrentDir", GamePath.Text);
            File.WriteAllText(Application.StartupPath + "\\JLConfig.json", jo.toString());
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            if (UserName.SelectedIndex == -1)
            {
                UserName.Items.Add(UserName.Text);
                Passwords.Add(Password.Text);
                UserName.SelectedIndex = UserName.Items.Count - 1;
            }
            else
            {
                Passwords[UserName.SelectedIndex] = Password.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "选取你的游戏文件夹（.minecraft文件夹）：";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                GamePath.Text = ofd.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Application.StartupPath + "\\version_manifest.json"))
            {
                TaskToDo t = new TaskToDo();
                t.param = "https://launchermeta.mojang.com/mc/game/version_manifest.json;" + Application.StartupPath + "\\version_manifest.json";//下载版本列表文件
                t.Type = "Download";
                ShowTask st = new ShowTask(new TaskToDo[] { t });
                st.onTaskComplete += new EventHandler(this.button3_Click);
                st.beginTask();
                return;
            }
            jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\version_manifest.json"));
            File.Delete(Application.StartupPath + "\\version_manifest.json");
            vers = jo.getJSONArray("versions").toJSONObjects();
            gl.Items.Clear();
            foreach (JSONObject ver in vers)
            {
                string showi = ver.getString("id") + " ";
                switch (ver.getString("type"))
                {
                    case "release":
                        showi = showi + "正式版";
                        break;
                    case "snapshot":
                        showi = showi + "快照版";
                        break;
                    case "old_alpha":
                        showi = showi + "Alpha测试版";
                        break;
                    case "old_beta":
                        showi = showi + "Beta测试版";
                        break;
                }
                showi = showi + " 发布时间：" + ver.getString("releaseTime");
                gl.Items.Add(showi);
            }
        }

        private void label11_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("作者：bangbang93,http://bmclapi2.bangbang93.com/", label11);
        }

        ShowTask st;

        private void button4_Click(object sender, EventArgs e)
        {
            TaskToDo[] t = new TaskToDo[] { new TaskToDo(), new TaskToDo() };
            t[0].Type = "Download";
            t[0].param = "http://bmclapi2.bangbang93.com/version/" + vers[gl.SelectedIndex].getString("id") + "/client;" + GamePath.Text.Replace(".\\", Application.StartupPath + "\\") + "\\versions\\" + vers[gl.SelectedIndex].getString("id") + "\\" + vers[gl.SelectedIndex].getString("id") + ".jar";
            t[1].Type = "Download";
            t[1].param = "http://bmclapi2.bangbang93.com/version/" + vers[gl.SelectedIndex].getString("id") + "/json;" + GamePath.Text.Replace(".\\", Application.StartupPath + "\\") + "\\versions\\" + vers[gl.SelectedIndex].getString("id") + "\\" + vers[gl.SelectedIndex].getString("id") + ".json";
            st = new ShowTask(t);
            st.beginTask();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TaskToDo t = new TaskToDo();
            string sv = VerSel.SelectedItem.ToString() + "-";
            sv = sv.Substring(0, sv.IndexOf("-"));
            t.param = "http://bmclapi2.bangbang93.com/forge/minecraft/" + sv + ";" + Application.StartupPath + "\\fvl.json";//下载版本列表文件
            t.Type = "Download";
            ShowTask st = new ShowTask(new TaskToDo[] { t });
            st.onTaskComplete += new EventHandler(loadFvl);
            st.beginTask();
            return;
        }

        JSONObject[] jos;

        private void loadFvl(object sender, EventArgs e)
        {
            try
            {
                fl.Items.Clear();
                JSONArray ja = new JSONArray(File.ReadAllText(Application.StartupPath + "\\fvl.json"));
                jos = ja.toJSONObjects();
                string branches = "";
                foreach (JSONObject jo in jos)
                {
                    String a = jo.getObject("branch");
                    if (jo.getObject("branch") != "null")
                    {
                        branches = "-" + jo.getString("branch");
                    }
                    fl.Items.Add(jo.getString("version") + branches);
                    branches = "";
                }
                fl.Sorted = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (fl.SelectedIndex != -1)
            {
                TaskToDo ttd = new TaskToDo();
                ttd.Type = "Download";
                ttd.param = "http://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/" + VerSel.Text + "-" + fl.SelectedItem.ToString() + "/forge-" + VerSel.Text + "-" + fl.SelectedItem.ToString() + "-installer.jar;" + Application.StartupPath + "\\forge-installer_" + fl.SelectedItem.ToString() + ".jar";
                TaskToDo tti = new TaskToDo();
                tti.Type = "InstallForge";
                tti.param = Application.StartupPath + "\\forge-installer_" + fl.SelectedItem.ToString() + ".jar;" + VerSel.Text;
                ShowTask st = new ShowTask(new TaskToDo[] { ttd, tti });
                st.onTaskComplete += new EventHandler(changeDSourcef);
                st.beginTask();
            }
        }

        private void changeDSourcef(object a, EventArgs b)
        {
            if (new FileInfo(Application.StartupPath + "\\forge-installer_" + fl.SelectedItem.ToString() + ".jar").Length == 0)
            {
                TaskToDo ttd = new TaskToDo();
                ttd.Type = "Download";
                ttd.param = "http://files.minecraftforge.net/maven/net/minecraftforge/forge/" + VerSel.Text + "-" + fl.SelectedItem.ToString() + "/forge-" + VerSel.Text + "-" + fl.SelectedItem.ToString() + "-installer.jar;" + Application.StartupPath + "\\forge-installer_" + fl.SelectedItem.ToString() + ".jar";
                TaskToDo tti = new TaskToDo();
                tti.Type = "InstallForge";
                tti.param = Application.StartupPath + "\\forge-installer_" + fl.SelectedItem.ToString() + ".jar;" + VerSel.Text;
                new ShowTask(new TaskToDo[] { ttd, tti }).beginTask();
            }
        }

        bool l_snap=false;

        private void changeDSourcel(object a, EventArgs b)
        {
            if (new FileInfo(Application.StartupPath + "\\liteloader-" + ll.SelectedItem.ToString() + ".jar").Length == 0)
            {
                string sv = lvs.SelectedItem.ToString() + "-";
                sv = sv.Substring(0, sv.IndexOf("-"));
                TaskToDo ttd = new TaskToDo();
                ttd.Type = "Download";
                if (l_snap)
                {
                    ttd.param = String.Format("http://dl.liteloader.com/versions/com/mumfrey/liteloader/{0}/liteloader-{0}.jar;" + Application.StartupPath + "\\liteloader-{0}.jar", ll.SelectedItem.ToString());
                }
                else
                {
                    ttd.param = String.Format("http://dl.liteloader.com/versions/com/mumfrey/liteloader/{0}/liteloader-{1}.jar;" + Application.StartupPath + "\\liteloader-{1}.jar", sv, ll.SelectedItem.ToString());
                }
                TaskToDo tti = new TaskToDo();
                tti.Type = "InstallLiteLoader";
                tti.param = ll.SelectedItem.ToString() + ";" + lvs.SelectedItem.ToString();
                new ShowTask(new TaskToDo[] { ttd, tti }).beginTask();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TaskToDo t = new TaskToDo();
            t.param = "http://dl.liteloader.com/versions/versions.json;" + Application.StartupPath + "\\lvl.json";//下载版本列表文件
            t.Type = "Download";
            ShowTask st = new ShowTask(new TaskToDo[] { t });
            st.onTaskComplete += new EventHandler(loadLvl);
            st.beginTask();
            return;
        }

        private void loadLvl(object sender, EventArgs e)
        {
            ll.Items.Clear();
            string sv = lvs.SelectedItem.ToString() + "-";
            sv = sv.Substring(0, sv.IndexOf("-"));
            jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\lvl.json")).getJSONObject("versions").getJSONObject(sv);
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
                    ll.Items.Add(stable.getJSONObject(key).getString("version"));
                }
            }
            if (jo.Contains("snapshots"))
            {
                JSONObject snapshot = jo.getJSONObject("snapshots").getJSONObject("com.mumfrey:liteloader");
                var keys = snapshot.getKeys();
                foreach (string key in keys)
                {
                    if (key == "latest")
                    {
                        continue;
                    }
                    ll.Items.Add(snapshot.getJSONObject(key).getString("version"));
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sv = lvs.SelectedItem.ToString() + "-";
            sv = sv.Substring(0, sv.IndexOf("-"));
            if (ll.SelectedIndex != -1)
            {
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
                        if (stable.getJSONObject(key).getString("version") == ll.SelectedItem.ToString())
                        {
                            TaskToDo t = new TaskToDo();
                            t.Type = "Download";
                            t.param = String.Format("http://bmclapi2.bangbang93.com/maven/com/mumfrey/liteloader/{0}/liteloader-{1}.jar;" + Application.StartupPath + "\\liteloader-{1}.jar", sv, ll.SelectedItem.ToString());
                            TaskToDo t2 = new TaskToDo();
                            t2.Type = "InstallLiteLoader";
                            t2.param = ll.SelectedItem.ToString() + ";" + lvs.SelectedItem.ToString();
                            st = new ShowTask(new TaskToDo[] { t, t2 });
                            st.onTaskComplete += new EventHandler(changeDSourcel);
                            st.beginTask();
                        }
                    }
                }
                if (jo.Contains("snapshots"))
                {
                    JSONObject snapshot = jo.getJSONObject("snapshots").getJSONObject("com.mumfrey:liteloader");
                    var keys = snapshot.getKeys();
                    foreach (string key in keys)
                    {
                        if (key == "latest")
                        {
                            continue;
                        }
                        if (snapshot.getJSONObject(key).getString("version") == ll.SelectedItem.ToString())
                        {
                            l_snap = true;
                            TaskToDo t = new TaskToDo();
                            t.Type = "Download";
                            t.param = String.Format("http://dl.liteloader.com/versions/com/mumfrey/liteloader/{0}/liteloader-{0}.jar;" + Application.StartupPath + "\\liteloader-{0}.jar", ll.SelectedItem.ToString());
                            TaskToDo t2 = new TaskToDo();
                            t2.Type = "InstallLiteLoader";
                            t2.param = ll.SelectedItem.ToString() + ";" + lvs.SelectedItem.ToString();
                            st = new ShowTask(new TaskToDo[] { t, t2 });
                            st.onTaskComplete += new EventHandler(changeDSourcel);
                            st.beginTask();
                        }
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string sv = ovs.SelectedItem.ToString() + "-";
            sv = sv.Substring(0, sv.IndexOf("-"));
            TaskToDo ttd = new TaskToDo();
            ttd.Type = "Download";
            ttd.param = "http://bmclapi2.bangbang93.com/optifine/" + sv + ";" + Application.StartupPath + "\\OptiList.json";
            st = new ShowTask(new TaskToDo[] { ttd });
            st.onTaskComplete+=new EventHandler(loadOvl);
            st.beginTask();
        }

        private void loadOvl(object sender, EventArgs e)
        {
            ol.Items.Clear();
            JSONArray ja=new JSONArray(File.ReadAllText(Application.StartupPath + "\\OptiList.json"));
            jos = ja.toJSONObjects();
            foreach (JSONObject svj in jos)
            {
                ol.Items.Add(svj.getString("type") + "-" + svj.getString("patch"));
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string sv = ovs.SelectedItem.ToString() + "-";
            sv = sv.Substring(0, sv.IndexOf("-"));
            if (ol.SelectedIndex != -1)
            {
                TaskToDo ttd = new TaskToDo();
                ttd.Type = "Download";
                ttd.param = "http://bmclapi2.bangbang93.com/optifine/" + sv + "/" + ol.SelectedItem.ToString().Replace("-", "/") + ";" + cd + "\\mods\\Optifine_" + sv + "_" + ol.SelectedItem.ToString().Replace("-", "_") + ".jar";
                TaskToDo t = new TaskToDo();
                t.Type = "InstallOptiFine";
                t.param = ol.SelectedItem.ToString().Replace("-", "_") + ";" + ovs.SelectedItem.ToString();
                st = new ShowTask(new TaskToDo[] { ttd,t });
                st.onTaskComplete += new EventHandler(changeDSourceO);
                st.beginTask();
            }
        }

        private void changeDSourceO(object sender, EventArgs e)
        {
            string sv = ovs.SelectedItem.ToString() + "-";
            sv = sv.Substring(0, sv.IndexOf("-"));
            string filn="Optifine_" + sv + "_" + ol.SelectedItem.ToString().Replace("-", "_") + ".jar";
            if (new FileInfo(cd + "\\mods\\" + filn).Length == 0)
            {
                TaskToDo ttd = new TaskToDo();
                ttd.Type = "Download";
                if(filn.Contains("_pre")){
                    filn="preview_" + filn;
                }
                filn = filn.Replace("Optifine", "OptiFine");
                var htxt = wc.DownloadString("http://optifine.net/adloadx?f=" + filn);
                var req = htxt.Substring(htxt.IndexOf("downloadx?f="), htxt.IndexOf("\"", htxt.IndexOf("downloadx?f=")) - htxt.IndexOf("downloadx?f="));
                ttd.param = "http://optifine.net/" + req + ";" + cd + "\\mods\\Optifine_" + sv + "_" + ol.SelectedItem.ToString().Replace("-", "_") + ".jar";
                TaskToDo t = new TaskToDo();
                t.Type = "InstallOptiFine";
                t.param = ol.SelectedItem.ToString().Replace("-", "_") + ";" + ovs.SelectedItem.ToString();
                st = new ShowTask(new TaskToDo[] { ttd,t });
                st.beginTask();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            loading = true;
            ml.Items.Clear();
            if (!Directory.Exists(cd + "\\mods"))
            {
                Directory.CreateDirectory(cd + "\\mods");
            }
            string[] mods = Directory.GetFiles(cd + "\\mods");
            foreach (string mod in mods)
            {
                string fn = new FileInfo(mod).Name.Replace(".jar", "").Replace(".disabled", "");
                if (mod.EndsWith(".disabled"))
                {
                    ml.Items.Add(fn, false);
                }
                else
                {
                    ml.Items.Add(fn, true);
                }
            }
            loading = false;
        }

        private void ml_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!loading)
            {
                if (e.CurrentValue == CheckState.Checked)
                {
                    File.Move(cd + "\\mods\\" + ml.Items[e.Index] + ".jar", cd + "\\mods\\" + ml.Items[e.Index] + ".jar.disabled");
                }
                else
                {
                    File.Move(cd + "\\mods\\" + ml.Items[e.Index] + ".jar.disabled", cd + "\\mods\\" + ml.Items[e.Index] + ".jar");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Filter = "Mod文件（*.jar）|*.jar";
            ofd.Multiselect = true;
            ofd.Title = "选择要添加的Mod文件";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string amod in ofd.FileNames)
                {
                    File.Copy(amod, cd + "\\mods\\" + new FileInfo(amod).Name,true);
                }
                button12_Click(null, null);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            for (int i =  ml.Items.Count-1; i >=0; i--)
            {
                if (ml.GetItemChecked(i))
                {
                    File.Delete(cd + "\\mods\\" + ml.Items[i].ToString() + ".jar");
                    ml.Items.RemoveAt(i);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            for (int i = ml.Items.Count - 1; i >= 0; i--)
            {
                ml.SetItemChecked(i,true);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            for (int i = ml.Items.Count - 1; i >= 0; i--)
            {
                ml.SetItemChecked(i, false);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            gop.Clear();
            gameOut.Text = "";
        }

        StringBuilder gop = new StringBuilder();
        LauncherReturn lr=new LauncherReturn();

        private void button18_Click(object sender, EventArgs e)
        {
            gop.Append("已启动游戏。");
            gameOut.Text = gop.ToString();
            if (!File.Exists(Application.StartupPath + "\\" + Nick.Text + ".authL"))
            {
                Guid G = Guid.NewGuid();
                File.WriteAllLines(Application.StartupPath + "\\" + Nick.Text + ".authL", new string[] { G.ToString(), G.ToString() });
            }
            JSONObject jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json"));
            MCLaunchArgs mcla = new MCLaunchArgs();
            YggDrasilAuth yda = new YggDrasilAuth();
            JSONObject mca = jo.getJSONObject("MCLSettings");
            JSONArray ya = jo.getJSONArray("YggDrasilLogin");
            mcla.MaxMem = short.Parse(mca.getString("MaxMemorySize"));
            mcla.Nick = jo.getString("Nick");
            JSONObject[] yas = ya.toJSONObjects();
            JSONObject yt = new JSONObject();
            foreach (JSONObject yd in yas)
            {
                if (yd.getObject("Selected") == "True")
                {
                    yt = yd;
                }
            }
            yda.UserName = yt.getString("UserName");
            yda.Password = yt.getString("Password");
            yda.Yggauth = Boolean.Parse(mca.getObject("doYgg"));
            mcla.JrePath = mca.getString("JrePath");
            mcla.ExtraJreArgs = mca.getString("ExtraJreArgs");
            if (mca.getObject("doServerC") == "True")
            {
                mcla.ExtraMCArgs = mcla.ExtraMCArgs + "--server " + mca.getObject("ServerIp").Split(":".ToCharArray())[0] + " --port " + mca.getObject("ServerIp").Split(":".ToCharArray())[1] + " ";
            }
            if (mca.getObject("FullScreen") == "True")
            {
                mcla.ExtraMCArgs = mcla.ExtraMCArgs + "--fullscreen ";
            }
            if (mca.getObject("Demo") == "True")
            {
                mcla.ExtraMCArgs = mcla.ExtraMCArgs + "--demo ";
            }
            mcla.ExtraMCArgs = mcla.ExtraMCArgs + "--width " + mca.getObject("WindowWidth") + " --height " + mca.getObject("WindowHeight") + " ";
            mcla.ExtraMCArgs = mcla.ExtraMCArgs + mca.getString("ExtraMCArgs");
            mcla.CurrentDir = jo.getString("CurrentDir");
            mcla.VerName = jo.getString("SelectedVersion");
            yda.uuid = File.ReadAllLines(Application.StartupPath + "\\" + Nick.Text + ".authL")[0];
            yda.accessToken = File.ReadAllLines(Application.StartupPath + "\\" + Nick.Text + ".authL")[1];
            lr = new Launcher().LaunchMC(mcla, yda,1);
            if (lr.rtv == "缺少文件，正在下载。")
            {
                gop.Append("\n对不起，游戏缺少重要文件，启动失败。请先在主界面点击“启动”下载好他们后再进行调试。");
                gameOut.Text = gop.ToString();
            }
            else
            {
                lr.MCMain.StartInfo.RedirectStandardOutput = true;
                lr.MCMain.OutputDataReceived+=new DataReceivedEventHandler(OutputDataReceived);
                lr.MCMain.Start();
                lr.MCMain.BeginOutputReadLine();
            }
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            gop.Append("\n" + e.Data);
            delegatedUpdate(gop.ToString());
        }

        private void delegatedUpdate(string str)
        {
            if (gameOut.InvokeRequired)
            {
                UpdgOCallBack UpdgO = new UpdgOCallBack(this.delegatedUpdate);
                gameOut.Invoke(UpdgO, new object[] { str });
            }
            else
            {
                gameOut.Text = str;
            }
        }

        private void gameOut_TextChanged(object sender, EventArgs e)
        {
            gameOut.Text = gop.ToString();
            gameOut.Select(gameOut.TextLength, 0);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                lr.MCMain.Kill();
            }
            catch (Exception ex)
            {
            }
        }
    }
}