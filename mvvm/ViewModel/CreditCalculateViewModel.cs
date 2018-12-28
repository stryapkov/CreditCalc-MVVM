using mvvm.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mvvm.ViewModel
{
    class CreditCalculateViewModel : INotifyPropertyChanged
    {       
        CreditCalculate calc = new CreditCalculate();
        Printing print = new Printing();        


        public CreditCalculate Calc
        {
            get { return calc; }
            set
            {
                calc = value;
                OnPropertyChanged(nameof(CreditCalculate));
            }
        }
            

        private RelayCommand calculateCrediit;
        public RelayCommand CalculateCrediit
        {
            get
            {
                return calculateCrediit ??
                  (calculateCrediit = new RelayCommand(obj =>
                  {
                      Calc.CalculationClick();
                  }));
            }
        }

        private RelayCommand calculateSave;
        public RelayCommand CalculateSave
        {
            get
            {
                return calculateSave ??
                  (calculateSave = new RelayCommand(obj =>
                  {
                      SaveOpenToCsv.SavedPage(Calc.DataZaima, Calc.Prc, Calc.CurrencyChange(), Calc.CbSrokTmp, Calc.SumConst,
                          Calc.TypePayToOpenSave(), Calc.Graph);
                  }));
            }
        }

        private RelayCommand calculateDel;
        public RelayCommand CalculateDel
        {
            get
            {
                return calculateDel ??
                  (calculateDel = new RelayCommand(obj =>
                  {
                      Calc.ClearClic();
                  }));
            }
        }
        private RelayCommand calculatePrint;
        public RelayCommand CalculatePrint
        {
            get
            {
                return calculatePrint ??
                  (calculatePrint = new RelayCommand(obj =>
                  {
                      print.SavedPage(Calc.DataZaima, Calc.Prc, Calc.CurrencyChange(), Calc.CbSrokTmp, Calc.SumConst,
                          Calc.TypePayToOpenSave(), Calc.Graph);
                  }));
            }
        }

        public RelayCommand calcOpenCsv;
        public RelayCommand CalcOpenCsv
        {
            get
            {
                return calcOpenCsv ??
                  (calcOpenCsv = new RelayCommand(obj =>
                  {
                      Calc.CalculateOpenCsv();
                  }));
            }
        }



        

        public event PropertyChangedEventHandler PropertyChanged;
public void OnPropertyChanged([CallerMemberName]string prop = "")
{
    if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(prop));
}
    }
}
