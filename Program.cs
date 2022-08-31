using System.Collections.Generic;
using System.Diagnostics;

SplitBill app = new SplitBill();
double tip = 5;
double NoPeople = 9;

PaymentData a = new("Toan", 30, 12.5 + tip / NoPeople);
PaymentData b = new("Duy", 0, 12.5 + tip / NoPeople);
PaymentData c = new("DuyG", 0, 17 + tip / NoPeople);
PaymentData d = new("DuyE", 60, 16 + tip / NoPeople);
PaymentData e = new("Hai", 20, 17 + tip / NoPeople);
PaymentData f = new("Khanh", 0, 28.5 + tip / NoPeople);
PaymentData g = new("Trang", 50, 17.5 + tip / NoPeople);
PaymentData h = new("Long", 10, 15.5 + tip / NoPeople);
PaymentData i = new("An", 0, 28.5 + tip / NoPeople);

List<PaymentData> list = new List<PaymentData> { a, b, c, d, e, f, g, h, i };
app.AddData(list);

List<Transaction> result = app.SettleOut();


foreach (var transaction in result)
{
    Debug.WriteLine($"{transaction.Payer} pays {transaction.Receiver} {Math.Round(transaction.Amount,4)}");
}