using mvvm.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvvm
{
    interface ISavePage
    {

         void  SavedPage(DateTime DataZaima, double procent, string CurrencyChange,
            string cbSrokTmp, double sumConst, string TypePayToOpenSave, ObservableCollection<CreditCalculate> listCalculations);

    }
}
