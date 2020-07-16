/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 06.07.2020
 * Время: 0:52
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class ServerList
	{
		public ArrayList serverList = new ArrayList();

		public ServerList()
		{
		}
		
		//Создаем serverList из configList
		public void createServerList(ArrayList configList)
		{
			serverList.Clear();
			foreach(ConfigLine line in configList)
			{
				WTSServer server = new WTSServer();
				server.serverName = line.serverName;
				server.serverType = line.serverType;
				serverList.Add(server);
			}
		}
		public int searchWTSbyName(string serverName)
		{
			foreach(WTSServer server in serverList)
			{
				if(server.serverName == serverName)

				{
					return serverList.IndexOf(server);
				}
			}
			return -1;
		}
	}
}
