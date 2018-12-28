using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace mvvm.Model
{
    class CreditCalculate : INotifyPropertyChanged
    {
        #region Variables
        double sumCalculate;
        private double sumConst;
        private DateTime dataZaim;
        int srok;

        private Dictionary<string, int> sroks = new Dictionary<string, int>()
        {
            ["3 месяца"] = 3,
            ["6 месяцев"] = 6,
            ["9 месяцев"] = 9,
            ["1 год"] = 12,
            ["1 год и 3 месяца"] = 15,
            ["1 год и 6 месяцев"] = 18,
            ["2 года"] = 24,
            ["3 года"] = 36,
            ["5 лет"] = 60
        };

        public DateTime n;
        double zadoljennostKred;
        double nachislPrc;
        double osnDolg;
        double summPlatezh;
        double procent;


        //текстовое представление переменной
        //для форматированного вывода даты займа
        private string dz;
        ObservableCollection<CreditCalculate> listCalculations = new ObservableCollection<CreditCalculate>();
        private bool tfDiff;
        private bool tfAnnuit;
        private string cbSrokTmp;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public CreditCalculate()
        {
            dataZaim = DateTime.Now;
            dz = DateTime.Now.ToString("dd.MM.yyyy");
        }
        public CreditCalculate(DateTime d, double zadoljennost_kred, double summ_platezh, double osn_dolg, double nachisl_procent)
        {
            zadoljennostKred = zadoljennost_kred;
            summPlatezh = summ_platezh;
            osnDolg = osn_dolg;
            nachislPrc = nachisl_procent;
            dz = d.Date.ToString("dd.MM.yyyy");
            dataZaim = d;
        }



        #region Property
        //Свойство вывода на View и форматирования даты займа
        public string Dz
        {
            get { return dz; }
            set
            {
                dz = value;
                OnPropertyChanged(nameof(Dz));
            }
        }
        public Dictionary<string, int> Sroks
        {
            get { return sroks; }
            set
            {
                sroks = value;
                OnPropertyChanged(nameof(Sroks));
            }
        }
        
            public double SumConst
        {
            get { return sumConst; }
            
        }
        public double NachislPrc
        {
            get { return nachislPrc; }
        }
        public double OsnDolg
        {
            get { return osnDolg; }
        }
        public double Zadolzh
        {
            get { return zadoljennostKred; }
        }
        public double SumPlat
        {
            get { return summPlatezh; }
            set { summPlatezh = value; }
        }
        public double Prc
        {
            set
            {   procent=value;
                OnPropertyChanged(nameof(Prc));
            }
            get { return procent; }
        }

        public double Sum
        {
            set
            {
               sumCalculate=value;
                OnPropertyChanged(nameof(Sum));
            }
            get { return sumCalculate; }

        }
        public DateTime DataZaima
        {
            set
            {
                dataZaim= value;
                OnPropertyChanged(nameof(DataZaima));
            }
            get { return dataZaim; }
        }
        public ObservableCollection<CreditCalculate> Graph
        {
            set { listCalculations = value; }
            get { return listCalculations; }
        }
        public string CbSrokTmp
        {
            set { cbSrokTmp = value; }
            get { return cbSrokTmp; }
        }
        public bool TrueFalseDiff
        {
            get { return tfDiff; }
            set
            {
                tfDiff= value;
                OnPropertyChanged(nameof(TrueFalseDiff));
            }
        }
        public bool TrueFalseAnnuit
        {
            get { return tfAnnuit; }
            set
            {
                tfAnnuit= value;
                OnPropertyChanged(nameof(TrueFalseAnnuit));
            }
        }
        

        #endregion
        /// <summary>
        /// Метод определяющий какой тип выплат был использован в рассчете
        /// </summary>
        /// <returns></returns>
        public string TypePayToOpenSave()
        {
            if (tfAnnuit)
                return "Ануитентный";
            else return "Дифференцированный";
        }

        private void CsvToTypePayCalculate(string param)
        {
            if (TypePayToOpenSave() == "Ануитентный")
                TrueFalseAnnuit = true;
            else TrueFalseDiff = true;
        }

        /// <summary>
        /// Метод передачи переменных из файла CSV для рассчета и передачи listCalculation в DataGrid
        /// </summary>
        /// <param name="s">Принимает параметр List<string> для передачи значений переменным класса Calculate</param>
        private void CsvToCalc(List<string> s)
        {
            Prc = Convert.ToDouble(Regex.Replace(s[0], @"[^\d,. ]+", ""));
            DataZaima = Convert.ToDateTime(s[1]);
            cbSrokTmp = s[2];
            sumCalculate = Convert.ToDouble(Regex.Replace(s[3], @"[^\d,. ]+", ""));
            CsvToTypePayCalculate(s[4]);
        }

        


        /// <summary>
        /// Метод открытия документа Csv сохраненного ранее
        /// </summary>
        public void CalculateOpenCsv()
        {
            //создание спска типа string для дальнейшей передачи значений классу Calculate
            List<string> arr = new List<string>();
            //открытие диалогового окна
            OpenFileDialog ofd = new OpenFileDialog();
            //фильтр выбора документа по расширению
            ofd.Filter = @"CSV-файл (*.csv)|*.csv";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    //открытие файлового текстового потоков
                    FileStream save = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(save, Encoding.Default);
                    //цикличное считывание строк из документа
                    do
                    {
                        string[] s = sr.ReadLine().Split(';');
                        //выбор строк равным двум словам(шапка документа) для
                        //присвоения переменных и рассчета кредита
                        if (s.Length == 2)
                            arr.Add(s[1]);
                    } while (!sr.EndOfStream);
                    //очистка предыдущих расчетов
                    listCalculations.Clear();
                    //присвоение переменных для рассчета
                    CsvToCalc(arr);
                    //метод рассчета кредита
                    CalculationClick();
                    sr.Close();
                }
                catch
                {
                    MessageBox.Show("Файл открыт другой программой\nили нарушен порядок выгруженного документа");
                }
            }
        }

        /// <summary>
        /// Метод для кнопки рассчета погашения кредита
        /// </summary>
        public void CalculationClick()
        {
            try
            {
                if (sumCalculate != 0 && procent != 0)
                {
                    //сохраняем дату займа и сумму в дополнительные переменные
                    //для их неизменности
                    n = dataZaim;
                    sumConst = Sum;
                    listCalculations.Clear();
                    if (tfAnnuit == true || tfDiff == true)
                    {
                        //устанавливаем срок погашения в месяцах
                        srok = sroks[cbSrokTmp];
                        int a = srok - 1;
                        //вызов метода рассчета кредита
                        CalcCred(a);
                        Sum = sumConst;
                    }
                    else if (tfAnnuit == false || tfDiff == false)
                    {
                        MessageBox.Show("Выберете метод расчёта:");
                    }
                }
            }
            catch { MessageBox.Show("Заполните все поля!"); }
            //возврат даты займа
            dataZaim = n;
        }

        /// <summary>
        /// Метод кнопки очистки данных для объекта, сбрасывает все поля на 0
        /// </summary>
        public void ClearClic()
        {
            Sum = 0;
            sumConst = 0;
            zadoljennostKred = 0;
            summPlatezh = 0;
            osnDolg = 0;
            nachislPrc = 0;
            Prc = 0;
            DataZaima = DateTime.Now;
            Dz = DateTime.Now.ToString("dd.MM.yyyy");
            listCalculations.Clear();

        }

        /// <summary>
        /// Метод рассчета кредита, принимает в качестве параметра число срока платежа,
        /// и выбирает нужный вариант рассчета кредита
        /// </summary>
        /// <param name="a">число срока рассчета платежа в месяцах</param>
        private void CalcCred(int a)
        {
            dataZaim = dataZaim.AddMonths(1);
            //определение типа рассчета кредита и последующий рассчет
            if (tfDiff == true)
            {
                for (int i = 0; i <= a; i++)
                {
                    //метод рассчета
                    RasschetCredidsDiffer();
                    //добавление в коллекцию
                    listCalculations.Add(new CreditCalculate(dataZaim, zadoljennostKred, SumPlat, osnDolg, nachislPrc)); ;
                    //дополнительные рассчеты
                    dataZaim = dataZaim.AddMonths(1);
                    Sum = sumCalculate - osnDolg;
                    srok--;
                }
            }
            else if (tfAnnuit == true)
            {
                for (int i = 0; i <= a; i++)
                {
                    CalculateCreditsAnnuitentn();
                    listCalculations.Add(new CreditCalculate(dataZaim, zadoljennostKred, SumPlat, osnDolg, nachislPrc));
                    dataZaim = dataZaim.AddMonths(1);
                    Sum = sumCalculate - osnDolg;
                    srok--;
                }
            }

            Dz = dataZaim.AddMonths(-1).ToString("dd.MM.yyyy");
        }

        /// <summary>
        /// Метод для определения текущей валюты рассчета
        /// используется при печати документа
        /// </summary>
        /// <returns></returns>
        public string CurrencyChange()
        {
            if (procent == 9)
                return "USD";
            else if (procent == 6)
                return "EUR";
            else return "Рубль";
        }

        /// <summary>
        /// Метод аннуитентного рассчета кредита
        /// </summary>
        private void CalculateCreditsAnnuitentn()
        {
            summPlatezh = sumCalculate * ((procent / 100.00 / 12.00) / (1 - Math.Pow((1 + (procent / 100.00 / 12.00)), srok - (srok * 2))));
            nachislPrc = (procent / 12) * sumCalculate / 100;
            osnDolg = summPlatezh - nachislPrc;
            zadoljennostKred = sumCalculate;
        }

        /// <summary>
        /// Метод дифференциального рассчета кредита
        /// </summary>
        private void RasschetCredidsDiffer()
        {
            osnDolg = sumCalculate / srok;
            nachislPrc = (procent / 12) * sumCalculate / 100;
            summPlatezh = nachislPrc + osnDolg;
            zadoljennostKred = sumCalculate;

        }
        
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
    }
}
