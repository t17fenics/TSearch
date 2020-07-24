/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 10.07.2020
 * Время: 13:38
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System.Drawing;
namespace TSearch_v0._1
{
	partial class ServerStatusForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn ServerName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ServerType;
		private System.Windows.Forms.DataGridViewTextBoxColumn ServerStatus;
		private System.Windows.Forms.DataGridViewTextBoxColumn SessionCount;
		private System.Windows.Forms.DataGridViewTextBoxColumn lastChecking;

		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
					
				
				private void InitializeComponent()
				{
					System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
					System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
					System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
					System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
					System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerStatusForm));
					this.dataGridView1 = new System.Windows.Forms.DataGridView();
					this.ServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
					this.ServerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
					this.ServerStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
					this.SessionCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
					this.lastChecking = new System.Windows.Forms.DataGridViewTextBoxColumn();
					((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
					this.SuspendLayout();
					// 
					// dataGridView1
					// 
					this.dataGridView1.AllowUserToAddRows = false;
					this.dataGridView1.AllowUserToDeleteRows = false;
					this.dataGridView1.AllowUserToResizeColumns = false;
					this.dataGridView1.AllowUserToResizeRows = false;
					this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
					this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
					dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
					dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
					dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
					dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
					dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
					dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
					dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
					this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
					this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
					this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.ServerName,
			this.ServerType,
			this.ServerStatus,
			this.SessionCount,
			this.lastChecking});
					this.dataGridView1.Location = new System.Drawing.Point(1, 1);
					this.dataGridView1.MultiSelect = false;
					this.dataGridView1.Name = "dataGridView1";
					this.dataGridView1.ReadOnly = true;
					this.dataGridView1.RowHeadersVisible = false;
					this.dataGridView1.Size = new System.Drawing.Size(665, 265);
					this.dataGridView1.TabIndex = 0;
					// 
					// ServerName
					// 
					this.ServerName.DataPropertyName = "ServerName";
					dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
					this.ServerName.DefaultCellStyle = dataGridViewCellStyle2;
					this.ServerName.FillWeight = 65.65144F;
					this.ServerName.HeaderText = "Имя сервера";
					this.ServerName.MinimumWidth = 110;
					this.ServerName.Name = "ServerName";
					this.ServerName.ReadOnly = true;
					// 
					// ServerType
					// 
					this.ServerType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
					this.ServerType.DataPropertyName = "ServerType";
					dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
					this.ServerType.DefaultCellStyle = dataGridViewCellStyle3;
					this.ServerType.FillWeight = 65.65144F;
					this.ServerType.HeaderText = "Тип сервера";
					this.ServerType.Name = "ServerType";
					this.ServerType.ReadOnly = true;
					this.ServerType.Width = 110;
					// 
					// ServerStatus
					// 
					this.ServerStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
					this.ServerStatus.DataPropertyName = "ServerStatus";
					this.ServerStatus.FillWeight = 65.65144F;
					this.ServerStatus.HeaderText = "Статус сервера";
					this.ServerStatus.Name = "ServerStatus";
					this.ServerStatus.ReadOnly = true;
					this.ServerStatus.Width = 190;
					// 
					// SessionCount
					// 
					this.SessionCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
					this.SessionCount.DataPropertyName = "SessionCount";
					dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
					this.SessionCount.DefaultCellStyle = dataGridViewCellStyle4;
					this.SessionCount.FillWeight = 203.0457F;
					this.SessionCount.HeaderText = "Сеансы";
					this.SessionCount.Name = "SessionCount";
					this.SessionCount.ReadOnly = true;
					this.SessionCount.Width = 70;
					// 
					// lastChecking
					// 
					this.lastChecking.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
					this.lastChecking.DataPropertyName = "LastCheck";
					this.lastChecking.HeaderText = "Последняя проверка";
					this.lastChecking.MinimumWidth = 160;
					this.lastChecking.Name = "lastChecking";
					this.lastChecking.ReadOnly = true;
					this.lastChecking.Width = 160;
					// 
					// ServerStatusForm
					// 
					this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
					this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
					this.AutoScroll = true;
					this.ClientSize = new System.Drawing.Size(666, 267);
					this.Controls.Add(this.dataGridView1);
					this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
					this.MaximizeBox = false;
					this.MinimizeBox = false;
					this.Name = "ServerStatusForm";
					this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
					this.Text = "Статус серверов";
					this.Deactivate += new System.EventHandler(this.ServerStatusFormDeactivate);
					((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
					this.ResumeLayout(false);

				}
	}
}
