/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 02.09.2019
 * Время: 15:46
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using Cassia;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of WTSServer.
	/// </summary>
	public class WTSServer
	{
		//Переменная обозначающая статус сервера
		public string serverStatus;
		
		//Пустая функция
		public WTSServer()
		{
			
		}

		//Возвращает список сеансов с сервера(имя сервера в качестве параметра)
		public List<ITerminalServicesSession> getSessionList(string serv)
		{
			//Создание листа, в который сохраняются сеансы, полученные в этом методе
			List<ITerminalServicesSession> sessionList = new List<ITerminalServicesSession>{};
			
			
			//Далее идет код для подключения к серверу https://github.com/danports/cassia
			ITerminalServicesManager manager = new TerminalServicesManager();
			using (ITerminalServer server = manager.GetRemoteServer(serv))
			{
				server.Open();

				//try
				//{
					// Перебираем все сеансы на сервере(Почему то выполняется в try)
					foreach (ITerminalServicesSession session in server.GetSessions())
					{
						//Фильтруем системный сеанс с Id 0. Отбираем сеансы в статусе Active или в статусе Disconnected, но не с пустым именем
						if(session.SessionId != 0 && ((session.ConnectionState == Cassia.ConnectionState.Active) || (session.ConnectionState == Cassia.ConnectionState.Disconnected && session.UserName != "")))
						{
							//Добавляем походящий сеанс в лист для сеансов sessionList
							sessionList.Add(session);
							
						}
						
						/*//Отбираем сеансы в статусе Active. Так же фильтруем системный сеанс с Id 0
						if(session.ConnectionState == Cassia.ConnectionState.Active && session.SessionId != 0)
						{
							//Добавляем походящий сеанс в лист для сеансов sessionList
							sessionList.Add(session);
							
						} else if (session.ConnectionState == Cassia.ConnectionState.Disconnected && session.SessionId != 0 && session.UserName != "")
						{
							//Добавляем походящий сеанс в лист для сеансов sessionList
							sessionList.Add(session);
						}*/
					}
				//}
				//catch (Exception ex)	
				//{
					//MessageBox.Show(ex.Message);//server.ServerName);
				//	serverStatus = ex.Message ;
				//}

			}
			return sessionList;
		}
	}
	
}
