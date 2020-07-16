/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 10.07.2020
 * Время: 18:59
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of SettingsForm.
	/// </summary>
	public partial class SettingsForm : Form
	{
		public ConfigList configList;
		public ServerList serverList;
		public ServerBase serverBase;
		public SessionList sessionList;
		public SessionBase sessionBase;
		

		


		public SettingsForm()
		{

			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	
		void Button3Click(object sender, EventArgs e)
		{
			this.Close();
		}
		void Button4Click(object sender, EventArgs e)
		{
					ConfigEditor configEditor = new ConfigEditor();
			configEditor.configEdit();
  			//newProcess.Kill();
			//System.Diagnostics.Process.Start(path);
		}
		void Button1Click(object sender, EventArgs e)
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
		
	}
}
