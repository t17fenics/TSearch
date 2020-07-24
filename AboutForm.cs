/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 13.07.2020
 * Время: 16:18
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of AboutForm.
	/// </summary>
	public partial class AboutForm : Form
	{
		public bool formStarted = false;
		public AboutForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		//Открытие ссылки при щелчке
		void LinkLabel1Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/danports/cassia");
		}
		void AboutFormDeactivate(object sender, EventArgs e)
		{
			formStarted = false;
		}
	}
}
