/*
 * Создано в SharpDevelop.
 * Пользователь: nkarpov
 * Дата: 21.07.2020
 * Время: 22:19
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Windows.Forms;

namespace TSearch_v0._1
{
	/// <summary>
	/// Description of customDataGridView.
	/// </summary>

		class customDataGridView : DataGridView
		{
		  protected override bool ProcessDialogKey(Keys keyData)
		  {
		     if (keyData == Keys.Enter)
		     {
		        int col = this.CurrentCell.ColumnIndex;
		        int row = this.CurrentCell.RowIndex;
		        this.CurrentCell = this[col, row];
		        return true;
		     }
		     return base.ProcessDialogKey(keyData);
		  }
		
		  protected override void OnKeyDown(KeyEventArgs e)
		  {
		     if (e.KeyData == Keys.Enter)
		     {
		     	if(this.CurrentCell == null) return;
		        int col = this.CurrentCell.ColumnIndex;
		        int row = this.CurrentCell.RowIndex;
		        this.CurrentCell = this[col, row];
		        e.Handled = true;
		     }
		     base.OnKeyDown(e);
		  }
		}
	
}
