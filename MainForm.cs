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
		//Путь к DameWare
		string DameWarePatch;
		
		public static Log log = new Log();
		
		bool firstRun = true;
		
		//Создаем объекты
		//Конфигурационный лист
		ConfigList configList = new ConfigList();
		ServerList serverList = new ServerList(log);
		SessionList sessionList = new SessionList(log);
		ServerBase serverBase = new ServerBase();
		SessionBase sessionBase = new SessionBase(log);
		ITerminalServicesManager manager = new TerminalServicesManager();
		
		
		ServerStatusForm serverStatusForm = new ServerStatusForm();
		InformationForm informationForm = new InformationForm();
		AboutForm aboutForm = new AboutForm();
			
		
		globalKeyboardHook gkh = new globalKeyboardHook();
		
		//Создаем объекты таймер
        //для обновления отображаемых данных
		System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
		//для обновления данных с серверов
		System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
		
		System.Windows.Forms.Timer timer3 = new System.Windows.Forms.Timer();
		
		public MainForm()
		{
			InitializeComponent();
				
			//Задаем переменную пути к DameWare, определяя его через функцию getDameWarePath();
			DameWarePatch = getDameWarePath();
			
			//Включаеем в меню пункт ВключитьАвтоматическоеОбновление
			ВключитьАвтоматическоеОбновлениеToolStripMenuItem.Checked = true;
			быстрыйДоступПоНажатиюPauseToolStripMenuItem.Checked = true;
			//Настраиваем поле statusStrip
			toolStripStatusLabel1.Text = "Автоматическое обновление включено";	
			toolStripStatusLabel2.Text = "Найдено сеансов: " + sessionList.allSessionList.Count;

			this.ShowInTaskbar = false; 
			notifyIcon1.Visible = true;
			
			//Создаем configList из текстового файла srvlst.ini
			configList.createConfigList();
			
			//Создаем serverList из configList
			serverList.createServerList(configList.configList);
			
			//Настраиваем Datasource и видимость колонок dataGridView2
			dataGridView2.DataSource = sessionBase.sessionListDB;
			dataGridView2.Columns["clientName"].Visible = false;
			dataGridView2.Columns["connectTime"].Visible = false;
			dataGridView2.Columns["disconnectTime"].Visible = false;
			
			//Включаем двойную буфферизацию для datagrid. Избавляет от подергиваний таблицы при обновлении
			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView2, new object[] { true });
			//Фильтация грида по полю ввода и настройке отображения disconnected
			filterDataGridView();
			
			
			gkh.HookedKeys.Add(Keys.Pause);
			try
			{
				gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
			}
			catch(Exception ex)
			{
			    Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + MethodBase.GetCurrentMethod().Name);
				Debug.WriteLine(ex.Message);
				log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
				MessageBox.Show("Не стоит так интенсивно сворачивать окно и нажимать Pause");
			}
   			
			//Запускаем поток с проверкой всех серверов
			Thread myThread = new Thread(checkServers);
			myThread.Start();
			
			//Настраиваем таймеры и запускаем их
			timer1.Interval = 500;
			timer1.Tick += new EventHandler(refrashData);
            timer1.Start();
           	timer2.Interval = 2000;
			timer2.Tick += new EventHandler(threadUpdateTerminalStatus);
            timer2.Start();
            timer3.Interval = 100;
			timer3.Tick += new EventHandler(clearSelection);
            timer3.Start();
            dataGridView2.ClearSelection();
       	}
		
		void clearSelection(object sender,EventArgs e)
		{
			if(dataGridView2.SelectedRows.Count == 0) dataGridView2.ClearSelection();
		}
		//Функция обновляет данные в листах и таблицах
		void refrashData(object sender,EventArgs e)
		{
			// Внесение данных из serverList в serverListDB
			serverBase.fillingTable(serverList.serverList);
			//Заполенние листа сеансами
			sessionList.fillingSessionList(serverList.serverList);
			//Внесение данных из sessionList в sessionListDB
			sessionBase.fillingTable(sessionList.allSessionList);
			//очистка листа и базы от удаленных сеансов
			sessionList.removeDeletedSession();
			sessionBase.removeDeletedRow();
			if(firstRun && sessionBase.sessionListDB.Rows.Count > 0)
			{
				firstRun = false;
				dataGridView2.ClearSelection();
			}
		}
		
		//Функция создающая поток, в котором обновляются статусы серверов и сессий
		void threadUpdateTerminalStatus(object sender,EventArgs e)
		{
			Thread myThread = new Thread(checkServers);
			myThread.Start();
			toolStripStatusLabel2.Text = "Найдено сеансов: " + sessionList.allSessionList.Count;
		}
		
		//Функция опрашивает все хосты из конфига. Если хост доступен, с него запрашивается список сессий и добавляется в общий лист сессий. Так же ищутся и помечаются удаленные сессии.
		public void checkServers()
		{
			try
			{
				foreach(WTSServer server in serverList.serverList)
				{
					Thread myThread = new Thread(server.checkServerStatus);
					myThread.Start();
					sessionList.serachDeletedSession(server);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Ошибка в методе" + MethodBase.GetCurrentMethod().Name);
				Debug.WriteLine(ex);
				log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
			}
		}
							
		//Фильтация грида по полю ввода и настройке отображения disconnected
		public void filterDataGridView()
		{
			var filters = new List<string>();
			if(отображатьОтключенныхToolStripMenuItem.Checked)
			{
				//отображатьОтключенныхToolStripMenuItem.Checked = true;
				filters.Add(string.Format("userName like '%{0}%'", textBox2.Text));
				sessionBase.sessionListDB.DefaultView.RowFilter = string.Join(" AND ", filters);
			}
			else
			{
				filters.Add(string.Format("status like '%{0}%'", "Active"));
				filters.Add(string.Format("userName like '%{0}%'", textBox2.Text));
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
		
		//функция прослушивания клавиатуры
		void gkh_KeyUp(object sender, KeyEventArgs e)
		{
    		if (e.KeyCode == Keys.Pause && быстрыйДоступПоНажатиюPauseToolStripMenuItem.Checked)
    		{
    			try{
    				Show();
	    			WindowState = FormWindowState.Normal;
    			}
    			catch(Exception ex)
    			{
    				Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + MethodBase.GetCurrentMethod().Name);
					Debug.WriteLine(ex.Message);
					log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
					MessageBox.Show("Не стоит так интенсивно сворачивать окно и нажимать Pause");
    			}
    		}
		}
	

		
		//---------------------------------------------------------------------------------------------

		//Управление интерфейсом
		
		// Событие изменения формы, для динамического сдвига элементов
		void MainFormResize(object sender, EventArgs e)
		{
			this.Refresh();
			label1.Left = this.Width/2-204;
			textBox2.Left = this.Width/2+58;
			if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
    			this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
            }
		}
		
		//Событие отслеживающее изменение TextBox2
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			filterDataGridView();
			dataGridView2.ClearSelection();
		}
		
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
						connectPC(dataGridView2[1, ind].Value.ToString(),true);
					} else if(type == "terminal")
					{
						Process.Start("mstsc", @"/shadow:" + Convert.ToInt32(dataGridView2[2, ind].Value) + " -v " + dataGridView2[1, ind].Value.ToString() + " /noConsentPrompt");
					}
				//Левый Щелчек
				} else if(e.Button == MouseButtons.Left)
				{
					if(type== "pc")
					{
						connectPC(dataGridView2[1, ind].Value.ToString(),false);
					} else if(type == "terminal")
					{
						Process.Start("mstsc", @"/shadow:" + Convert.ToInt32(dataGridView2[2, ind].Value) + " -v " + dataGridView2[1, ind].Value.ToString() + " /control /noConsentPrompt");
					}
				}
			}
		}

		//События на нажатие клавиш в datagridview
		private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
			Debug.WriteLine(e.KeyValue);
            if(e.KeyData == Keys.Enter)
            {          
	            var ind = dataGridView2.CurrentRow.Index;
				string type = dataGridView2[4, ind].Value.ToString();
	            if(type== "pc")
				{
					connectPC(dataGridView2[1, ind].Value.ToString(),true);
				} else if(type == "terminal")
				{
					Process.Start("mstsc", @"/shadow:" + Convert.ToInt32(dataGridView2[2, ind].Value) + " -v " + dataGridView2[1, ind].Value.ToString() + " /control /noConsentPrompt");
	            }
            } else if(e.KeyValue >= 48 && e.KeyValue <= 57 || e.KeyValue >= 65 && e.KeyValue <= 90)// || (e.KeyChar >=65 && e.e.KeyChar <=90))
	        {
            	textBox2.Text += Convert.ToChar(e.KeyValue).ToString().ToLower();
            	textBox2.Focus();
            	textBox2.SelectionStart = textBox2.Text.Length;
            	textBox2.SelectionLength = 0;
            	dataGridView2.ClearSelection();
            } else if(e.KeyValue == 8)
            {
            	textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);
            	textBox2.Focus();
            	textBox2.SelectionStart = textBox2.Text.Length;
            	textBox2.SelectionLength = 0;
            } else if(e.KeyData == (Keys.Enter | Keys.Control))
            {
            	Debug.WriteLine("fuck");
            		            var ind = dataGridView2.CurrentRow.Index;
				string type = dataGridView2[4, ind].Value.ToString();
	            if(type== "pc")
				{
					connectPC(dataGridView2[1, ind].Value.ToString(),false);
				} else if(type == "terminal")
				{
					Process.Start("mstsc", @"/shadow:" + Convert.ToInt32(dataGridView2[2, ind].Value) + " -v " + dataGridView2[1, ind].Value.ToString() + " /noConsentPrompt");
	            }
	        }
            return;
        }
		//Событие пункта меню СписокСерверовToolStripMenuItem
		void СписокСерверовToolStripMenuItemClick(object sender, EventArgs e)
		{
			serverStatusForm.formStarted = true;
			serverStatusForm.serverBase = serverBase;
			serverStatusForm.refresh();
			serverStatusForm.ShowDialog();
		}
		
		//Событие пункта меню ОтображатьОтключенныхToolStripMenuItem
		void ОтображатьОтключенныхToolStripMenuItemClick(object sender, EventArgs e)
		{	
			if(отображатьОтключенныхToolStripMenuItem.Checked)
			{
				отображатьОтключенныхToolStripMenuItem.Checked = false;
			} else
			{
				отображатьОтключенныхToolStripMenuItem.Checked = true;
			}
			filterDataGridView();
		}

		//Событие пункта меню ИмяКлиентаToolStripMenuItem
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
		
		//Событие пункта меню ВремяОтключенияToolStripMenuItem
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
		
		//Событие пункта меню ВремяПодключенияToolStripMenuItem
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
		
		//Событие пункта меню РедактированиеСпискаСерверовToolStripMenuItem
		void РедактированиеСпискаСерверовToolStripMenuItemClick(object sender, EventArgs e)
		{
			ConfigEditor configEditor = new ConfigEditor();
			configEditor.configEdit();
		}
		
		//Событие пункта меню ОбновлениеСпискаСерверовСДискаToolStripMenuItem
		void ОбновлениеСпискаСерверовСДискаToolStripMenuItemClick(object sender, EventArgs e)
		{
			configList.createConfigList();
			serverList.createServerList(configList.configList);
			serverBase.fillingTable(serverList.serverList);
			ArrayList serverDeleted = serverBase.clearServerListDB(serverList);
			foreach(string serverName in serverDeleted)
			{
				sessionList.removeAllServerSession(serverName);
				sessionBase.removeAllServerSession(serverName);
			}
			toolStripStatusLabel2.Text = "Найдено сеансов: " + sessionList.allSessionList.Count;
		}
		
		//Событие пункта меню ИнформацияToolStripMenuItem
		void ИнформацияToolStripMenuItemClick(object sender, EventArgs e)
		{
			informationForm.formStarted = true;
			informationForm.ShowDialog();
		}
		
		//Событие пункта меню ОПрограммеToolStripMenuItem
		void ОПрограммеToolStripMenuItemClick(object sender, EventArgs e)
		{
			aboutForm.formStarted = true;
			aboutForm.ShowDialog();
		}

		//Событие пункта меню ВключитьАвтоматическоеОбновлениеToolStripMenuItem
		void ВключитьАвтоматическоеОбновлениеToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(ВключитьАвтоматическоеОбновлениеToolStripMenuItem.Checked)
			{
				ВключитьАвтоматическоеОбновлениеToolStripMenuItem.Checked = false;
				timer1.Stop();
				timer2.Stop();
				toolStripStatusLabel1.Text = "Автоматическое обновление отключено";
			} else
			{
				ВключитьАвтоматическоеОбновлениеToolStripMenuItem.Checked = true;
				timer1.Start();
				timer2.Start();
				toolStripStatusLabel1.Text = "Автоматическое обновление включено";
			}
		}
		
		//Событие щелчка по иконке у часов
		void NotifyIcon1Click(object sender, MouseEventArgs e)
		{
			Show();
			WindowState = FormWindowState.Normal;
			if(e.Button == MouseButtons.Right)
			{
				Left = Screen.PrimaryScreen.WorkingArea.Width/2 - Width/2;
				Top = Screen.PrimaryScreen.WorkingArea.Height/2 - Height/2;
			}
		}

		//События на нажатие клавиш в textbox2
		private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
			if(e.KeyData!=Keys.Enter && e.KeyData!=Keys.Down && e.KeyData!=Keys.Up) return;
            if(dataGridView2.CurrentRow != null) 
            {
            	dataGridView2.Focus();
            	dataGridView2.FirstDisplayedCell.Selected = true;
            	dataGridView2.CurrentRow.Selected = true;
            }
        }
		
		//При входе в textbox2 убираем выделение datagridview
		void TextBox2Enter(object sender, EventArgs e)
		{
			dataGridView2.ClearSelection();
		}
		
		//Событие при сворачивании формы
		void MainFormDeactivate(object sender, EventArgs e)
		{
			if(!serverStatusForm.formStarted && !aboutForm.formStarted && !informationForm.formStarted)
			{
	    		Hide();
	    		WindowState = FormWindowState.Minimized;
	    		
    			if(notifyIcon1.Text != "")
    			{
    				notifyIcon1.Visible = true;
    			} else {
    				Application.Exit();
    			}
    		}
		}
		
		//Событие пункта меню МинимальныйРазмерToolStripMenuItem
		void МинимальныйРазмерToolStripMenuItemClick(object sender, EventArgs e)
		{
			Width = 438;
			Height = 200;
			Left = Screen.PrimaryScreen.WorkingArea.Width - 438;
			Top = Screen.PrimaryScreen.WorkingArea.Height-200;
		}
		
		//Событие пункта меню БыстрыйДоступПоНажатиюPauseToolStripMenuItem
		void БыстрыйДоступПоНажатиюPauseToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(быстрыйДоступПоНажатиюPauseToolStripMenuItem.Checked)
			{
				быстрыйДоступПоНажатиюPauseToolStripMenuItem.Checked = false;
			} else
			{
				быстрыйДоступПоНажатиюPauseToolStripMenuItem.Checked = true;
			}
		}
	}
}