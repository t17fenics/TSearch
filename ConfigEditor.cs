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
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of ConfigEditor.
	/// </summary>
	public class ConfigEditor
	{
				Process newProcess;
		public ConfigEditor()
		{
		}
		public void configEdit()
		{
		string path = Application.StartupPath+@"\settings.ini";
		newProcess = Process.Start(path);
		}
	}
}
