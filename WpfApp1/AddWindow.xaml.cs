using AssetsHandler.Models;
using Npgsql;
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
using WpfApp1.Repositories;

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
        private const string NOTNULL = "Не должно быть пустых полей!";

        //репозитории всех видов активов
        private MoneyAssetRepository _moneyAssetRepository;
        private BankMoneyAssetRepository _bankMoneyAssetRepository;
        private DifferentMoneyAssetRepository _differentMoneyAssetRepository;
        private InventoryAssetRepository _inventoryAssetRepository;
        private RealEstateAssetRepository _realEstateAssetRepository;

        //контекст базы данных со строкой подключения
        private DbContext dbContext;
        public AddWindow()
        {
            InitializeComponent();
            dbContext = new DbContext();
            _moneyAssetRepository = new MoneyAssetRepository(dbContext);
            _bankMoneyAssetRepository = new BankMoneyAssetRepository(dbContext);
            _differentMoneyAssetRepository = new DifferentMoneyAssetRepository(dbContext);
            _inventoryAssetRepository = new InventoryAssetRepository(dbContext);
            _realEstateAssetRepository = new RealEstateAssetRepository(dbContext);
            control.ContentTemplate = this.GetDataTemplate(0);
        }

       //метод который устанавливает шаблон с полями для нужного актива
        public void changeControl(DataTemplate template)
        {
            if (control != null)
            {
                control.ContentTemplate = template;
            }
            else return;
        }

        //метод инициализации всех текстовых полей
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
        private void assetTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int type = assetTypeComboBox.SelectedIndex;
            changeControl(this.GetDataTemplate(type));
        }

        //метод кнопки добавления актива, в зависимости от выбранного типа актива добавляет его в 
        //нужный репозиторий.
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int type = assetTypeComboBox.SelectedIndex;
            //базовые поля для денежных типов
            InitializeAllDynamicControls();

            string curr = currText.Text;
            var main = new MainWindow();
            //базовые поля для неденежных типов

            Asset newAsset;
            switch (type)
            {
                //денежные активы
                case 0:
           
                    if (!this.IsAnyStringEmptyOrNull(amountText.Text, curr))
                    {
                        decimal amount = decimal.Parse(amountText.Text);
                        newAsset = new Money(amount, curr);
                        _moneyAssetRepository.AddAsset(newAsset);
                        Hide();
                        main.UpdateAssetList();
                        main.ShowDialog();
                    }
                    else MessageBox.Show(NOTNULL);
                    break;
                //банковские счета
                case 1:
                   
                    if(!this.IsAnyStringEmptyOrNull(amountText.Text, curr,bankText.Text, billText.Text)){
                        decimal amount = decimal.Parse(amountText.Text);
                        string bank = bankText.Text;
                        int bill = int.Parse(billText.Text);
                        newAsset = new BankMoney(amount, curr,bank,bill);
                        _bankMoneyAssetRepository.AddAsset(newAsset);
                        Hide();
                        main.UpdateAssetList();
                        main.ShowDialog();
                    }
                    else MessageBox.Show(NOTNULL);
                    break;
                //другие активы
                case 2:
                    if(!this.IsAnyStringEmptyOrNull(amountText.Text, assetText.Text, curr, ownerText.Text))
                    {
                        decimal amount = decimal.Parse(amountText.Text);
                        string asset = assetText.Text;
                        string owner = ownerText.Text;
                        newAsset = new DiffrentMoney(amount,curr,asset,owner);
                        _differentMoneyAssetRepository.AddAsset(newAsset);
                        Hide();
                        main.UpdateAssetList(); 
                        main.ShowDialog();
                    }
                    else MessageBox.Show(NOTNULL);
                    break;
                //Недвижимость
                case 3:
                    
                    if(!this.IsAnyStringEmptyOrNull(initialText.Text, marketText.Text, constructionYearText.Text, addresText.Text,
                        constructionTypeText.Text, numberText.Text, curr))
                    {
                        decimal init = decimal.Parse(initialText.Text);
                        decimal marcet = decimal.Parse(marketText.Text);
                        int year = int.Parse(constructionYearText.Text);
                        string addres = addresText.Text;
                        string constructType = constructionTypeText.Text;
                        int number = int.Parse(numberText.Text);
                        newAsset = new RealEstate(addres,constructType,curr,
                            year, number, init,marcet);
                        _realEstateAssetRepository.AddAsset(newAsset);
                        main.UpdateAssetList(); main.ShowDialog();
                        Hide();
                    }
                    else MessageBox.Show(NOTNULL);
                    break;
                //Инвентарь
                case 4:
                    
                    if(!this.IsAnyStringEmptyOrNull(initialText.Text, marketText.Text, curr, quantityText.Text,unitText.Text, typeText.Text))
                    {
                        decimal init = decimal.Parse(initialText.Text);
                        decimal marcet = decimal.Parse(marketText.Text);
                        int quantity = int.Parse(quantityText.Text);
                        string unit = unitText.Text;
                        string inventoryType = typeText.Text;
                        newAsset = new Inventory(inventoryType,unit,curr,quantity, init,marcet);
                        _inventoryAssetRepository.AddAsset(newAsset);
                        main.UpdateAssetList();
                        main.ShowDialog();
                        Hide();
                    }
                    else MessageBox.Show(NOTNULL);
                    break;
                default:
                    return;
            }
        }
    }
}
