/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 13.07.2020
 * Время: 14:29
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of ConfigList.
	/// </summary>
	public class ConfigList
	{
		public ArrayList configList = new ArrayList();
		
		public ConfigList()
		{
		}
		
		//Функция создания ConfigList из файла srvlst.ini
		public void createConfigList()
		{
			configList.Clear();
			string path = Application.StartupPath+@"\srvlst.ini";
			try
			{
				using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
	    		{
	        		string line = sr.ReadLine();
	        		
	        		while (line != null)
	        		{
	     	 			ConfigLine configLine = new ConfigLine();
	        			string[] words = line.Split(new char[] { '	' });
	        			if(words.Length == 2)
	        			{
	        				if(words[1] != "terminal" && words[1] != "pc")
	        				{
	        					MessageBox.Show("Исправьте ошибки в списке серверов. В строкe " + (configList.Count + 1) + " ошибка в записи 'Тип сервера'.");
	        					break;
	        				}
	        				configLine.serverName = words[0];
	        				configLine.serverType = words[1];
	        				configList.Add(configLine);
	        				line = sr.ReadLine();
	        			} else
	        			{
	        				MessageBox.Show("Исправьте ошибки в списке серверов. Количество элементов в строке " + (configList.Count + 1) + " не равно двум.");
	        				break;
	        			}
	       			}
	        		sr.Close();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Ошибка открытия конфига. \n" + ex.Message);
			}
		}
	}
}
