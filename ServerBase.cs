/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 07.07.2020
 * Время: 15:46
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class ServerBase
	{
		public DataTable serverListDB = new DataTable();

		public ServerBase()
		{
			serverListDB.Columns.Add("ServerName");
			serverListDB.Columns.Add("ServerType");
			serverListDB.Columns.Add("ServerStatus");
			serverListDB.Columns.Add("SessionCount");
			serverListDB.Columns.Add("LastCheck");
		}
		
		// Внесение данных из serverList в serverListDB
		public void fillingTable(ArrayList  serverList)
		{
			//serverListDB.Clear();
			foreach(WTSServer server in serverList)
			{
				int rowID = searchWTSbyName(server.serverName);
				if(rowID < 0)
				{
					//serverListDB.Clear();
					DataRow dRow = serverListDB.NewRow();
					dRow["ServerName"] = server.serverName;
					dRow["ServerType"] = server.serverType;
					dRow["ServerStatus"] = server.serverStatus;
					dRow["sessionCount"] = server.sessionCount;
					dRow["sessionCount"] = server.lastCheck;
					serverListDB.Rows.Add(dRow);
				} else {
					DataRow dRow = serverListDB.Rows[rowID];
					dRow["ServerStatus"] = server.serverStatus;
					dRow["sessionCount"] = server.sessionCount;
					dRow["LastCheck"] = server.lastCheck;
				}
			}
		}
		
		public ArrayList clearServerListDB(ServerList  serverList)
		{
			ArrayList indexList = new ArrayList();
			ArrayList serverDeleted = new ArrayList();
			foreach(DataRow row in serverListDB.Rows)
			{
				int rowID = serverList.searchWTSbyName(row["serverName"].ToString());
				if(rowID == -1)
				{
					indexList.Add(serverListDB.Rows.IndexOf(row));
					serverDeleted.Add(row["serverName"].ToString());
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				Debug.WriteLine("перед удалением");
				serverListDB.Rows.RemoveAt(i);
				Debug.WriteLine("после удалением");
				//Debug.WriteLine("удаляем " + i);
				//Debug.WriteLine(allSessionList.Count);

			}
			return serverDeleted;
		}
		
		//Функция поиска сервера по имени сервера
		public int searchWTSbyName(string serverName)
		{
			foreach(DataRow row in serverListDB.Rows)
			{
				if(row["serverName"].ToString() == serverName)

				{

					return serverListDB.Rows.IndexOf(row);
				}
			}
			return -1;
		}
		public int getWTSSessionCount(string serverName)
		{
			foreach(DataRow row in serverListDB.Rows)
			{
				if(row["serverName"].ToString() == serverName)

				{
					return Int32.Parse(row["sessionCount"].ToString());
				}
			}
			return 0;
		}
		
		
	}
}
