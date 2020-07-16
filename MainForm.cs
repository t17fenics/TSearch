/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 02.09.2019
 * Время: 15:36
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
 
 
//для того, чтобы при опросе компьютеров не получать сообщение "Отказано в доступе", необходимо внести изменения в реестр удаленной машины
//[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server]
//“AllowRemoteRPC”=dword:00000001

//Подключение через DMware c:\Program Files\DameWare Development\DameWare NT Utilities>dwrcc -c: -h: -m:design12 -v:

//Для работы с Терминальными серверами спользуется библиотека https://github.com/danports/cassia

//------Доработки

//Блокировка подключения к disconnected

//Попробовать делать запрос к серверу при щелчке для подключения, чтобы обновить информацию о сесии




using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Cassia;
using System.Diagnostics;
using System.IO;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		//Статус проверки серверов
		//bool startCheckingTerminal = false;
		bool startCheckingPC = false;
		//Путь к DameWare
		string DameWarePatch;
		
		//Создаем объекты всех листов и баз
		ConfigList configList = new ConfigList();
		ServerList serverList = new ServerList();
		SessionList sessionList = new SessionList();
		ServerBase serverBase = new ServerBase();
		SessionBase sessionBase = new SessionBase();
		public string statusLabel;
		ITerminalServicesManager manager = new TerminalServicesManager();
		ConfigEditor configEditor;
		
		ToolStripMenuItem fileItem = new ToolStripMenuItem("Файл");
		ServerStatusForm serverStatusForm = new ServerStatusForm();
		InformationForm informationForm = new InformationForm();
		AboutForm aboutForm = new AboutForm();
		
		
		
        //Создаем объект таймер
		System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
		System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();

		public MainForm()
		{
			InitializeComponent();
			toolStripStatusLabel1.Text = statusLabel;		
			//Задаем переменную пути к DameWare, определяя его через функцию getDameWarePath();
			//DameWarePatch = getDameWarePath();
			
			//Удаляем старый файл журнала
			File.Delete(Application.StartupPath+@"\log.txt");
			            отображатьОтключенныхToolStripMenuItem.Checked = false;
			//Фильтация грида по отсутствующей галочке checkBox2
			filterDataGridView(textBox2.Text);
			
			//Создаем configList из текстового файла settings.ini
			configList.createConfigList();
			//menuStrip1.Items.Add(fileItem);
			//Создаем serverList из configList
			serverList.createServerList(configList.configList);
			//serverBase.fillingTable(serverList.serverList);
			
			//dataGridView3.DataSource = serverBase.serverListDB;
			dataGridView2.DataSource = sessionBase.sessionListDB;
			dataGridView2.Columns["clientName"].Visible = false;
			dataGridView2.Columns["connectTime"].Visible = false;
			dataGridView2.Columns["disconnectTime"].Visible = false;
			dataGridView2.Refresh();
			//dataGridView3.Refresh();
			//refrashTable()
			Thread myThread = new Thread(checkTerminalServerStatus);
			myThread.Start();
			//refrashT();



			timer1.Interval = 500; //интервал между срабатываниями 500 миллисекунд
			timer1.Tick += new EventHandler(refrashT); //подписываемся на события Tick
            timer1.Start();
           	timer2.Interval = 2000; //интервал между срабатываниями 15000 миллисекунд
			timer2.Tick += new EventHandler(threadUpdateTerminalStatus); //подписываемся на события Tick
            timer2.Start();
       
			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView2, new object[] { true });

		}
		
		//Функция запрашивает статусы для всех сереров. Если статус Success, то так же у сервера запрашиваются сессии
		
		void refrashT(object sender,EventArgs e)
		{
			
			Thread myThread = new Thread(refrashList);
			myThread.Start();
			refrashTable();
		}
			
		void refrashT()
		{
			Thread myThread = new Thread(refrashList);
			myThread.Start();
			//refrashTable();
		}
		void refrashList()
		{
			//Заполняем лист сессиями
			sessionList.fillingSessionList(serverList.serverList);
			
		}
		
		
		void refrashTable()
		{
			serverBase.fillingTable(serverList.serverList);
			//Внесение данных из sessionList в sessionListDB
			sessionBase.fillingTable(sessionList.allSessionList);
			
		}
		//Функция запрашивает статусы для терминальных сереров. Если статус Success, то так же у сервера запрашиваются сессии
		void checkTerminalServerStatus()
		{
			Debug.WriteLine("Запускаем checkTerminalServerStatus" + " " + DateTime.Now.ToString());
			//statusLabel = "Получаем сессии с терминальных серверов...";
			//toolStripStatusLabel1.Text = statusLabel;
			string LC = "";
			UserSession session;
			string serverStatus;
			long diffTime = 0;
			
			DateTime old;// = Convert.ToDateTime("01.01.0001 0:00:00");
			try
			{
			foreach(WTSServer server in serverList.serverList)
			{	
				//Debug.WriteLine("Начинаем сканировать " + server.serverName);
				//if(server.serverType != "terminal")
				//{
				serverStatus = server.checkServerStatus();

				if((server.serverStatus == "Success" || server.serverStatus == null))
					{
										
					//serverStatus = server.checkServerStatus();
					//Debug.WriteLine((long)(Convert.ToDateTime(server.lastCheck) - DateTime.Now).TotalSeconds);
					// - DateTime.Now).TotalSeconds);
					//Debug.WriteLine(server.serverName + " " + server.lastCheck);
					//Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
					//Debug.WriteLine(server.lastCheck);
					//Debug.WriteLine("жопа");
					LC = server.lastCheck;
					if(serverStatus =="Success" && LC != "retry")
					{
						old = Convert.ToDateTime(server.lastCheck);
						//if(old.ToString() != "01.01.0001 0:00:00")
						//{
						
						diffTime = (long)(DateTime.Now - old).TotalSeconds;
						//Debug.WriteLine(old);
						//}
					}
					if(serverStatus =="Success" && server.serverType == "terminal" && diffTime >= 3) //&& server.lastCheck != null)
					{

							//Debug.WriteLine(server.serverName + " " + server.lastCheck);
							//Debug.WriteLine(DateTime.Now);
							
							//DateTime old = Convert.ToDateTime(server.lastCheck);
							//Debug.WriteLine("жопа2");
							
		
							//if(LC != "retry")// && )//  && diffTime >= 5)
							//{
							//Debug.WriteLine("Начинаем сканировать " + server.serverName + " (" + server.lastCheck + ")"); 
							//Debug.WriteLine(Convert.ToDateTime(server.lastCheck));
							//Debug.WriteLine(DateTime.Now);
							//Debug.WriteLine((long)(DateTime.Now - Convert.ToDateTime(server.lastCheck)).TotalSeconds);
							server.lastCheck = "retry";
							Thread myThread = new Thread(server.getSessionList1);
							myThread.Name = server.serverName + "Thread";
							//Debug.WriteLine("Поток" + server.serverName);
							myThread.Start();
						} else if(serverStatus =="Success" && server.serverType == "pc" && diffTime >= 3)
						{
													//Debug.WriteLine(DateTime.Now);
							
							//DateTime old = Convert.ToDateTime(server.lastCheck);
							//Debug.WriteLine("жопа2");
							
		
							//if(LC != "retry")// && )//  && diffTime >= 5)
							//{
							//Debug.WriteLine("Начинаем сканировать " + server.serverName + " (" + server.lastCheck + ")"); 
							//Debug.WriteLine(Convert.ToDateTime(server.lastCheck));
							//Debug.WriteLine(DateTime.Now);
							//Debug.WriteLine((long)(DateTime.Now - Convert.ToDateTime(server.lastCheck)).TotalSeconds);
							server.lastCheck = "retry";
							Thread myThread = new Thread(server.getSessionList1);
							myThread.Name = server.serverName + "Thread";
							//Debug.WriteLine("Поток" + server.serverName);
							myThread.Start();
							//Debug.WriteLine("На сервере " + server.serverName + " " + server.serverSessionList.Count + " сессий");
							
							//if(server.serverSessionList.Count != 0)
							//{
								//session = sessionList.setServerDeleted(server);
								//sessionBase.setRowDeleted(session);
							//}
				}
							
						
					
					}
					//Debug.WriteLine("Поток" + server.serverName + " завершен");
					
				}
			
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Ошибка в методе" + MethodBase.GetCurrentMethod().Name);
				Debug.WriteLine(LC);
				Debug.WriteLine(ex);
			}
			
			//startCheckingTerminal = false;
			
			//statusLabel = "Сесии с терминальных сереров получены";
			//toolStripStatusLabel1.Text = statusLabel;
		}
		
		void checkTerminalServerStatus(object sender,EventArgs e)
		{
			//statusLabel = "Получаем сессии с терминальных серверов...";
			//toolStripStatusLabel1.Text = statusLabel;
			UserSession session;
			string serverStatus;
			try
			{
			foreach(WTSServer server in serverList.serverList)
			{	
				Debug.WriteLine(server.serverName + " " + server.serverStatus);
				if(server.serverStatus != "Success")
				{
					Debug.WriteLine("жопа");
				}
						
					serverStatus = server.checkServerStatus();
					//Debug.WriteLine("На сервере " + server.serverName + " " + server.serverStatus);
					if(serverStatus =="Success" && server.lastCheck != "checking")
					{
						server.lastCheck = "checking";
						Thread myThread = new Thread(server.getSessionList1);
						myThread.Start();
						//Debug.WriteLine("На сервере " + server.serverName + " " + server.serverSessionList.Count + " сессий");

						//if(server.serverSessionList.Count != 0)
						//{
							//session = sessionList.setServerDeleted(server);
							//sessionBase.setRowDeleted(session);
						//}
					}
					server.lastCheck = DateTime.Now.ToString();

				//}
				
			}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			//startCheckingTerminal = false;
			
			//statusLabel = "Сесии с терминальных сереров получены";
			//toolStripStatusLabel1.Text = statusLabel;
		}
		
		//Функция запрашивает статусы для Компьютеров. Если статус Success, то так же у сервера запрашиваются сессии
		void checkPCStatus()
		{
			UserSession session;
			statusLabel = "Получаем сессии с компьютеров...";
			toolStripStatusLabel1.Text = statusLabel;
			
			string serverStatus;
			int countSession;
			try
			{
			foreach(WTSServer server in serverList.serverList)
			{	
				if(server.serverType == "pc")
				{			
					statusLabel = "Получаем сессии с компьютеров... " + server.serverName;
					toolStripStatusLabel1.Text = statusLabel;
					serverStatus = server.checkServerStatus();
					if(serverStatus =="Success")
					{
						//countSession = 
							server.getSessionList();
						//if(countSession != serverBase.getWTSSessionCount(server.serverName))
						//{
							//server.getSessionList();
						if(!startCheckingPC)
						{
							startCheckingPC = true;
							
							if(server.serverSessionList.Count != 0)
							{
								session = sessionList.setServerDeleted(server);
								sessionBase.setRowDeleted(session);
								//session = sessionList.setRowDeleted(server);
								//sessionBase.removeRow(session);
							}
							
						}
					}
				}
			}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			startCheckingPC = false;
			statusLabel = "Сесии с компьютеров получены";
			toolStripStatusLabel1.Text = statusLabel;

		}
		
		//Функция для односекундного таймера обновление данных в листе и в базе
		void updateAllBase(object sender,EventArgs e)
		{

			//toolStripStatusLabel1.Text = statusLabel;
			//Внесение данных из serverList в serverListDB
			serverBase.fillingTable(serverList.serverList);
			//Заполняем лист сессиями
			sessionList.fillingSessionList(serverList.serverList);
			//sessionList.removeRow();
			//Внесение данных из sessionList в sessionListDB
			sessionBase.fillingTable(sessionList.allSessionList);
			
			//threadUpdateTerminalStatus();
			
			sessionList.removeSession();
			sessionBase.removeRow();
			dataGridView2.Refresh();

			

			
		}
		//Функция создающая поток, в котором обновляются статусы серверов и сессий
		void threadUpdateTerminalStatus(object sender,EventArgs e)
		{
				Thread myThread = new Thread(checkTerminalServerStatus);
				myThread.Start();
				

		}
		
		
		
		
		void threadUpdatePCStatus(object sender,EventArgs e)
		{
			if(!startCheckingPC)
			{
				Thread myThread1 = new Thread(checkPCStatus);
				myThread1.Start();
			}
		}
		
		//Фильтруем DataGrid по статусу Active если checkBoxChecked установлен
		public void filterDataGridView(string userName)
		{
			var filters = new List<string>();
			if(отображатьОтключенныхToolStripMenuItem.Checked)
			{
				//отображатьОтключенныхToolStripMenuItem.Checked = true;

				filters.Add(string.Format("userName like '{0}%'", userName));
				sessionBase.sessionListDB.DefaultView.RowFilter = string.Join(" AND ", filters);

			}
			else
			{


				filters.Add(string.Format("status like '{0}%'", "Active"));
				filters.Add(string.Format("userName like '{0}%'", userName));
				sessionBase.sessionListDB.DefaultView.RowFilter = string.Join(" AND ", filters);


			}

		}
		
		//Функция подключения к компьютерам(pc)
		void connectPC(string namePC, bool viewOnly)
		{
			if(viewOnly)
			{
				string dwRun = " -c: -h: -m:" + namePC + " -v:";
				Process.Start(DameWarePatch, dwRun);
			} else {
				string dwRun = " -c: -h: -m:" + namePC;
				Process.Start(DameWarePatch,dwRun);
			}
		}
			
		//Функция для получеиня пути установки DameWare
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
	
		//Функция формирующая файл RDP на диске U
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
		//---------------------------------------------------------------------------------------------
		//Управление интерфейсом
		
		//Событие отслеживающее изменение TextBox2
		void TextBox2TextChanged(object sender, EventArgs e)
		{

			//filterRow();
			filterDataGridView(textBox2.Text);
		}
		
		//Событие изменения CheckBox2
		//void CheckBox2CheckedChanged(object sender, EventArgs e)
		//{
			//filterDataGridView(textBox2.Text);
		//}
		
		//Событие правого и левого щелчка мышью по строке DataGridView2C
		void DataGridView2CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(e.RowIndex != -1)
			{
				dataGridView2.CurrentCell = dataGridView2[e.ColumnIndex, e.RowIndex];		
				var ind = dataGridView2.CurrentRow.Index;
				string type = dataGridView2[4, ind].Value.ToString();
				//Правый щелчек
				if (e.Button == MouseButtons.Right)
				{
					if(type== "pc")
					{
						connectPC(dataGridView2[1, ind].Value.ToString(),true);//MessageBox.Show(dataGridView2[4, ind].Value.ToString());
					} else if(type == "terminal")
					{
						
						//rdpCreate(dataGridView2[1, ind].Value.ToString(), Convert.ToInt32(dataGridView2[2, ind].Value), false);
						//Process.Start("mstsc", @"U:\Tsearch.tmp");
						
						//Process.Start("mstsc /shadow:" + dataGridView2[1, ind].Value.ToString() + " -v " + Convert.ToInt32(dataGridView2[2, ind].Value) + " /noConsentPrompt");
						Process.Start("mstsc", @"/shadow:" + Convert.ToInt32(dataGridView2[2, ind].Value) + " -v " + dataGridView2[1, ind].Value.ToString() + " /noConsentPrompt");
					}
				//Левый Щелчек
				} else if(e.Button == MouseButtons.Left)
				{
					if(type== "pc")
					{
						connectPC(dataGridView2[1, ind].Value.ToString(),false);//MessageBox.Show(dataGridView2[4, ind].Value.ToString());
					} else if(type == "terminal")
					{
						Process.Start("mstsc", @"/shadow:" + Convert.ToInt32(dataGridView2[2, ind].Value) + " -v " + dataGridView2[1, ind].Value.ToString() + " /control /noConsentPrompt");
						//rdpCreate(dataGridView2[1, ind].Value.ToString(), Convert.ToInt32(dataGridView2[2, ind].Value), true);
						//Process.Start("mstsc", @"U:\Tsearch.tmp");
					}
				}
			}
		}
		//void FdfToolStripMenuItem1Click(object sender, EventArgs e)
		//{
	
		//}

		void СписокСерверовToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			
			
			ServerStatusForm serverStatusForm = new ServerStatusForm();
			
			serverStatusForm.serverBase = serverBase;
			serverStatusForm.refresh();
			serverStatusForm.ShowDialog();
			//serverStatusForm.Show();
			serverStatusForm.refresh();
			//serverStatusForm
			
		}
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			Graphics gr = e.Graphics;
            Pen p = new Pen(Color.Black, 1);// цвет линии и ширина
            Point p1 = new Point(1,24);// первая точка
            Point p2 = new Point(this.Width-15,24);// вторая точка
            gr.DrawLine(p, p1, p2);// рисуем линию
            gr.Dispose();// освобождаем все ресурсы, связанные с отрисовкой
		}
		
		//Форма с настройками по нажатию пункта меню Настройки
		void НастройкиToolStripMenuItemClick(object sender, EventArgs e)
		{
			SettingsForm settingsForm = new SettingsForm();
			settingsForm.configList = configList;
			settingsForm.serverList = serverList;
			settingsForm.serverBase = serverBase;
			settingsForm.sessionList = sessionList;
			settingsForm.sessionBase = sessionBase;
			settingsForm.ShowDialog();
			//settingsForm.configList
		}
		void ToolStripComboBox1Click(object sender, EventArgs e)
		{
	
		}
		void ОтображатьОтключенныхToolStripMenuItemClick(object sender, EventArgs e)
		{	
			if(отображатьОтключенныхToolStripMenuItem.Checked)
			{
				отображатьОтключенныхToolStripMenuItem.Checked = false;
			} else
			{
				отображатьОтключенныхToolStripMenuItem.Checked = true;
			}
			filterDataGridView(textBox2.Text);
		}

		void ИмяКлиентаToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(имяКлиентаToolStripMenuItem.Checked)
			{
				имяКлиентаToolStripMenuItem.Checked = false;
				dataGridView2.Columns["clientName"].Visible = false;

			} else
			{
				имяКлиентаToolStripMenuItem.Checked = true;
				dataGridView2.Columns["clientName"].Visible = true;
			}
		}
		
		void ВремяОтключенияToolStripMenuItemClick(object sender, EventArgs e)
		{
				if(времяОтключенияToolStripMenuItem.Checked)
			{
				времяОтключенияToolStripMenuItem.Checked = false;
				dataGridView2.Columns["disconnectTime"].Visible = false;

			} else
			{
				времяОтключенияToolStripMenuItem.Checked = true;
				dataGridView2.Columns["disconnectTime"].Visible = true;
			}
		}
		
		void ВремяПодключенияToolStripMenuItemClick(object sender, EventArgs e)
		{
				if(времяПодключенияToolStripMenuItem.Checked)
			{
				времяПодключенияToolStripMenuItem.Checked = false;
				dataGridView2.Columns["connectTime"].Visible = false;

			} else
			{
				времяПодключенияToolStripMenuItem.Checked = true;
				dataGridView2.Columns["connectTime"].Visible = true;
			}
		}
		void РедактированиеСпискаСерверовToolStripMenuItemClick(object sender, EventArgs e)
		{
			configEditor = new ConfigEditor();
			configEditor.configEdit();
		}
		void ОбновлениеСпискаСерверовСДискаToolStripMenuItemClick(object sender, EventArgs e)
		{
						//System.Diagnostics.Process.
			configList.createConfigList();
			serverList.createServerList(configList.configList);
			serverBase.fillingTable(serverList.serverList);
			ArrayList serverDeleted = serverBase.clearServerListDB(serverList);
			foreach(string serverName in serverDeleted)
			{
				sessionList.removeAllServerSession(serverName);
				sessionBase.removeAllServerSession(serverName);
				//sessionList. . removeAllServerSession
			}
		}
		void ИнформацияToolStripMenuItemClick(object sender, EventArgs e)
		{
			informationForm.ShowDialog();
		}
		void ОПрограммеToolStripMenuItemClick(object sender, EventArgs e)
		{
			aboutForm.ShowDialog();
		}



	}
}
