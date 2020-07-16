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
		
		public WTSServer()
		{
		}
		
		//Возвращает список сеансов с сервера(имя сервера в качестве параметра)
		public void getSessionList()
		{
			Thread myThread = new Thread(getSessionListIn);
			myThread.Start();
		}
		public void getSessionList1()
		{
			//Debug.WriteLine("Получаем сессии с сервера " + serverName);
			IList<ITerminalServicesSession> serverSession;
			
			int countSession = 0;
			serverSessionList.Clear();
			
			//Далее идет код для подключения к серверу 
			ITerminalServicesManager manager = new TerminalServicesManager();
			using (ITerminalServer server = manager.GetRemoteServer(serverName))
			{
				server.Open();
				// Перебираем все сеансы на сервере(Почему то выполняется в try)
				try
				{

					serverSession = server.GetSessions();
				}
				catch(Exception ex)
				{
					//Debug.WriteLine(ex.Message);
					serverStatus = ex.Message;
					serverSession = null;
				}
				server.Close();
				if(serverSession != null)
				{
				foreach (ITerminalServicesSession session in serverSession)
				{
					//server.
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
			//return sessionCount;
		}
		public void getSessionListIn()
		{
			IList<ITerminalServicesSession> serverSession;
			
			int countSession = 0;
			serverSessionList.Clear();
			
			//Далее идет код для подключения к серверу 
			ITerminalServicesManager manager = new TerminalServicesManager();
			using (ITerminalServer server = manager.GetRemoteServer(serverName))
			{
				server.Open();
				// Перебираем все сеансы на сервере(Почему то выполняется в try)
				try
				{

					serverSession = server.GetSessions();
				}
				catch(Exception ex)
				{
					//Debug.WriteLine(ex.Message);
					serverStatus = ex.Message;
					serverSession = null;
				}
				if(serverSession != null)
				{
				foreach (ITerminalServicesSession session in serverSession)
				{
					//Фильтруем системный сеанс с Id 0. Отбираем сеансы в статусе Active или в статусе Disconnected, но не с пустым именем
					if(session.SessionId != 0 && ((session.ConnectionState == Cassia.ConnectionState.Active) || (session.ConnectionState == Cassia.ConnectionState.Disconnected && session.UserName != "")))
					{
						serverSessionList.Add(session);
						countSession++;
						
					}
				}
				}

			}
			sessionCount = countSession;
			//return sessionCount;
		}
		//Проверка доступности серера с помощью ping
		public string checkServerStatus()
		{
			string status;
			Ping ping = new Ping();
            PingReply pingReply = null;
            try
			{
				pingReply = ping.Send(serverName,150);
				status = pingReply.Status.ToString();
			}
			catch (Exception ex)
			{
				if(ex.InnerException.Message != "Этот хост неизвестен")
				{
					Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
					Debug.WriteLine(ex.InnerException.Message);
				}
					status = ex.InnerException.Message;
				
			}
			/*if(status == "Success" || status == "TimedOut" ||  status == "Этот хост неизвестен")
			{

			} else {
							Debug.WriteLine("status" + status);
			}*/
			
			serverStatus=status;
			return status;

		}
		
		public bool findSession(string userName)
		{
			foreach(ITerminalServicesSession session in serverSessionList)
			{
				
				if(session.UserName.ToLower() == userName.ToLower())
				{
					
					return true;
				} 
			}
			//Debug.WriteLine("Не нашли " + userName + " в " + serverName);
			return false;
		}
		
		public bool findServer(string serverName)
		{
			foreach(ITerminalServicesSession session in serverSessionList)
			{
				
				if(session.Server.ServerName == serverName)
				{
					
					return true;
				} 
			}
			//Debug.WriteLine("Не нашли " + userName + " в " + serverName);
			return false;
		}
		
		
		//Получение всех сеансов из serverSessionList
		public void printAllSession()
		{
			foreach(ITerminalServicesSession session in serverSessionList)
			{
				Debug.WriteLine(session.SessionId + " " + session.UserName);
			}
		}
	}
}
