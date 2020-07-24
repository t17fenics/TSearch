/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 13.07.2020
 * Время: 3:24
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of ConfigEditor.
	/// </summary>
	public class ConfigEditor
	{
		public ConfigEditor()
		{
		}
		
		//
		public void configEdit()
		{
			try
			{
			string path = Application.StartupPath+@"\srvlst.ini";
			Process newProcess = Process.Start(path);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + MethodBase.GetCurrentMethod().Name);
				Debug.WriteLine(ex.Message);
				//log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
			}
		}
	}
}
