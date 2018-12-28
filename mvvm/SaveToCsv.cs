using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mvvm.Model;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace mvvm
{
   static class SaveOpenToCsv
    { 
        /// <summary>
        /// Метод для сохранения рассчета в формат CSV
        /// </summary>
       static public void SavedPage(DateTime DataZaima, double procent, string CurrencyChange,
                   string cbSrokTmp, double sumConst, string TypePayToOpenSave, ObservableCollection<CreditCalculate> listCalculations)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = @"CSV-файл (*.csv)|*.csv";
            //счетчик строк
            int i = 1;
            if (saveFile.ShowDialog() == true)
            {
                //открываем или создаем файл .csv для записи
                using (StreamWriter sw = new StreamWriter(saveFile.OpenFile(), Encoding.Default))
                {
                    //запись данных в определенном порядке
                    sw.WriteLine($"Процентная ставка:;{procent} %\n" +
                                 $"Дата займа:;{DataZaima.ToString("dd.MM.yyyy")}\n" +
                                 $"Срок погашения:;{cbSrokTmp}\n" +
                                 $"Сумма:;{sumConst}\n" +
                                 $"Тип выплат:;{TypePayToOpenSave}\n");
                    sw.WriteLine("№;Дата платежа;Задолженность по кредиту;Сумма платежа;Основной долг;Начисленные проценты");
                    foreach (var cell in listCalculations)
                    {
                        sw.Write($"{i++};" +
                                 $"{cell.Dz};" +
                                 $"{cell.Zadolzh.ToString("##.##")};" +
                                 $"{cell.SumPlat.ToString("##.##")};" +
                                 $"{cell.OsnDolg.ToString("##.##")};" +
                                 $"{cell.NachislPrc.ToString("##.##")};");
                        sw.WriteLine();
                    }
                    sw.Close();
                }
            }
        }


        /// <summary>
        /// Метод открытия документа Csv сохраненного ранее
        /// </summary>
       //static public void CalculateOpenCsv(DateTime DataZaima, double procent, string CurrencyChange,
       //            string cbSrokTmp, double sumConst, string TypePayToOpenSave, ObservableCollection<CreditCalculate> listCalculations)
       // {
       //     //создание спска типа string для дальнейшей передачи значений классу Calculate
       //     List<string> arr = new List<string>();
       //     //открытие диалогового окна
       //     OpenFileDialog ofd = new OpenFileDialog();
       //     //фильтр выбора документа по расширению
       //     ofd.Filter = @"CSV-файл (*.csv)|*.csv";
       //     if (ofd.ShowDialog() == true)
       //     {
       //         try
       //         {
       //             //открытие файлового текстового потоков
       //             FileStream save = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
       //             StreamReader sr = new StreamReader(save, Encoding.Default);
       //             //цикличное считывание строк из документа
       //             do
       //             {
       //                 string[] s = sr.ReadLine().Split(';');
       //                 //выбор строк равным двум словам(шапка документа) для
       //                 //присвоения переменных и рассчета кредита
       //                 if (s.Length == 2)
       //                     arr.Add(s[1]);
       //             } while (!sr.EndOfStream);
       //             //очистка предыдущих расчетов
       //             listCalculations.Clear();
       //             //присвоение переменных для рассчета
       //             CsvToCalc(arr);
       //             //метод рассчета кредита
       //             CalculationClick();
       //             sr.Close();
       //         }
       //         catch
       //         {
       //             MessageBox.Show("Файл открыт другой программой\nили нарушен порядок выгруженного документа");
       //         }
       //     }
       // }


       // /// <summary>
       // /// Метод передачи переменных из файла CSV для рассчета и передачи listCalculation в DataGrid
       // /// </summary>
       // /// <param name="s">Принимает параметр List<string> для передачи значений переменным класса Calculate</param>
       // private void CsvToCalc(List<string> s)
       // {
       //     Prc = Convert.ToDouble(Regex.Replace(s[0], @"[^\d,. ]+", ""));
       //     DataZaima = Convert.ToDateTime(s[1]);
       //     cbSrokTmp = s[2];
       //     sumCalculate = Convert.ToDouble(Regex.Replace(s[3], @"[^\d,. ]+", ""));
       //     CsvToTypePayCalculate(s[4]);
       // }

    }
}
