using mvvm.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace mvvm
{
    /// <summary>
    /// Класс для печати рассчетов
    /// </summary>

    class Printing :ISavePage
    {
        PrintDialog printDialog;
        TextBlock header;
        DataTable dt;
        PrintigPage dsPaginator;
        public void SavedPage(DateTime DataZaima, double procent, string CurrencyChange,
                   string cbSrokTmp, double sumConst, string TypePayToOpenSave, ObservableCollection<CreditCalculate> listCalculations)
        {
            //открываем диалоговое окно принтера
            printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                //нумератор строк
                int i = 1;
                //шапка документа
                header = new TextBlock();
                header.Inlines.Add($"Процентная ставка: {procent}%\n" +
                                   $"Дата займа: {DataZaima.ToString("dd.MM.yyyy")}\n" +
                                   $"Валюта: {CurrencyChange}\n" +
                                   $"Срок погашения: {cbSrokTmp}\n" +
                                   $"Сумма: {sumConst.ToString("f")} {CurrencyChange}\n" +
                                   $"Тип выплат: {TypePayToOpenSave}");

                //таблица рассчета документа для печати
                dt = new DataTable();
                //DataSet ds=new DataSet();
                //создаем колонки
                dt.Columns.Add(new DataColumn("№", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Дата платежа", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Остаток", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Сумма платежа", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Основной платеж", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("Проценты", System.Type.GetType("System.String")));

                //заполняем сроки таблиц
                foreach (var cell in listCalculations)
                {
                    DataRow rowAdd = dt.NewRow();
                    rowAdd["№"] = i.ToString();
                    rowAdd["Дата платежа"] = cell.Dz;
                    rowAdd["Остаток"] = cell.Zadolzh.ToString("C");
                    rowAdd["Сумма платежа"] = cell.SumPlat.ToString("C");
                    rowAdd["Основной платеж"] = cell.OsnDolg.ToString("C");
                    rowAdd["Проценты"] = cell.NachislPrc.ToString("P");
                    dt.Rows.InsertAt(rowAdd, i++);
                }
                //создаем экземпляр класса StoreDataSetPaginator
                //для вывода на печать и рассчета печатаемых страниц
                dsPaginator = new PrintigPage(dt, header,
                    new Typeface("Calibri"), 14, 69 * 0.75,
                    new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
                try
                {
                    printDialog.PrintDocument(dsPaginator, "printing...");
                }
                catch
                {
                    MessageBox.Show("Пустой документ,\n составьте документ заново");
                }

            }

        }
    }
}
