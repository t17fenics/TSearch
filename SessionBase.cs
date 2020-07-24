/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 07.07.2020
 * Время: 23:43
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of SessionBase.
	/// </summary>
	public class SessionBase
	{	
		public DataTable sessionListDB = new DataTable();
	    Log log;                               
		public SessionBase(Log logMainForm)
		{
			log = logMainForm;
			sessionListDB.Columns.Add("userName");
			sessionListDB.Columns.Add("serverName");
			sessionListDB.Columns.Add("sessionID");
			sessionListDB.Columns.Add("status");
			sessionListDB.Columns.Add("type");
			sessionListDB.Columns.Add("clientName");
			sessionListDB.Columns.Add("connectTime");
			sessionListDB.Columns.Add("disconnectTime");
			sessionListDB.Columns.Add("deleted");
		}	
		
		//Внесение данных из sessionList в sessionListDB
		public void fillingTable(ArrayList sessionList)
		{ 
			try
			{
				foreach(UserSession session in sessionList)
				{	
					int rowID = searchRow(session.serverName, session.sessionID);
					if(rowID < 0)
					{
						DataRow dRow = sessionListDB.NewRow();
						dRow["userName"] = session.userName;
						dRow["serverName"] =session.serverName;
						dRow["sessionID"] = session.sessionID;
						dRow["status"] = session.status;
						dRow["type"] = session.type;
						dRow["clientName"] = session.clientName;
						dRow["connectTime"] = session.connectTime;
						dRow["disconnectTime"] = session.disconnectTime;
						sessionListDB.Rows.Add(dRow);
					} else {
						DataRow dRow = sessionListDB.Rows[rowID];
						dRow["userName"] = session.userName;
						dRow["status"] = session.status;
						dRow["deleted"] = session.deleted;
					}
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + MethodBase.GetCurrentMethod().Name);
				Debug.WriteLine(ex.Message);
				log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
			}
		}
		
		//Поиск записи о сесии по имени пользователя и имени сервера
		public int searchRow(string serverName, string sessionID)
		{
			foreach(DataRow row in sessionListDB.Rows)
			{
				if(row["sessionID"].ToString() == sessionID && row["serverName"].ToString() == serverName)
				{
					return sessionListDB.Rows.IndexOf(row);
				}
			}
			return -1;
		}
		
		//очистка базы от строк помеченых как deleted
		public void removeDeletedRow()
		{
			ArrayList indexList = new ArrayList();
			foreach(DataRow row in sessionListDB.Rows)
			{
				if(row["deleted"].ToString() == "deleted")
				{
					indexList.Add(sessionListDB.Rows.IndexOf(row));
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				sessionListDB.Rows.RemoveAt(i);
			}
		}
		
		//Очистка базы от сессий с удаленног осервера		
		public void removeAllServerSession(string serverName)
		{
			ArrayList indexList = new ArrayList();
			foreach(DataRow row in sessionListDB.Rows)
			{
				if(row["serverName"].ToString() == serverName)
				{
					indexList.Add(sessionListDB.Rows.IndexOf(row));
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				sessionListDB.Rows.RemoveAt(i);
			}
		}
	}
}
