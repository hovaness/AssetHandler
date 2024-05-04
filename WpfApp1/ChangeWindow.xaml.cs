using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Extensions;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ChangeWindow.xaml
    /// </summary>
    public partial class ChangeWindow : Window
    {

        private TextBox amountText;
        private ComboBox currText;
        //поля банковских активов
        private TextBox bankText;
        private TextBox billText;
        //поля твердых номиналов
        private TextBox assetText;
        private TextBox ownerText;

        //поля всех возможных недежных активов
        private TextBox initialText;
        private TextBox marketText;
        //поля недвижимости
        private TextBox addresText;
        private TextBox constructionYearText;
        private TextBox constructionTypeText;
        private TextBox numberText;
        //поля инвентаря
        private TextBox typeText;
        private TextBox unitText;
        private TextBox quantityText;


        private Asset asset;
        public Asset Asset { get { return asset; } }
        public ChangeWindow(Asset asset)
        {
            InitializeComponent();
            this.asset = asset;
            control.ContentTemplate = this.GetDataTemplate(asset);
            SetTypeName();
        }

        private void SetTypeName()
        {
            Type type = asset.GetType();
            switch(type.Name)
            {
                case "Money":
                    typeName.Content = "Кеш";
                    break;
                case "BankMoney":
                    typeName.Content = "Банковский счет";
                    break;
                case "DifferentMoney":
                    typeName.Content = "Твердый номинал";
                    break;
                case "RealEstate":
                    typeName.Content = "Недвижимость";
                    break;
                case "Inventory":
                    typeName.Content = "Инвентарь";
                    break;
                default:
                    return;
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Am: " + amountText.Text + " Curr: " + currText.Text);
            //MainWindow.assets[MainWindow.assets.IndexOf(asset)] = new Money(decimal.Parse(amountBox.Text), currBox.Text);
            Hide();
        }
    }
}
