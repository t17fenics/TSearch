/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 10.07.2020
 * Время: 18:59
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
namespace TSearch_v0._1
{
	partial class SettingsForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.TrackBar trackBar2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		
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
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.trackBar2 = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
			this.SuspendLayout();
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Items.AddRange(new object[] {
			"Тип сервера",
			"Имя клиента",
			"Время входа",
			"Время отключения"});
			this.checkedListBox1.Location = new System.Drawing.Point(223, 119);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(163, 64);
			this.checkedListBox1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(311, 261);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 31);
			this.button1.TabIndex = 1;
			this.button1.Text = "Обновить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(223, 327);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Ок";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(311, 327);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Cancel";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.Button3Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(311, 218);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 32);
			this.button4.TabIndex = 4;
			this.button4.Text = "Открыть";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.Button4Click);
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(282, 18);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(104, 45);
			this.trackBar1.TabIndex = 5;
			// 
			// trackBar2
			// 
			this.trackBar2.Location = new System.Drawing.Point(282, 52);
			this.trackBar2.Name = "trackBar2";
			this.trackBar2.Size = new System.Drawing.Size(104, 45);
			this.trackBar2.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 140);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 21);
			this.label1.TabIndex = 7;
			this.label1.Text = "Колонки для отображения:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(252, 32);
			this.label2.TabIndex = 8;
			this.label2.Text = "Частота обновления терминальных серверов";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(252, 32);
			this.label3.TabIndex = 9;
			this.label3.Text = "Частота обновления компьютеров";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 218);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(271, 32);
			this.label4.TabIndex = 10;
			this.label4.Text = "Открыть список серверов в текстовом редакторе";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 260);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(271, 32);
			this.label5.TabIndex = 11;
			this.label5.Text = "Обновить список серверов с диска";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(412, 364);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.trackBar2);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.checkedListBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Настройки";
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
