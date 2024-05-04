using AssetsHandler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        //поля всех возможных активов денежных активов
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

        //интерфейс
        public AddWindow()
        {
            InitializeComponent();
            control.ContentTemplate = this.GetDataTemplate(0);
        }

        public void changeControl(DataTemplate template)
        {
            if (control != null)
            {
                control.ContentTemplate = template;
            }
            else return;
        }


        private void InitializeAllDynamicControls()
        {
            amountText = this.FindVisualChild<TextBox>("amount");
            currText = this.FindVisualChild<ComboBox>("curr");
            bankText = this.FindVisualChild<TextBox>("bank");
            billText = this.FindVisualChild<TextBox>("bill");
            assetText = this.FindVisualChild<TextBox>("holding");
            ownerText = this.FindVisualChild<TextBox>("owner");
            initialText = this.FindVisualChild<TextBox>("init");
            marketText = this.FindVisualChild<TextBox>("market");
            addresText = this.FindVisualChild<TextBox>("addres");
            constructionYearText = this.FindVisualChild<TextBox>("year");
            constructionTypeText = this.FindVisualChild<TextBox>("construction");
            numberText = this.FindVisualChild<TextBox>("number");
            typeText = this.FindVisualChild<TextBox>("type");
            unitText = this.FindVisualChild<TextBox>("unit"); 
            quantityText = this.FindVisualChild<TextBox>("quantity");
        }
        private void checkNullProperties(int typeIndex)
        {

        }
        private void assetTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int type = assetTypeComboBox.SelectedIndex;
            changeControl(this.GetDataTemplate(type));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int type = assetTypeComboBox.SelectedIndex;
            InitializeAllDynamicControls();
            //базовые поля для денежных типов
            
            string curr = currText.Text;

            //базовые поля для неденежных типов
            
            Asset newAsset;
            switch (type)
            {
                //денежные активы
                case 0:
                    decimal amount = decimal.Parse(amountText.Text);
                    newAsset = new Money(amount, curr);
                    break;
                //банковские счета
                case 1:
                    decimal amountBank = decimal.Parse(amountText.Text);
                    string bank = bankText.Text;
                    int bill = int.Parse(billText.Text);
                    newAsset = new BankMoney(amountBank, curr,bank,bill);
                    break;
                //другие активы
                case 2:
                    decimal amountDiff = decimal.Parse(amountText.Text);
                    string asset = assetText.Text;
                    string owner = ownerText.Text;
                    newAsset = new DiffrentMoney(amountDiff,curr,asset,owner);
                    break;
                //Недвижимость
                case 3:
                    decimal initEstate = decimal.Parse(initialText.Text);
                    decimal marcet = decimal.Parse(marketText.Text);
                    int year = int.Parse(constructionYearText.Text);
                    string addres = addresText.Text;
                    string constructType = constructionTypeText.Text;
                    int number = int.Parse(numberText.Text);
                    newAsset = new RealEstate(addres,constructType,curr,
                        year, number, initEstate,marcet);
                    break;
                //Инвентарь
                case 4:
                    decimal initInventory = decimal.Parse(initialText.Text);
                    decimal marcet1 = decimal.Parse(marketText.Text);
                    int quantity = int.Parse(quantityText.Text);
                    string unit = unitText.Text;
                    string inventoryType = typeText.Text;
                    newAsset = new Inventory(inventoryType,unit,curr,quantity, initInventory,marcet1);
                    break;
                default:
                    return;
            }
            MainWindow.assets.Add(newAsset);
            Hide();
        }
    }
}
