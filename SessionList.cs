/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 07.07.2020
 * Время: 11:23
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Cassia;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of Session.
	/// </summary>
	public class SessionList
	{
		public ArrayList allSessionList = new ArrayList();
		
		public bool listLocked;

		public SessionList()
		{
		}
		
		// Функция заполняет лист сессиями
		public void fillingSessionList(ArrayList serverList)
		{
			listLocked = true;
			
			foreach(WTSServer server in serverList)
			{
				//Воткнуть сюда try
				try
				{
					foreach(ITerminalServicesSession session in server.serverSessionList)
					{
						int rowID = searchSession(session.UserName.ToLower(), server.serverName);
						if(rowID < 0) {
							UserSession userSession = new UserSession();
							userSession.userName = session.UserName.ToLower();
							userSession.serverName = session.Server.ServerName;
							userSession.sessionID = session.SessionId.ToString();
							userSession.status = session.ConnectionState.ToString();
							userSession.type = server.serverType;
							userSession.clientName = session.ClientName;
							userSession.connectTime = session.LoginTime.ToString();
							userSession.disconnectTime = session.DisconnectTime.ToString();
							allSessionList.Add(userSession);
						} else {
							UserSession userSession = (UserSession)allSessionList[rowID];
							userSession.sessionID = session.SessionId.ToString();
							userSession.status = session.ConnectionState.ToString();
						}
					}
				}
				catch(Exception ex)
				{
					Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name);
					Debug.WriteLine(ex.Message);
				}
			}

			listLocked = false;
		}
		
		//Функция поиска сессии по имени пользователя и имени сервера
		public int searchSession(string userName, string serverName)
		{
			foreach(UserSession session in allSessionList)
			{
				if(session.userName == userName && session.serverName == serverName)
				{
					return allSessionList.IndexOf(session);
				}
			}
			return -1;
		}
		
		//Функция удаляющая строки из allSessionList
		public UserSession setServerDeleted(WTSServer server)
		{

			//Получаем нужную userSession с заданного сервера
			UserSession userSession = serachWTSSession(server);
			//Если получено какое то значенеи
			if(userSession != null) 
			{
				Debug.WriteLine("Удаляем " + userSession.userName + " с сервера " + server.serverName);
				//удаляем эту строку из allSessionList
				//Debug.WriteLine("Удаляем из sessionList");
				//allSessionList.Remove(userSession);
				userSession.deleted = "deleted";

			}
			return userSession;
		}
		
		//Функция удаляющая строки из allSessionList
		public void removeSession()
		{
			ArrayList indexList = new ArrayList();
			foreach(UserSession session in allSessionList)
			{
				if(session.deleted == "deleted")
				{
					indexList.Add(allSessionList.IndexOf(session));
					//Debug.WriteLine("добавляем " + allSessionList.IndexOf(session));
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				//Debug.WriteLine(allSessionList.Count);
				allSessionList.RemoveAt(i);
				//Debug.WriteLine("удаляем " + i);
				//Debug.WriteLine(allSessionList.Count);

			}

			
		}	
		public void removeAllServerSession(string serverName)
		{
			ArrayList indexList = new ArrayList();
			foreach(UserSession session in allSessionList)
			{
				if(session.serverName == serverName)
				{
					indexList.Add(allSessionList.IndexOf(session));
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				Debug.WriteLine(allSessionList.Count);
				Debug.WriteLine("Удаляем " + i);
				allSessionList.RemoveAt(i);
			}
		}
		
		public UserSession serachWTSSession(WTSServer server)
		{

			try
			{
			foreach(UserSession session in allSessionList)
			{
				if(session.serverName == server.serverName)
				{
					if(!server.findSession(session.userName))
					{
						return session;
					}
				}
				
			}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name);
				Debug.WriteLine(ex.Message);
			}
			return null;

		}

	}
}
