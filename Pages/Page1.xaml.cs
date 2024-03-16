using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlTypes;
using System.Diagnostics;


namespace SupermarketHack.Pages
{
    public class Values
    {
        public static readonly string Money = "\"Money\" : ";
        public static readonly string StoreLevel = "\"CurrentStoreLevel\" : ";
        public static readonly string CompletedCheckout = "\"CompletedCheckoutCount\" : ";
    }

    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private readonly static string saveFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\..\\LocalLow\\Nokta Games\\Supermarket Simulator\\SaveFile.es3";

        public Page1()
        {
            InitializeComponent();

            try
            {
                string allSaveText = File.ReadAllText(saveFile);
                MoneyTextBox.Text = GetValue(allSaveText, Values.Money);
                StoreLevelTextBox.Text = GetValue(allSaveText, Values.StoreLevel);
                CompletedCheckoutTextBox.Text = GetValue(allSaveText, Values.CompletedCheckout);
            }
            catch
            {
                MoneyTextBox.Text = "0";
                StoreLevelTextBox.Text = "0";
                CompletedCheckoutTextBox.Text = "0";
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProcessIsRunning())
                    MessageBox.Show("Обнаружена запущена игра!\nПожалуйста, закройте её для корректной работы программы", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);

                else
                {
                    string setMoney = Convert.ToDouble(MoneyTextBox.Text) > 1E+38 ? "1E+38" : MoneyTextBox.Text;
                    string setStoreLevel = Convert.ToInt32(StoreLevelTextBox.Text) > 101 ? "101" : StoreLevelTextBox.Text;
                    string setCompletedCheckout = Convert.ToInt64(CompletedCheckoutTextBox.Text) > 999999999 ? "999999999" : CompletedCheckoutTextBox.Text;

                    if (!string.IsNullOrEmpty(setMoney) && !string.IsNullOrEmpty(setStoreLevel) && !string.IsNullOrEmpty(setCompletedCheckout))
                    {
                        string allSaveText = File.ReadAllText(saveFile);

                        allSaveText = SetValue(allSaveText, Values.Money, setMoney);
                        allSaveText = SetValue(allSaveText, Values.StoreLevel, setStoreLevel);
                        allSaveText = SetValue(allSaveText, Values.CompletedCheckout, setCompletedCheckout);

                        File.WriteAllText(saveFile, allSaveText, Encoding.UTF8);
                        MessageBox.Show("Успех!", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Значения не должны быть пустыми!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string SetValue(string allSaveText, string key, string value)
        {
            foreach (string line in allSaveText.Split('\n'))
            {
                if (line.Contains(key))
                {
                    string oldValue = line.Replace(key, string.Empty).Trim();
                    string modifiedString = line.Replace(oldValue, $"{value},");

                    allSaveText = allSaveText.Replace(line, modifiedString);
                }
            }
            return allSaveText;
        }

        private string GetValue(string allSaveText, string key)
        {
            foreach (string line in allSaveText.Split('\n'))
            {
                if (line.Contains(key))
                    return line.Replace(key, string.Empty).Replace(",", string.Empty).Trim();
            }
            return string.Empty;
        }

        private bool ProcessIsRunning(string processName = "Supermarket Simulator")
        {
            return Process.GetProcessesByName(processName).Length > 0;
        }
    }
}
