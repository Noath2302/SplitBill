// See https://aka.ms/new-console-template for more information
public class SplitBill
{
    private List<Transaction> Ledger;

    List<PaymentData> Data{ get; set; }

    Dictionary<string, double> SurplusList { get; set; }

    public void AddData(List<PaymentData> data)
    {
        SurplusList = new();
        Ledger = new();
        this.Data = data;
        foreach(var item in data)
        {
            SurplusList.Add(item.Name, item.Paid - item.Use);
        }
    }

    public List<Transaction> SettleOut()
    {
        if (CheckData()) {

            int index = 0;

            // Normal Calculation
            while (!CheckSettled())
            {
                
                double transferAmount = SurplusList[Data[index].Name];
                Ledger.Add(new Transaction(Data[index].Name, Data[index + 1].Name, -transferAmount));
                SurplusList[Data[index].Name] -= transferAmount;
                SurplusList[Data[index+1].Name] += transferAmount;
                index++;
            }

            // Ensure Positive transfer Amount
            Ledger = Transaction.FixPositiveTransactionAmount(Ledger);

            // Calculate smallest amount to transfer
            Ledger = Transaction.OptimizeAmount(Ledger);

            return Ledger;
        }
        else
        {
            return new List<Transaction>();
        }
    }

    private bool CheckSettled()
    {
        foreach (var item in SurplusList)
        {
            if (item.Value > 0.000000000000001)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckData()
    {
        double sum = 0;
        foreach (var item in SurplusList)
        {
            sum += item.Value;
        }
        if(sum < 0.000000000000001)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class PaymentData
{
    public string Name { get; set; }
    public double Paid { get; set; }
    public double Use { get; set; }

    public PaymentData(string name, double paid, double use)
    {
        Name = name;
        Paid = paid;
        Use = use;
    }
}

public class Transaction
{
    public string Payer { get; set; }
    public string Receiver { get; set; }
    public double Amount { get; set; }

    public Transaction(string payer, string receiver, double amount) { 
        this.Payer = payer;
        this.Receiver = receiver;
        this.Amount = amount;
    }

    internal static List<Transaction> FixPositiveTransactionAmount(List<Transaction> ledger)
    {
        foreach(Transaction transaction in ledger)
        {
            if(transaction.Amount < 0)
            {
                var temp = transaction.Payer;
                transaction.Payer = transaction.Receiver;
                transaction.Receiver = temp;
                transaction.Amount = Math.Abs(transaction.Amount);
            }
        }
        return ledger;
    }

    internal static List<Transaction> OptimizeAmount(List<Transaction> ledger)
    {
        var resultLedger = new List<Transaction>();
        for (int j = 0; j < ledger.Count; j += 1)
        {
            for(int i = 0; i< ledger.Count; i+=1)
            {
                if (ledger[i] == ledger[j])
                {
                    continue;
                }
                else
                {
                    if(ledger[j].Payer == ledger[i].Receiver)
                    {
                        if(ledger[j].Amount > ledger[i].Amount)
                        {
                            var a = ledger[j];
                            var b = ledger[i];
                            ledger[j] = new Transaction(b.Payer, a.Receiver, b.Amount);
                            ledger[i] = new Transaction(a.Payer, a.Receiver, a.Amount - b.Amount);
    }
                        else if (ledger[j].Amount < ledger[i].Amount)
                        {
                            var a = ledger[j];
                            var b = ledger[i];
                            ledger[j] = new Transaction(b.Payer, a.Receiver, a.Amount);
                            ledger[i] = new Transaction(b.Payer, a.Payer, b.Amount - a.Amount);
                        }
                        else
                        {
                            var a = ledger[j];
                            var b = ledger[i];

                            ledger[i] = new Transaction(b.Payer, a.Receiver, a.Amount);
                            ledger[j] = new Transaction(b.Payer, a.Receiver, a.Amount);
                        }
                    }
                }
            }
        }
        ledger = ledger.Distinct<Transaction>().ToList();
        return ledger;
    }
}