using AssetsHandler.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Extensions;
using WpfApp1.Models;
using WpfApp1.Repositories;

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
        private const string NOTNULL = "Не должно быть пустых полей!";

        private MoneyAssetRepository _moneyAssetRepository;
        private BankMoneyAssetRepository _bankMoneyAssetRepository;
        private DifferentMoneyAssetRepository _differentMoneyAssetRepository;
        private DbContext dbContext;
        string connectionString = "Server=localhost;Port=5433;Database=test;User Id=postgres;Password=12345;";

        public ChangeWindow(Asset asset)
        {
            InitializeComponent();
            this.asset = asset;
            control.ContentTemplate = this.GetDataTemplate(asset);
            SetTypeName();
            dbContext = new DbContext(connectionString);
            _moneyAssetRepository = new MoneyAssetRepository(dbContext);
            _bankMoneyAssetRepository = new BankMoneyAssetRepository(dbContext);
            _differentMoneyAssetRepository = new DifferentMoneyAssetRepository(dbContext);
        }
        
        private void SetTypeName()
        {
            Type type = asset.GetType();
            switch(type.Name)
            {
                case "Money":
                    typeName.Content = "Кеш";
                    typeName.Foreground = new SolidColorBrush(Color.FromRgb(252, 191, 73));
                    break;
                case "BankMoney":
                    typeName.Content = "Банковский счет";
                    typeName.Foreground = new SolidColorBrush(Color.FromRgb(247, 127, 0));
                    break;
                case "DiffrentMoney":
                    typeName.Content = "Твердый номинал";
                    typeName.Foreground = new SolidColorBrush(Color.FromRgb(188, 108, 37));
                    break;
                case "RealEstate":
                    typeName.Content = "Недвижимость";
                    typeName.Foreground = new SolidColorBrush(Color.FromRgb(0, 48, 73));
                    break;
                case "Inventory":
                    typeName.Content = "Инвентарь";
                    typeName.Foreground = new SolidColorBrush(Color.FromRgb(214, 40, 40));
                    break;
                default:
                    return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Asset updated;
            var main = new MainWindow();
            if (asset is RealAsset)
            {
                if(asset is RealEstate)
                {
                    if (!this.IsAnyStringEmptyOrNull(initialText.Text, marketText.Text, constructionYearText.Text, addresText.Text,
                        constructionTypeText.Text, numberText.Text, currText.Text))
                    {
                        //MainWindow.assets.Remove(asset);
                        updated = new RealEstate(addresText.Text, constructionTypeText.Text, currText.Text, int.Parse(constructionYearText.Text),
                            int.Parse(numberText.Text), decimal.Parse(initialText.Text), decimal.Parse(marketText.Text));
                        //MainWindow.assets.Add(updated);
                        Hide();
                    }
                    else
                        MessageBox.Show(NOTNULL);
                }
                else if(asset is Inventory)
                {
                    if(!this.IsAnyStringEmptyOrNull(initialText.Text, marketText.Text, unitText.Text, quantityText.Text, typeText.Text, currText.Text))
                    {
                        //MainWindow.assets.Remove(asset);
                        updated = new Inventory(typeText.Text, unitText.Text, currText.Text, int.Parse(quantityText.Text),
                            decimal.Parse(initialText.Text), decimal.Parse(marketText.Text));
                        //MainWindow.assets.Add(updated);
                        Hide();
                    }
                    else
                        MessageBox.Show(NOTNULL);
                }
            }
            else
            {
                if(asset is BankMoney)
                {
                    if(!this.IsAnyStringEmptyOrNull(amountText.Text, currText.Text, bankText.Text, billText.Text))
                    {
                        updated = new BankMoney(decimal.Parse(amountText.Text), currText.Text, bankText.Text, int.Parse(billText.Text));
                        updated.Id = asset.Id;
                        _bankMoneyAssetRepository.UpdateAsset(updated);
                        Hide();
                        main.UpdateAssetList();
                        main.ShowDialog();
                    }
                    else
                        MessageBox.Show(NOTNULL);
                }
                else if(asset is DiffrentMoney)
                {
                    if (!this.IsAnyStringEmptyOrNull(amountText.Text, currText.Text,assetText.Text, ownerText.Text))
                    {
                        updated = new DiffrentMoney(decimal.Parse(amountText.Text), currText.Text, assetText.Text, ownerText.Text);
                        updated.Id = asset.Id;
                       
                        _differentMoneyAssetRepository.UpdateAsset(updated);
                        Hide();
                        main.UpdateAssetList();
                        main.ShowDialog();
                    }
                    else
                        MessageBox.Show(NOTNULL);
                }
                else if(asset is Money)
                {
                    if(!this.IsAnyStringEmptyOrNull(amountText.Text, currText.Text))
                    {
                        updated = new Money(decimal.Parse(amountText.Text), currText.Text);
                        updated.Id = asset.Id;
                        _moneyAssetRepository.UpdateAsset(updated);
                        //MessageBox.Show(updated.Id.ToString());
                        Hide();
                        main.UpdateAssetList();
                        main.ShowDialog();
                    }
                    else
                        MessageBox.Show(NOTNULL);
                }
            }
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

        private void GetInfo(object sender, RoutedEventArgs e)
        {
            InitializeAllDynamicControls();
            if(asset is RealAsset)
            {
                if(asset is Inventory)
                {
                    var temp = (Inventory)asset;
                    initialText.Text = temp.InitialValue.ToString();
                    marketText.Text = temp.MarketValue.ToString();
                    currText.Text = temp.Currency;
                    typeText.Text = temp.Type;
                    unitText.Text = temp.Unit;
                    quantityText.Text = temp.Quantity.ToString();
                }
                else if(asset is RealEstate)
                {
                    var temp = (RealEstate)asset;
                    initialText.Text = temp.InitialValue.ToString();
                    marketText.Text = temp.MarketValue.ToString();
                    currText.Text = temp.Currency;
                    addresText.Text = temp.Address;
                    constructionYearText.Text = temp.ConstructionYear.ToString();
                    constructionTypeText.Text = temp.ConstructionType;
                    numberText.Text = temp.InventoryNumber.ToString();
                }
            }
            else
            {
                if(asset is BankMoney)
                {
                    var temp = (BankMoney)asset;
                    amountText.Text = temp.Amount.ToString();
                    currText.Text = temp.Currency;
                    bankText.Text = temp.Bank;
                    billText.Text = temp.Bill.ToString();
                }
                else if(asset is DiffrentMoney)
                {
                    var temp = (DiffrentMoney)asset;
                    amountText.Text = temp.Amount.ToString();
                    currText.Text = temp.Currency;
                    assetText.Text = temp.Holding;
                    ownerText.Text = temp.Owner;
                }
                else if(asset is Money)
                {
                    var temp = (Money)asset;
                    amountText.Text = temp.Amount.ToString();
                    currText.Text = temp.Currency;
                }
            }
        }
    }
}
