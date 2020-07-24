/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 22.07.2020
 * Время: 15:30
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of Log.
	/// </summary>
	public class Log
	{
		string logFile = @"\log" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt";
		
		public Log()
		{
			Debug.WriteLine(logFile);
		}
		
		public void appendLog(string message) {
			
 			//File.AppendAllText(Application.StartupPath+logFile, message+"\n");
		}
	}
}
