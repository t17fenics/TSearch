/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 02.09.2019
 * Время: 15:36
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cassia;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		string DameWarePatch;
		public MainForm()
		{
			
			InitializeComponent();
			
			//Создаем переменную пуки к DameWare, определяя его через функцию getDameWarePath();
			DameWarePatch = getDameWarePath();
			//Удаляем файл журнала
			File.Delete(Application.StartupPath+@"\log.txt");
			
			createSessionList();
			filterRow();
			textBox2.TabIndex=0;
		}
		
		
		//Функция создания 
		void createSessionList()
		{

			//System.Threading.Thread.Sleep(1000);
			//string srvName;
			//serverListRead()
			//string[] srvList = new string[]{"srv1wts2","srv1wts3","srv2wts2","srv2wts3","buhwts2012","abel2"};// "DEsign12"};
			//string[] srvList = new string[]{"abel2"};
			WTSServer Server = new WTSServer();
			dataGridView1.Rows.Clear();
			dataGridView3.Rows.Clear();
			Ping ping = new Ping();
            PingReply pingReply = null;
			foreach(string srvName in serverListRead())
			{
				pingReply = ping.Send(srvName,150);
				//Log(srvName + " " + pingReply.Status.ToString());
				if (pingReply.Status == IPStatus.Success)
 				{
					int countSession=0;
					//Log(srvName);
					//MessageBox.Show(srvName);
					//srvName=name;
					//sessionList = Server.getSessionList(srvName);
					foreach(ITerminalServicesSession session in Server.getSessionList(srvName))
					{
						
						//sess
						//if(session.UserAccount.ToString().Contains(textBox2.Text))
						//{
							dataGridView1.Rows.Add("0", session.SessionId, session.UserAccount, session.ConnectionState, srvName, session.ClientName);
							Log(session.UserAccount+" "+ pingReply.Address);
						//}
						countSession++;
					}
					if(countSession==0)
					{
						dataGridView3.Rows.Add(srvName, countSession + Server.serverStatus);
					}else
					{
						dataGridView3.Rows.Add(srvName, countSession);
					}
				}else{
					dataGridView3.Rows.Add(srvName, pingReply.Status);
				}
			}
			//button1.Text = "Обновлено";
			//label2.Text = "Обновлено";

		}
		
		/*void filterRow()
		{
			
			dataGridView2.Rows.Clear();
			for(int i =0; i < dataGridView1.RowCount; i++)
			{
				//dataGridView1[i,2].FormattedValue.ToString()
					//MessageBox.Show(dataGridView1[2,i].FormattedValue.ToString());
					if(dataGridView1[2,i].FormattedValue.ToString().ToLower().Contains(textBox2.Text))
					{
						
					   //MessageBox.Show(dataGridView1[2,i].FormattedValue.ToString());
					   dataGridView2.Rows.Add(dataGridView1[2,i].FormattedValue,dataGridView1[4,i].FormattedValue,dataGridView1[1,i].FormattedValue, dataGridView1[3,i].FormattedValue);
					}
			}
		}*/
		void filterRow()
		{
			string userName;
			dataGridView2.Rows.Clear();
			foreach(DataGridViewRow row in dataGridView1.Rows)
			{
				if(!row.Cells[2].FormattedValue.ToString().ToLower().Substring(0,9).Contains("interstep")){
					userName = row.Cells[2].FormattedValue.ToString().ToLower();
				} else {
					userName = row.Cells[2].FormattedValue.ToString().ToLower().Substring(10);
				}
				//MessageBox.Show(userName);
				//userName = row.Cells[2].FormattedValue.ToString().Substring(10);
				//MessageBox.Show(userName);
				if(userName.Contains(textBox2.Text)&&checkBox2.Checked == true)
				{
					dataGridView2.Rows.Add(userName, row.Cells[4].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString(), getServerType(row.Cells[4].Value.ToString()));
				
				//MessageBox.Show(row.Cells[2].Value.ToString());
				//string userName = row.Cells[2].Value.ToString();
				//MessageBox.Show(userName);
				//dataGridView2.Rows.Add(row.Cells[2].Value.ToString(), row.Cells[4].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString(), getServerType(row.Cells[4].Value.ToString()));
				}else if(userName.Contains(textBox2.Text)&&checkBox2.Checked == false&&row.Cells[3].Value.ToString().Equals("Active"))
				         {
				         	dataGridView2.Rows.Add(userName, row.Cells[4].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString(), getServerType(row.Cells[4].Value.ToString()));
				         }
			}
			//for(int i =0; i < dataGridView1.RowCount; i++)
			//{
				//dataGridView1[i,2].FormattedValue.ToString()
					//MessageBox.Show(dataGridView1[2,i].FormattedValue.ToString());
					//if(dataGridView1[2,i].FormattedValue).ToLower().Contains(textBox2.Te.ToString(xt))
					//{
						
					   //MessageBox.Show(dataGridView1[2,i].FormattedValue.ToString());
					   //dataGridView2.Rows.Add(dataGridView1[2,i].FormattedValue,dataGridView1[4,i].FormattedValue,dataGridView1[1,i].FormattedValue, dataGridView1[3,i].FormattedValue);
					//}
			//}

		}

	
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			label3.Text="";
			filterRow();
		}
		
		//для того, чтобы при опросе компьютеров не получать сообщение "Отказано в доступе", необходимо внести изменения в реестр удаленной машины
		//[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server]
		//“AllowRemoteRPC”=dword:00000001
		void connectPC(string namePC, bool viewOnly)
		{
			//ProcessStartInfo startInfo = new ProcessStartInfo("dwrcc.exe");
			
			//startInfo.WorkingDirectory = "\"C:\\Program Files\\DameWare Development\\DameWare NT Utilities\"";
			//startInfo.WorkingDirectory = @"C:\Program Files\DameWare Development\DameWare NT Utilities\";
			//startInfo.FileName = "\"C:\\Program Files\\DameWare Development\\DameWare NT Utilities\\dwrcc.exe\"";
			//startInfo.FileName = @"C:\Program Files\DameWare Development\DameWare NT Utilities\dwrcc.exe";
			//startInfo.Arguments="-c: -h: -m:" + namePC + " -v:";
			//MessageBox.Show(namePC);
			if(viewOnly)
			{
				string dwRun = " -c: -h: -m:" + namePC + " -v:";
				//Log(DameWarePatch+dwRun);
				Process.Start(DameWarePatch, dwRun);
			} else {
				string dwRun = " -c: -h: -m:" + namePC;
				//Process.Start(DameWarePatch,"-c: -h: -m:" + namePC);
				//Log(DameWarePatch+dwRun);
				Process.Start(DameWarePatch,dwRun);
			}
				//("c:\Program Files\DameWare Development\DameWare NT Utilities\dwrcc -c: -h: -m:design12 -v:");
		}
		void Button1Click(object sender, EventArgs e)
		{
			//button1.Text="Обновляем";
			//string path= @Application.StartupPath @"C:\SomeDir\hta.txt"
				//MessageBox.Show(Application.StartupPath);
				//dataGridView3.Rows.Clear();
						createSessionList();
			
			filterRow();
						label3.Text="Обновлено";
						textBox2.Focus();

			//MessageBox.Show("Обновлено"
			
		}
		//void DataGridView2Click(object sender, EventArgs e)
		//{
			//connectPC("abel2")
		//}
		/*void DataGridView2MouseClick(object sender, DataGridViewCellMouseEventArgs e)// MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				//MessageBox.Show("Right");
				//var ind = dataGridView2.CurrentRow.Index;
				dataGridView2.CurrentCell = dataGridView2[e.Location.ColumnIndex, e.Location.RowIndex];
				
				MessageBox.Show(dataGridView2.CurrentCell.Value.ToString());
				//connectPC(dataGridView2[2,ind].ToString());
			} else if(e.Button == MouseButtons.Left)
			{
				MessageBox.Show("Left");
			}
		}*/
		void DataGridView2CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			//
			//MessageBox.Show(e.RowIndex.ToString());
			if(e.RowIndex != -1)
			{
							dataGridView2.CurrentCell = dataGridView2[e.ColumnIndex, e.RowIndex];
					//t Message = dataGridView2.CurrentRow.Index;		
				var ind = dataGridView2.CurrentRow.Index;
				string type = dataGridView2[4, ind].Value.ToString();
				//MessageBox.Show(dataGridView2.CurrentRow.Index)
			if (e.Button == MouseButtons.Right)
			{

				
				//MessageBox.Show(dataGridView2[1, ind].ToString());
				if(type== "pc")
				{
					Log("Connect to " + dataGridView2[1, ind].Value);
					//MessageBox.Show(dataGridView2[1, ind].ToString());
					connectPC(dataGridView2[1, ind].Value.ToString(),true);//MessageBox.Show(dataGridView2[4, ind].Value.ToString());
				} else if(type == "terminal")
				{
					rdpCreate(dataGridView2[1, ind].Value.ToString(), Convert.ToInt32(dataGridView2[2, ind].Value), false);
					Process.Start("mstsc", @"U:\Tsearch.tmp");
					//rdpCreate(", dataGridView2[0, ind].Value, false);
				}
				//connectPC("abel2");
			} else if(e.Button == MouseButtons.Left)
			{
				if(type== "pc")
				{
					Log("Connect to " + dataGridView2[1, ind].Value);
					//MessageBox.Show(dataGridView2[1, ind].ToString());
					connectPC(dataGridView2[1, ind].Value.ToString(),false);//MessageBox.Show(dataGridView2[4, ind].Value.ToString());
				} else if(type == "terminal")
				{
					rdpCreate(dataGridView2[1, ind].Value.ToString(), Convert.ToInt32(dataGridView2[2, ind].Value), true);
					Process.Start("mstsc", @"U:\Tsearch.tmp");
					//MessageBox.Show("управление PC");
				}
				//MessageBox.Show(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432").ToString());
			}
			}
		}
		
		
		//Подключение через DMware c:\Program Files\DameWare Development\DameWare NT Utilities>dwrcc -c: -h: -m:design12 -v:
		string getDameWarePath()
		{
			string x86 = @"C:\Program Files\DameWare Development\DameWare NT Utilities\dwrcc.exe";
			string x64 = @"C:\Program Files (x86)\DameWare Development\DameWare NT Utilities\dwrcc.exe";
			if(File.Exists(x86))
			{
				return x86;

			} else if(File.Exists(x64))
			{
				return x64;
			}  
			
				MessageBox.Show("У вас не установелн DameWare");
				return null;
			

		}
		
		
		List<string> serverListRead()
		{
			//string[] serverList;
			List<string> serverList = new List<string>{};
			string path= Application.StartupPath+@"\settings.ini";
			using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
    		{
        		string line;
        		while ((line = sr.ReadLine()) != null)
        		{
        			
        			string[] words = line.Split(new char[] { '	' });

					if(words[1]=="terminal" && checkBox1.Checked==false)
					{
						serverList.Add(words[0]);// .Substring);
					}else if(checkBox1.Checked==true)
					{
						serverList.Add(words[0]);// .Substring);
					}
        				
       			}
			}
			return serverList;
		}
		string getServerType(string srvName)
		{
			string path= Application.StartupPath+@"\settings.ini";
			using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
    		{
        		string line;
        		while ((line = sr.ReadLine()) != null)
        		{
        			string[] words = line.Split(new char[] { '	' });
        			if(words[0].Equals(srvName))
        			{
        				return words[1];
        			}
        			//return null;
					
       			}
			}
			return null;
		}
		void rdpCreate(string adress, int sessionID, bool control)
		{
			string path = @"U:\TSearch.tmp";
			string sessionWTS;
			string[] rdpLines = {	"screen mode id:i:2" ,
									"desktopwidth:i:1280" ,
									"desktopheight:i:1024" ,
									"session bpp:i:32" ,
									"winposstr:s:0,3,0,0,800,600" ,
									"compression:i:1" ,
									"keyboardhook:i:2" ,
									"audiomode:i:0" ,
									"redirectprinters:i:1" ,
									"redirectcomports:i:0" ,
									"redirectsmartcards:i:1" ,
									"redirectclipboard:i:1" ,
									"redirectposdevices:i:0" ,
									"displayconnectionbar:i:1" ,
									"autoreconnection enabled:i:1" ,
									"authentication level:i:0" ,
									"prompt for credentials:i:0" ,
									"negotiate security layer:i:1" ,
									"remoteapplicationmode:i:0"};
			System.IO.File.WriteAllLines(@"U:\TSearch.tmp", rdpLines);
			if(control == true)
			{
				sessionWTS = "alternate shell:s:mstsc /shadow:" + sessionID + " /noConsentPrompt /control";
			}else{
				sessionWTS = "alternate shell:s:mstsc /shadow:" + sessionID + " /noConsentPrompt";
			}
			using(StreamWriter sw = File.AppendText(path))
			{
				sw.WriteLine(sessionWTS);
			}					
			string[] rdpLines1 = {	"shell working directory:s:" ,
									"disable wallpaper:i:1" ,
									"disable full window drag:i:1" ,
									"allow desktop composition:i:0" ,
									"allow font smoothing:i:0" ,
									"disable menu anims:i:1" ,
									"disable themes:i:0" ,
									"disable cursor setting:i:0" ,
									"bitmapcachepersistenable:i:1" ,
									"gatewayhostname:s:" ,
									"gatewayusagemethod:i:0" ,
									"gatewaycredentialssource:i:4" ,
									"gatewayprofileusagemethod:i:0" ,
									"use multimon:i:0" ,
									"audiocapturemode:i:0" ,
									"videoplaybackmode:i:1" ,
									"connection type:i:7" ,
									"networkautodetect:i:1" ,
									"bandwidthautodetect:i:1" ,
									"enableworkspacereconnect:i:0" ,
									"promptcredentialonce:i:0" ,
									"gatewaybrokeringtype:i:0" ,
									"use redirection server name:i:0" ,
									"rdgiskdcproxy:i:0" ,
									"kdcproxyname:s:"};
			System.IO.File.AppendAllLines(@"U:\TSearch.tmp", rdpLines1);
			string adressWTS = "full address:s:" + adress;
			using(StreamWriter sw = File.AppendText(path))
			{
				sw.WriteLine(adressWTS);
			}

		}
		void Label3Click(object sender, EventArgs e)
		{
	
		}
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
	label3.Text="";
		}
		void Label2Click(object sender, EventArgs e)
		{
	
		}
				void CheckBox2CheckedChanged(object sender, EventArgs e)
		{
			

			filterRow();
		}
		//Функция журналирования
		public static void Log(string message) {
 			File.AppendAllText(Application.StartupPath+@"\log.txt", message+"\n");
		}

		void Button2Click(object sender, EventArgs e)
		{
			//MessageBox.Show(this.Height.ToString());
			if(this.Height==715)
			{
				this.Size = new System.Drawing.Size(594, 514);
			} else if (this.Height==514)
				this.Size = new System.Drawing.Size(594, 715);
				label3.Text="";
			//button2.Image="down_icon-icons.com_61209.ico";
			
		}

		void DataGridView2CellEnter(object sender, DataGridViewCellEventArgs e)
		{
	label3.Text="";
		}
		void Button1Enter(object sender, EventArgs e)
		{
	label3.Text="";
		}


	}
}
