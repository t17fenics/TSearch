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
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Cassia;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of Session.
	/// </summary>
	public class SessionList
	{
		public ArrayList allSessionList = new ArrayList();
		//MainForm form1 = this.
		//main
		Log log;

		public SessionList(Log logMainForm)
		{
			log = logMainForm;
		}
		
		// Функция заполняет лист сессиями
		public void fillingSessionList(ArrayList serverList)
		{
			//listLocked = true;
			
			foreach(WTSServer server in serverList)
			{
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
					Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name + " " + MethodBase.GetCurrentMethod().Name);
					Debug.WriteLine(ex.Message);
					log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
				}
			}
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
		
		//Функция помечающая deleted строки из allSessionList
		public void serachDeletedSession(WTSServer server)
		{
			try
			{
				foreach(UserSession session in allSessionList)
				{
					if(session.serverName == server.serverName && server.serverSessionList.Count != 0 && !server.findSession(session.userName))
					{
							session.deleted = "deleted";
					}
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(MethodBase.GetCurrentMethod().ReflectedType.Name  + " " + MethodBase.GetCurrentMethod().Name);
				Debug.WriteLine(ex.Message);
				log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
			}
		}

		//Функция удаляющая строки из allSessionList
		public void removeDeletedSession()
		{
			ArrayList indexList = new ArrayList();
			foreach(UserSession session in allSessionList)
			{
				if(session.deleted == "deleted")
				{
					indexList.Add(allSessionList.IndexOf(session));
				}
			}
			indexList.Reverse();
			foreach(int i in indexList)
			{
				allSessionList.RemoveAt(i);
			}
		}
		
		//удаление всех сеансов удаленного сервера из листа
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
				allSessionList.RemoveAt(i);
			}
		}
		
		public static void Log(string message) {
 			File.AppendAllText(Application.StartupPath+@"\log.txt", message+"\n");
		}
	}
}
