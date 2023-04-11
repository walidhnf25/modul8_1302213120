using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace modul8_1302213120
{
    class Program
    {
        static void Main(string[] args)
        {
            BankTransferConfig config = new BankTransferConfig();
            Console.WriteLine(config.Lang == "en" ? "Please insert the amount of money to transfer:" 
                : "Masukkan jumlah uang yang akan di-transfer:");
            int nominalTransfer = int.Parse(Console.ReadLine());
            int transferFee = nominalTransfer <= config.Threshold ? config.LowFee : config.HighFee;
            int totalAmount = nominalTransfer + transferFee;
            Console.WriteLine(config.Lang == "en" ? $"Transfer fee = {transferFee}" : $"Biaya transfer = {transferFee}");
            Console.WriteLine(config.Lang == "en" ? $"Total amount = {totalAmount}" : $"Total biaya = {totalAmount}");
            Console.WriteLine(config.Lang == "en" ? "Select transfer method:" : "Pilih metode transfer:");
            foreach (string method in config.Methods)
            {
                Console.WriteLine($"{config.Methods.ToList().IndexOf(method) + 1}. {method}");
            }
            Console.WriteLine("4");
            Console.WriteLine(config.Lang == "en" ? $"Please type \"{config.ConfirmationEn}\" " +
                $"to confirm the transaction:" : $"Ketik \"{config.ConfirmationId}\" untuk mengkonfirmasi transaksi:");
            string confirmation = Console.ReadLine();
            if (confirmation == (config.Lang == "en" ? config.ConfirmationEn : config.ConfirmationId))
            {
                Console.WriteLine(config.Lang == "en" ? "The transfer is completed" : "Proses transfer berhasil");
            }
            else
            {
                Console.WriteLine(config.Lang == "en" ? "Transfer is cancelled" : "Transfer dibatalkan");
            }
        }
    }

    class BankTransferConfig
    {
        public string Lang { get; set; }
        public int Threshold { get; set; }
        public int LowFee { get; set; }
        public int HighFee { get; set; }
        public string[] Methods { get; set; }
        public string ConfirmationEn { get; set; }
        public string ConfirmationId { get; set; }

        public BankTransferConfig()
        {
            try
            {
                string jsonString = File.ReadAllText
                    (@"C:\Users\walid\source\repos\modul8_1302213120\modul8_1302213120\bank_transfer_config.json");
                JObject jsonObject = JObject.Parse(jsonString);
                Lang = jsonObject.GetValue("lang").ToString();
                Threshold = int.Parse(jsonObject.SelectToken("transfer.threshold").ToString());
                LowFee = int.Parse(jsonObject.SelectToken("transfer.low_fee").ToString());
                HighFee = int.Parse(jsonObject.SelectToken("transfer.high_fee").ToString());
                Methods = jsonObject.SelectToken("methods").ToObject<string[]>();
                ConfirmationEn = jsonObject.SelectToken("confirmation.en").ToString();
                ConfirmationId = jsonObject.SelectToken("confirmation.id").ToString();
            }
            catch (FileNotFoundException)
            {
                Lang = "en";
                Threshold = 25000000;
                LowFee = 6500;
                HighFee = 15000;
                Methods = new string[] { "RTO (real-time)", "SKN", "RTGS", "BI FAST" };
                ConfirmationEn = "yes";
                ConfirmationId = "ya";
            }
        }
    }
}