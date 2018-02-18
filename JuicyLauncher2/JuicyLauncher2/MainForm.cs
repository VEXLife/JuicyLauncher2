using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using BottleJson;

namespace JuicyLauncher2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        bool Loading = false;
        bool Exit = true;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Loading = true;
            if (File.Exists(Application.StartupPath + "\\JLConfig.json"))
            {
                JSONObject jo = new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json"));
                Nick.Text = jo.getString("Nick");
                if (!Directory.Exists(jo.getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions"))
                {
                    Directory.CreateDirectory(jo.getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions");
                }
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
            }
            else
            {
                JSONObject jo = new JSONObject();
                jo.putString("Nick", "Steve");
                jo.putString("CurrentDir", ".\\.minecraft");
                if (!Directory.Exists(jo.getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions"))
                {
                    Directory.CreateDirectory(jo.getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions");
                }
                string[] fs = Directory.GetDirectories(jo.getString("CurrentDir").Replace(".\\", Application.StartupPath + "\\").Replace("./", Application.StartupPath + "/") + "\\versions");
                for (int i = 0; i < fs.Length; i++)
                {
                    fs[i] = fs[i].Replace(Directory.GetParent(fs[i]) + "\\", "");
                }
                VerSel.Items.AddRange(fs);
                if (VerSel.Items.Count != 0)
                {
                    VerSel.SelectedIndex = 0;
                }
                if (VerSel.Items.Count == 0)
                {
                    jo.putString("SelectedVersion", "");
                }
                else
                {
                    jo.putString("SelectedVersion", VerSel.Items[0].ToString());
                }
                String[] js = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment").GetSubKeyNames();
                JSONObject mca = new JSONObject();
                mca.putString("JrePath", Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JavaSoft\Java Runtime Environment\" + js[0]).GetValue("JavaHome") + "\\bin\\javaw.exe");
                mca.putString("MaxMemorySize", "1024");
                mca.putString("WindowWidth", "854");
                mca.putString("WindowHeight", "480");
                mca.putString("ServerIp", "");
                mca.putString("ExtraJreArgs", "-Dfml.ignoreInvalidMinecraftCertificates=true -Dfml.ignorePatchDiscrepancies=true -Dminecraft.launcher.brand=JuicyLauncher2 -Dminecraft.launcher.version=2");
                mca.putString("ExtraMCArgs", "");
                mca.putObject("doServerC", "False");
                mca.putObject("FullScreen", "False");
                mca.putObject("Demo", "False");
                mca.putObject("doYgg", "False");
                JSONArray YggDatas = new JSONArray();
                jo.putJSONObject("MCLSettings", mca);
                jo.putJSONArray("YggDrasilLogin", YggDatas);
                File.WriteAllText(Application.StartupPath + "\\JLConfig.json", jo.toString());
            }
            Loading = false;
        }

        private void saveChange()
        {
            if (Loading == false)
            {
                String fw = File.ReadAllText(Application.StartupPath + "\\JLConfig.json");
                JSONObject jo = new JSONObject(fw);
                jo.putString("Nick", Nick.Text);
                jo.putString("SelectedVersion", VerSel.Text);
                File.WriteAllText(Application.StartupPath + "\\JLConfig.json", jo.toString());
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveChange();
            if(Exit==true){
                Application.Exit();
            }
        }

        private void VerSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveChange();
        }

        private void Nick_TextChanged(object sender, EventArgs e)
        {
            saveChange();
        }

        private void Stts_Click(object sender, EventArgs e)
        {
            Exit = false;
            new Settings().Show();
            this.Close();
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            saveChange();
            if (!File.Exists(Application.StartupPath + "\\" + Nick.Text + ".authL"))
            {
                Guid G = Guid.NewGuid();
                File.WriteAllLines(Application.StartupPath + "\\" + Nick.Text + ".authL", new string[] { G.ToString(), G.ToString() });
            }
            JSONObject jo=new JSONObject(File.ReadAllText(Application.StartupPath + "\\JLConfig.json"));
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
                mcla.ExtraMCArgs = mcla.ExtraMCArgs + "--server " + mca.getString("ServerIp").Split(":".ToCharArray())[0] + " --port " + mca.getString("ServerIp").Split(":".ToCharArray())[1] + " ";
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
            this.Enabled = false;
            this.Text = "果汁启动器正在启动……";
            label3.Text = "果汁启动器正在启动……";
            new Launcher().LaunchMC(mcla, yda);
        }

        private void cb_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        bool mdown = false;
        int dX;
        int dY;
        private void fMouseDown(object sender, MouseEventArgs e)
        {
            mdown = true;
            dX = e.X;
            dY = e.Y;
        }

        private void fMouseUp(object sender, MouseEventArgs e)
        {
            mdown = false;
        }

        private void fMouseMove(object sender, MouseEventArgs e)
        {
            if (mdown)
            {
                this.Left += e.X-dX;
                this.Top += e.Y-dY;
            }
        }
    }
}
