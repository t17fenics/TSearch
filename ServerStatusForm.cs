/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 10.07.2020
 * Время: 13:38
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of ServerStatusForm.
	/// </summary>
	public partial class ServerStatusForm : Form
	{
		public ServerBase serverBase;
		public bool formStarted = false;
		public ServerStatusForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//Включение двойной буфферизации
			typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });
		}
		
		public void refresh()
		{
			dataGridView1.DataSource = serverBase.serverListDB;
		}
		void ServerStatusFormDeactivate(object sender, EventArgs e)
		{
			formStarted = false;
		}
	}
}
