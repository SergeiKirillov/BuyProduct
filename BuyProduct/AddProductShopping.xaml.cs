using System;
using System.Collections.Generic;
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
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Globalization;

namespace BuyProduct
{
    /// <summary>
    /// Логика взаимодействия для AddProductShopping.xaml
    /// </summary>
    public partial class AddProductShopping : Window
    {
        PriceShop AddShoping = new PriceShop();

        #region Автоматическое переключение раскладки
        //*****************************************************
        //Функции винапи
        //*****************************************************
        const int KL_NAMELENGTH = 9;
        const int KLF_ACTIVATE = 1;

        [DllImport("user32.dll")]
        public static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);
        [DllImport("user32.dll")]
        public static extern long GetKeyboardLayoutName(System.Text.StringBuilder pwszKLID);

        public static string getKLName()
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder(KL_NAMELENGTH);
            GetKeyboardLayoutName(name);
            return name.ToString();
        }

        private static void setKLName(string pwszKLID)
        {
            LoadKeyboardLayout(pwszKLID, KLF_ACTIVATE);
        }

        #endregion


        PriceShop pokupka;
        float Zero = 0.001f;
        private DateTime dtShop;

        public AddProductShopping()
        {
            InitializeComponent();

            pokupka = new PriceShop();
            this.DataContext = pokupka;

            dtShop = DateTime.Now;

            LoadComboBox();

        }

        private void LoadComboBox()
        {
            
            dtShoping.Text = dtShop.ToString();

            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                string sqlExpression;
                SQLiteCommand command;

                #region Список категорий продуктов

                sqlExpression = "select name from ProdCategoriaName order by name asc";
                command = new SQLiteCommand(sqlExpression, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string rt = reader.GetString(0);
                            cmbCategoriaProduct.Items.Add(reader.GetString(0));
                        }


                    }

                }

                #endregion

                #region Категория расходов
                
                sqlExpression = "Select CategoriaShopping FROM CatShop order by CategoriaShopping asc";
                command = new SQLiteCommand(sqlExpression, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cmbCatShop.Items.Add(reader.GetString(0));
                        }


                    }

                }
                #endregion

                #region Ед измерения массы
                sqlExpression = "select Unit from ProdUnit order by Unit asc";
                command = new SQLiteCommand(sqlExpression, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cmbProdUnit.Items.Add(reader.GetString(0));
                        }


                    }

                }
                #endregion


                #region Магазин
                sqlExpression = "select name,City,Addres from ShopName order by name asc";
                command = new SQLiteCommand(sqlExpression, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cmbShopName.Items.Add(reader.GetString(0)+","+ reader.GetString(1)+","+reader.GetString(2));
                        }


                    }

                }
                #endregion


                connection.Close();
                
            }
        }

        private void LoadComboBox2()
        {
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();

                

            }
        }

        private void BtnSaveDB_Click(object sender, RoutedEventArgs e)
        {
            dtShop = (DateTime)dtShoping.SelectedDate;

            LoadComboBox();
        }

        private void cmbCategoriaProduct_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!cmbCategoriaProduct.IsKeyboardFocusWithin && String.IsNullOrEmpty(cmbCategoriaProduct.Text))
            {
                MessageBox.Show("Выберите значение");
            }
        }

        private void cmbCategoriaProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter)||(e.Key == Key.Tab))
            {
                string txtCatProduct = cmbCategoriaProduct.Text;

                //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                if (!cmbCategoriaProduct.Items.Contains(txtCatProduct))
                {
                    cmbCategoriaProduct.Items.Add(txtCatProduct);
                   System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Продуктов________________");


                    #region Записываем данные в БД
                    using (var connection = new SQLiteConnection("Data Source = product.db"))
                    {
                        connection.Open();
                        string sqlExpression;
                        SQLiteCommand command;

                        sqlExpression = "INSERT INTO  ProdCategoriaName(name) VALUES ('"+txtCatProduct+"')";
                        command = new SQLiteCommand(sqlExpression, connection);

                        int result = command.ExecuteNonQuery();
                        
                        connection.Close();

                    }

                    #endregion

                }


            }
        }

        private void cmbCatShop_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string txtCatShop = cmbCatShop.Text;

                //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                if (!cmbCatShop.Items.Contains(txtCatShop))
                {
                    cmbCatShop.Items.Add(txtCatShop);
                    System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Расходов________________");


                    #region Записываем данные в БД
                    using (var connection = new SQLiteConnection("Data Source = product.db"))
                    {
                        connection.Open();
                        string sqlExpression;
                        SQLiteCommand command;

                        sqlExpression = "INSERT INTO  CatShop(CategoriaShopping) VALUES ('"+ txtCatShop + "')";
                        command = new SQLiteCommand(sqlExpression, connection);

                        int result = command.ExecuteNonQuery();

                        connection.Close();

                    }

                    #endregion

                }


            }
        }

        private void cmbProdUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string txtProdUnit = cmbProdUnit.Text;

                //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                if (!cmbProdUnit.Items.Contains(txtProdUnit))
                {
                    cmbProdUnit.Items.Add(txtProdUnit);
                    System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Размерности________________");


                    #region Записываем данные в БД
                    using (var connection = new SQLiteConnection("Data Source = product.db"))
                    {
                        connection.Open();
                        string sqlExpression;
                        SQLiteCommand command;

                        sqlExpression = "INSERT INTO  ProdUnit(Unit) VALUES ('" + txtProdUnit + "')";
                        command = new SQLiteCommand(sqlExpression, connection);

                        int result = command.ExecuteNonQuery();

                        connection.Close();

                    }

                    #endregion

                }


            }
        }

        private void cmbShopName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string txtShopName = cmbShopName.Text;
                string NameShop;
                string CityShop;
                String AddresShop;

                //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                if (!cmbShopName.Items.Contains(txtShopName))
                {
                    cmbShopName.Items.Add(txtShopName);
                    System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Размерности________________");

                    #region разщепляем строчку на Name, City, Addres
                    if (txtShopName!="")
                    {

                    }
                    string[] shop = txtShopName.Split(new char[] { ',' });
                    NameShop = shop[0];
                    CityShop = shop[1];
                    AddresShop = shop[2];
                    #endregion


                    #region Записываем данные в БД
                    using (var connection = new SQLiteConnection("Data Source = product.db"))
                    {
                        connection.Open();
                        string sqlExpression;
                        SQLiteCommand command;

                        sqlExpression = "INSERT INTO  ShopName(Name,City,Addres) VALUES ('" + NameShop+"','"+ CityShop+"','"+ AddresShop+ "')";
                        command = new SQLiteCommand(sqlExpression, connection);

                        int result = command.ExecuteNonQuery();

                        connection.Close();

                    }

                    #endregion

                }


            }
        }

        private void txtProductName_GotFocus(object sender, RoutedEventArgs e)
        {
            setKLName("00000419");
            txtProductName.SelectionStart = 0;
            txtProductName.SelectionLength = txtProductName.Text.Length;
            
        }


        
        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            #region Вариант 1
            KeyConverter converter = new KeyConverter();

            string key = converter.ConvertToString(e.Key);

            if ((key != null && key.Length == 1))
            {
                // e.Handled = Char.IsDigit(key[0]) == false;
                e.Handled = Char.IsDigit(key[0]) == false;
            }
            #endregion

            #region Вариант 2



            #endregion

        }

        private void txtProductPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) || (e.Key.Equals(Key.Tab)))
            {
                //AddShoping.productPrice = (txtProductPrice.Text == "") ? float.Parse(txtProductPrice.Text) : 0 ;
                if ((txtProductPrice.Text.Equals("")) || (txtProductPrice.Text.Equals("0"))) txtProductPrice.Text = Zero.ToString();
                txtProductMassa.Focus();
            }
            else if (e.Key.Equals(Key.OemComma))
            {
                e.Handled = true;
            }
            
        }
      

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectionStart = 0;
            tb.SelectionLength = tb.Text.Length;

        }

        private void txtProductMassa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) || (e.Key.Equals(Key.Tab)))
            {
                //AddShoping.productMassa = (txtProductMassa.Text == "") ? float.Parse(txtProductMassa.Text) : 0 ;
                if (txtProductMassa.Text.Equals("")|| txtProductMassa.Text.Equals("0")) txtProductMassa.Text = Zero.ToString();
                cmbProdUnit.Focus();
            }
            else if (e.Key.Equals(Key.OemComma))
            {
                e.Handled = true;
            }
        }

        private void txtProductSkidka_KeyDown(object sender, KeyEventArgs e)
        {
            
            

            if (e.Key.Equals(Key.Enter)||(e.Key.Equals(Key.Tab)) )
            {
                if (txtProductSkidka.Text.Equals("")) txtProductSkidka.Text = "0";
                AddShoping.productPrice = float.Parse(txtProductPrice.Text, CultureInfo.InvariantCulture);
                AddShoping.productMassa = float.Parse(txtProductMassa.Text, CultureInfo.InvariantCulture);
                AddShoping.ProductDiscont = float.Parse(txtProductSkidka.Text, CultureInfo.InvariantCulture);

               // AddShoping.ProductDiscont = (txtProductSkidka.Text == "") ? float.Parse(txtProductSkidka.Text) : { 0};

                float tt = (AddShoping.productPrice * AddShoping.productMassa) - AddShoping.ProductDiscont;
               


                txtProductItogo.Text = String.Format("{0:0.##}",AddShoping.productRashod);
                cmbShopName.Focus();
            }
            else if (e.Key.Equals(Key.OemComma))
            {
                e.Handled = true;
            }
        }
    }
}
