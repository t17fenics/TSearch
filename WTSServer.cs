/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 02.09.2019
 * Время: 15:46
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Cassia;
using System.Collections.Generic;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of WTSServer.
	/// </summary>
	public class WTSServer
	{
		//Переменная обозначающая статус сервера
		public string serverStatus;
		public string serverName;
		public string serverType;
		public int sessionCount;
		public string lastCheck;
		//Создание листа, в который сохраняются сеансы, полученные в этом методе
		public 	List<ITerminalServicesSession> serverSessionList = new List<ITerminalServicesSession>{};
		Log log;
		
		public WTSServer(Log logMainForm)
		{
			log = logMainForm;
		}
		
		//Функция проверяет текущий хост. Если хост доступен и проходит по типу и задержке между сканированиями с него запрашивается список сессий и добавляется в общий лист сессий.
		public void checkServerStatus()
		{
			long diffTime = 0;
			pingServer();
			if((serverStatus == "Success" || serverStatus == null))
			{
				if(serverStatus =="Success" && lastCheck != "retry")
				{
					DateTime old = Convert.ToDateTime(lastCheck);
					diffTime = (long)(DateTime.Now - old).TotalSeconds;
				}
				if((serverStatus =="Success" && serverType == "terminal" && diffTime >= 3) || (serverStatus =="Success" && serverType == "pc" && diffTime >= 5)) //&& server.lastCheck != null)
				{
					lastCheck = "retry";
					getSessionList();
				}
			}
		}
		
		//Проверка доступности серера с помощью ping
		public void pingServer()
		{
			Ping ping = new Ping();
            PingReply pingReply = null;
            try
			{
				pingReply = ping.Send(serverName,150);
				serverStatus = pingReply.Status.ToString();
			}
			catch (Exception ex)
			{
				if(ex.InnerException.Message != "Этот хост неизвестен")
				{
					Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
					Debug.WriteLine(ex.InnerException.Message);
					log .appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex.Message + "\n" + Application.UserAppDataPath);
				}
				serverStatus = ex.InnerException.Message;
			}
		}
		
		//Возвращает список сеансов с сервера(имя сервера в качестве параметра)
		public void getSessionList()
		{
			IList<ITerminalServicesSession> serverSession;
			int countSession = 0;
			
			serverSessionList.Clear();
			
			//Далее идет код для подключения к серверу 
			ITerminalServicesManager manager = new TerminalServicesManager();
			using (ITerminalServer server = manager.GetRemoteServer(serverName))
			{
				server.Open();
				
				// Перебираем все сеансы на сервере.
				try
				{
					serverSession = server.GetSessions();
				}
				catch(Exception ex)
				{
					serverStatus = ex.Message;
					serverSession = null;
					log.appendLog(MethodBase.GetCurrentMethod().ReflectedType.Name + "\n" + MethodBase.GetCurrentMethod().Name + "\n" + ex + "\n" + Application.UserAppDataPath);
				}
				server.Close();
				if(serverSession != null)
				{
					foreach (ITerminalServicesSession session in serverSession)
					{
						//Фильтруем системный сеанс с Id 0. Отбираем сеансы в статусе Active или в статусе Disconnected, но не с пустым именем
						if(session.SessionId != 0 && ((session.ConnectionState == ConnectionState.Active) || (session.ConnectionState == ConnectionState.Disconnected && session.UserName != "")))
						{
							serverSessionList.Add(session);
							countSession++;
						}
					}
				}
			}
			sessionCount = countSession;
			lastCheck = DateTime.Now.ToString();
		}
		
		//Поиск сессии в serverSessionList по ммени
		public bool findSession(string userName)
		{
			foreach(ITerminalServicesSession session in serverSessionList)
			{
				if(session.UserName.ToLower() == userName.ToLower())
				{
					return true;
				} 
			}
			return false;
		}
	}
}