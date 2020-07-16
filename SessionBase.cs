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

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of SessionBase.
	/// </summary>
	public class SessionBase
	{	
		public DataTable sessionListDB = new DataTable();
		//DateTime localDate = DateTime.Now;
		//string time;
		//DateTime dateValue = DateTime.Parse(localDate);
			                                    
		public SessionBase()
		{
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
				}
			}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name);
				Debug.WriteLine(ex.Message);
				
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
		
		public int serachSessionOnServer(string serverName, string sessionID)
		{
			foreach(DataRow row in sessionListDB.Rows)
			{
				if(row["serverName"].ToString() == serverName)
				{
					if(row["sessionID"].ToString() == sessionID)
					{
						return sessionListDB.Rows.IndexOf(row);
					}
				}
			}
			return -1;			
		}

		public void setRowDeleted(UserSession session)
		{
			if(session != null)
			{	
				try
				{
				int index =  serachSessionOnServer(session.serverName, session.sessionID);
				//Debug.WriteLine("Удаляем из sessionBase " + index);

					DataRow dRow = sessionListDB.Rows[index];
					dRow["deleted"] = session.deleted;
					//dRow["status"] = session.status;
				//sessionListDB.Rows.Remove(sessionListDB.Rows[index]);//DataRow row = serachSessionOnServer(session.serverName, session.sessionID);
				}
				catch(Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
				
			}
		}
		public void removeRow()
		{

	
			ArrayList indexList = new ArrayList();
			foreach(DataRow row in sessionListDB.Rows)
			{
				if(row["deleted"] == "deleted")
				{
					indexList.Add(sessionListDB.Rows.IndexOf(row));
					//Debug.WriteLine("добавляем " + sessionListDB.Rows.IndexOf(row));
				}
			}
			foreach(int i in indexList)
			{
				sessionListDB.Rows.RemoveAt(i);
				//Debug.WriteLine("удаляем " + i);

			}
			try
			{
			//int index =  serachSessionOnServer(session.serverName, session.sessionID);
			//Debug.WriteLine("Удаляем из sessionBase " + index);

				//DataRow dRow = sessionListDB.Rows[index];
				//dRow["deleted"] = session.deleted;
				//dRow["status"] = session.status;
			//sessionListDB.Rows.Remove(sessionListDB.Rows[index]);//DataRow row = serachSessionOnServer(session.serverName, session.sessionID);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}
				
		public void removeAllServerSession(string serverName)
		{
			ArrayList indexList = new ArrayList();
			foreach(DataRow row in sessionListDB.Rows)
			{
				if(row["serverName"] == serverName)
				{
					indexList.Add(sessionListDB.Rows.IndexOf(row));
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				//Debug.WriteLine(allSessionList.Count);
				Debug.WriteLine("Удаляем " + i);
				sessionListDB.Rows.RemoveAt(i);
				
			}
		}
	}
}
