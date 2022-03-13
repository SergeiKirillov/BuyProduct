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
                string txtProductName = cmbProductName.Text;

                System.Diagnostics.Debug.WriteLine($"*КатРасход,Цена1ед,Кол-во,ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");

                //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                if (!cmbCatShop.Items.Contains(txtCatShop))
                {
                    cmbCatShop.Items.Add(txtCatShop);
                    System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Расходов________________");


                    #region Записываем данные в БД CatShop
                    using (var connection = new SQLiteConnection("Data Source = product.db"))
                    {
                        connection.Open();
                        string sqlExpression;
                        SQLiteCommand command;

                        sqlExpression = "INSERT INTO  CatShop(CategoriaShopping) VALUES ('" + txtCatShop + "')";
                        command = new SQLiteCommand(sqlExpression, connection);

                        int result = command.ExecuteNonQuery();

                        connection.Close();

                    }
                    #endregion
                }

                #region Сохраняем В БД  ProductNames

                using (var connection = new SQLiteConnection("Data Source = product.db"))
                {
                    connection.Open();
                    string sqlExpression;
                    SQLiteCommand command;

                    sqlExpression = "select productName from ProductNames where productName='" + txtProductName + "'";
                    command = new SQLiteCommand(sqlExpression, connection);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            sqlExpression = "INSERT INTO  ProductNames(productName,ProductCategoriaName,CategoriaShopping) VALUES ('" + txtProductName + "','" + cmbCategoriaProduct.Text + "','" + txtCatShop + "')";
                            command = new SQLiteCommand(sqlExpression, connection);

                            int result = command.ExecuteNonQuery();


                        }

                    }

                    connection.Close();

                }

                #endregion
               
                MoveToNextElement(e);

                //txtProductPrice.Focus();


            }
        }

        private void cmbProdUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string txtProdUnit = cmbProdUnit.Text;

                System.Diagnostics.Debug.WriteLine($"КатРасход,Цена1ед,Кол-во,*ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");

                //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                if (!cmbProdUnit.Items.Contains(txtProdUnit))
                {
                    cmbProdUnit.Items.Add(txtProdUnit);
                    //System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Размерности________________");


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



                if (!txtProductPrice.Text.Equals("0"))
                {
                    float massa = float.Parse(txtProductMassa.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    float Price1ed = float.Parse(txtProductPrice.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    float tt = Price1ed * massa;
                    txtItogo.Text = String.Format("{0:0.##}", tt);
                }

                    txtProductSkidka.Focus();


               

            }
        }

        private void cmbShopName_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                string txtShopName = cmbShopName.Text;
                if (!txtShopName.Equals(""))
                {
                    string NameShop;
                    string CityShop="";
                    string AddresShop="";

                    //System.Diagnostics.Debug.WriteLine(txtCatProduct);

                    if (!cmbShopName.Items.Contains(txtShopName))
                    {
                        cmbShopName.Items.Add(txtShopName);
                        System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию Категория Размерности________________");

                        #region разщепляем строчку на Name, City, Addres
                        
                        string[] shop = txtShopName.Split(new char[] { ',' });

                        NameShop = shop[0];
                        if (!NameShop.Equals(""))
                        {
                            if (shop.Length>1)
                            {
                                CityShop = !shop[1].Equals("") ? shop[1] : "Темиртау";
                                AddresShop = shop[2];
                            }
                            else 
                            {
                                CityShop = "Темиртау";
                            }

                            //CityShop = !shop[1].Equals("")?shop[1]:"Темиртау";
                            //AddresShop = shop[2];

                            #region Записываем данные в БД
                            using (var connection = new SQLiteConnection("Data Source = product.db"))
                            {
                                connection.Open();
                                string sqlExpression;
                                SQLiteCommand command;

                                sqlExpression = "INSERT INTO  ShopName(Name,City,Addres) VALUES ('" + NameShop + "','" + CityShop + "','" + AddresShop + "')";
                                command = new SQLiteCommand(sqlExpression, connection);

                                int result = command.ExecuteNonQuery();

                                if (!result.Equals(0))
                                {
                                    cmbShopName.Items.Add(NameShop+","+ CityShop+","+ AddresShop);
                                }
                                connection.Close();

                            }

                            #endregion
                        }
                        else
                        {
                            cmbShopName.Focus();
                        }
                       
                        #endregion


                        

                    }
                }
                else
                {
                    cmbShopName.Focus();
                }

                #region записываем значение в таблицу ProductNames 
                // string txtProductName = cmbProductName.Text;



                //if (!cmbProductName.Items.Contains(txtProductName))
                //{
                //    cmbProductName.Items.Add(txtProductName);
                //    System.Diagnostics.Debug.WriteLine("________________Добавлено в коллекцию ПодКласс Продукта________________");


                //    #region Записываем данные в БД
                //    using (var connection = new SQLiteConnection("Data Source = product.db"))
                //    {
                //        connection.Open();
                //        string sqlExpression;
                //        SQLiteCommand command;

                //        sqlExpression = "INSERT INTO  ProductNames(productName,ProductCategoriaName) VALUES ('" + txtProductName + "','"+ cmbCategoriaProduct.Text + "')";
                //        command = new SQLiteCommand(sqlExpression, connection);

                //        int result = command.ExecuteNonQuery();

                //        connection.Close();

                //    }

                //    #endregion

                //}

                #endregion



            }
        }

        private void txtProductName_GotFocus(object sender, RoutedEventArgs e)
        {
            //setKLName("00000419");
            //txtProductName.SelectionStart = 0;
            //txtProductName.SelectionLength = txtProductName.Text.Length;
            
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

                System.Diagnostics.Debug.WriteLine($"КатРасход,*Цена1ед,Кол-во,ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");

                //AddShoping.productPrice = (txtProductPrice.Text == "") ? float.Parse(txtProductPrice.Text) : 0 ;
                if ((txtProductPrice.Text.Equals("")) || (txtProductPrice.Text.Equals("0")))
                {
                    txtProductPrice.Text = "0";
                    txtItogo.IsEnabled = true;
                    txtItogo.TabIndex = 9;

                    txtProductMassa.TabIndex = 7;
                    cmbProdUnit.TabIndex = 8;
                    txtProductSkidka.TabIndex = 10;
                    btnSaveDB.TabIndex = 11;
                    txtItogo.IsTabStop = true;
                    txtProductPrice.IsTabStop = false;
                    txtProductMassa.Focus();
                }
                else
                {
                    txtItogo.IsEnabled = false;
                    txtItogo.IsTabStop = false;
                    txtProductMassa.TabIndex = 7;
                    cmbProdUnit.TabIndex = 8;
                    txtProductSkidka.TabIndex = 9;
                    btnSaveDB.TabIndex = 10;
                    txtItogo.TabIndex = 11;
                    txtProductMassa.IsTabStop = true;
                    txtProductMassa.Focus();
                }

               //MoveToNextElement(e);


            }
            else if (e.Key.Equals(Key.OemComma))
            {
                e.Handled = true;
            }
            
        }
      

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //System.Diagnostics.Debug.WriteLine($"Текст блок имя {tb.Text}");
            tb.SelectionStart = 0;
            tb.SelectionLength = tb.Text.Length;

        }

        private void txtProductMassa_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"КатРасход,Цена1ед,*Кол-во,ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");

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

            if (e.Key.Equals(Key.Enter) || (e.Key.Equals(Key.Tab)))
            {

                if (txtProductSkidka.Text.Equals("")) txtProductSkidka.Text = "0";

                if (!txtProductSkidka.Text.Equals("0"))
                {
                    float itogo = ((!txtItogo.Text.Equals("0")) || (!txtItogo.Text.Equals(""))) ? float.Parse(txtItogo.Text.Replace(",", "."), CultureInfo.InvariantCulture) : 0;
                    float itogoSkidka = itogo - float.Parse(txtProductSkidka.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    txtItogoSkidka.Text = String.Format("{0:0.##}", itogoSkidka);
                    btnSaveDB.Focus();
                }
                else
                {
                    txtItogoSkidka.Text = txtItogo.Text;
                }

            }
            else if (e.Key == Key.Space) e.Handled = true; //если пробел то не учитываем
            else if (e.Key.Equals(Key.OemComma)) e.Handled = true; //если точка то не учитываем
        }


        private void BtnSaveDB_Click(object sender, RoutedEventArgs e)
        {
            dtShop = (DateTime)dtShoping.SelectedDate;
            string strdtShop;

            string strproductName = cmbProductName.Text;
            string strProductCategoriaName = cmbCategoriaProduct.Text;
            string strproductDateTime = dtShop.ToString("dd.MM.yyyy");

            //IFormatProvider formatZPT = new NumberFormatInfo { NumberDecimalSeparator = "." };
            ////string strproductPrice = txtProductPrice.Text.Replace(",", ".");
            ////float flproductPrice = float.Parse(strproductPrice, formatZPT);
            //string strProductPrice = float.Parse(txtProductPrice.Text.Replace(",", "."),formatZPT).ToString().Replace(",",".");


            //float flproductMassa = float.Parse(txtProductMassa.Text.Replace(".", ","), CultureInfo.InvariantCulture);
            //float flproductRashod = float.Parse(txtItogo.Text.Replace(".", ","), CultureInfo.InvariantCulture);
            //string strShopName = cmbShopName.Text;
            //float flProductDiscont = float.Parse(txtProductSkidka.Text.Replace(".", ","), CultureInfo.InvariantCulture); ;
            //string strproductUnit = cmbProdUnit.Text;
            //string strCategoriaShopping = cmbCatShop.Text;
            //float flItogoskidka = float.Parse(txtItogoSkidka.Text.Replace(".", ","), CultureInfo.InvariantCulture);

            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            //string strproductName = cmbProductName.Text;
            //string strProductCategoriaName = cmbCategoriaProduct.Text;
            //string strproductDateTime = dtShop.ToString("dd.MM.yyyy");
            //IFormatProvider formatZPT = new NumberFormatInfo { NumberDecimalSeparator = "." };
            //float flproductPrice = float.Parse(txtProductPrice.Text.Replace(",", "."), formatZPT);
            //string strProductPrice = flproductPrice.ToString();
            //float flproductMassa = float.Parse(txtProductMassa.Text.Replace(",", "."), formatZPT);
            //float flproductRashod = float.Parse(txtItogo.Text.Replace(",", "."), formatZPT);
            //string strShopName = cmbShopName.Text;
            //float flProductDiscont = float.Parse(txtProductSkidka.Text.Replace(",", "."), formatZPT); ;
            //string strproductUnit = cmbProdUnit.Text;
            //string strCategoriaShopping = cmbCatShop.Text;
            //float flItogoskidka = float.Parse(txtItogoSkidka.Text.Replace(",", "."), formatZPT);


            //сначала заменяем в строке  "," на ".", потом преобразуем его в float и после этого в string  и меняем запятую на точку
            IFormatProvider formatZPT = new NumberFormatInfo { NumberDecimalSeparator = "." };
            string strProductPrice = float.Parse(txtProductPrice.Text.Replace(",", "."), formatZPT).ToString().Replace(",", ".");
            string strproductMassa = float.Parse(txtProductMassa.Text.Replace(",", "."), formatZPT).ToString().Replace(",", ".");
            string strproductRashod = float.Parse(txtItogo.Text.Replace(",", "."), formatZPT).ToString().Replace(",", ".");
            string strShopName = cmbShopName.Text;
            string strProductDiscont = float.Parse(txtProductSkidka.Text.Replace(",", "."), formatZPT).ToString().Replace(",", ".");
            string strproductUnit = cmbProdUnit.Text;
            string strCategoriaShopping = cmbCatShop.Text;
            string strItogoskidka = float.Parse(txtItogoSkidka.Text.Replace(",", "."), formatZPT).ToString().Replace(",", ".");





            #region Записываем данные в БД
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                string sqlExpression;
                SQLiteCommand command;

                sqlExpression = "INSERT INTO  PriceShops(productName,ProductCategoriaName,productDateTime,productPrice,productMassa,productRashod,ShopName,ProductDiscont,productUnit,CategoriaShopping,ItogoSkigka) " +
                    "VALUES ('" +
                    strproductName + "','" +
                    strProductCategoriaName + "','" +
                    strproductDateTime + "'," +
                    strProductPrice + "," +
                    strproductMassa + "," +
                    strproductRashod + ",'" +
                    strShopName + "'," +
                    strProductDiscont + ",'" +
                    strproductUnit + "','" +
                    strCategoriaShopping + "','" +
                    strItogoskidka + "'" +
                    ")";
                command = new SQLiteCommand(sqlExpression, connection);

                int result = command.ExecuteNonQuery();

                if (!result.Equals(0))
                {
                    //txtProductName.Text = ""; 
                    cmbProductName.Text = "";
                    
                    cmbCategoriaProduct.Text = "";
                    txtProductPrice.Text = "";
                    txtProductMassa.Text = "";
                    txtItogo.Text = "";
                    txtProductSkidka.Text = "";
                    cmbProdUnit.Text = "";
                    txtItogoSkidka.Text = "";

                    dtShoping.TabIndex = 1;
                    cmbShopName.TabIndex = 2;
                    cmbCategoriaProduct.TabIndex = 3;
                    cmbProductName.TabIndex = 4;
                    cmbCatShop.TabIndex = 5;
                    txtProductPrice.TabIndex = 6;
                    txtProductMassa.TabIndex = 7;
                    cmbProdUnit.TabIndex = 8;
                    txtProductSkidka.TabIndex = 9;
                    txtItogo.IsTabStop = false;
                    
                    btnSaveDB.TabIndex = 10;
                    txtItogo.TabIndex = 11;
                    txtProductPrice.IsEnabled = true;
                    txtProductPrice.IsTabStop = true;
                    txtItogo.IsTabStop = false;
                    txtItogo.IsEnabled = false;
                    txtItogoSkidka.IsTabStop = false;
                    txtItogoSkidka.IsEnabled = false;

                    System.Diagnostics.Debug.WriteLine($"Сброс-> КатРасход,Цена1ед,Кол-во,ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");


                }

                connection.Close();

            }

            #endregion

            LoadComboBox();
        }

        private void cmbProductName_KeyDown(object sender, KeyEventArgs e)
        {
            #region событие при нажатии на Enter или Tab
            string txtProductName = cmbProductName.Text;
            
            System.Diagnostics.Debug.WriteLine($"Подкласс продукта-> КатРасход,Цена1ед,Кол-во,ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");

            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                if (!cmbProductName.Items.Contains(txtProductName))
                {
                    cmbProductName.Items.Add(txtProductName);


                }

                //cmbCatShop.Focus();
                MoveToNextElement(e);

            }

            #endregion
        }

        private void cmbProductName_GotFocus(object sender, RoutedEventArgs e)
        {
            #region Заполнение даннми при получении фокуса

           

            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                string sqlExpression;
                SQLiteCommand command;


                #region Подкласс продуктов 
                
                sqlExpression = "select productName,ProductCategoriaName from ProductNames where ProductCategoriaName='" + cmbCategoriaProduct.Text + "' order by productName asc";
                    
                
                command = new SQLiteCommand(sqlExpression, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        cmbProductName.Items.Clear();

                        while (reader.Read())
                        {
                            cmbProductName.Items.Add(reader.GetString(0));
                            cmbProductName.Text = cmbProductName.Items[0].ToString();

                        }

                        

                    }

                }
                #endregion

                
                connection.Close();

            }

            #endregion
        }

        private void cmbCatShop_GotFocus(object sender, RoutedEventArgs e)
        {
            #region При получении фокуса если элемент подкласс выбран то получаем в зависимости от подкласса значения и заполняем элемент

            if (!cmbProductName.Text.Equals(""))
            {
                

                using (var connection = new SQLiteConnection("Data Source = product.db"))
                {
                    connection.Open();
                    string sqlExpression;
                    SQLiteCommand command;

                        sqlExpression = (cmbCatShop.Text.Equals(""))&&(cmbCategoriaProduct.Text.Equals(""))
                        ? "select ProductCategoriaName,CategoriaShopping from ProductNames  order by CategoriaShopping asc"
                        : "select ProductCategoriaName,CategoriaShopping from ProductNames where ProductCategoriaName='" + cmbCategoriaProduct.Text + "' order by CategoriaShopping asc";



                    command = new SQLiteCommand(sqlExpression, connection);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            cmbCatShop.Items.Clear();

                            while (reader.Read())
                            {
                                cmbCatShop.Items.Add(reader.GetString(1));
                                

                            }

                            cmbCatShop.Text = cmbCatShop.Items[0].ToString();
                        }

                    }
                   
                    

                    connection.Close();
                }

            }
            #endregion
        }

        private void txtProductItogo_GotFocus(object sender, RoutedEventArgs e)
        {
            //setKLName("00000419");
            txtItogo.SelectionStart = 0;
            txtItogo.SelectionLength = txtItogo.Text.Length;
        }

        private void txtProductItogo_KeyDown(object sender, KeyEventArgs e)
        {
            

            if (e.Key.Equals(Key.Enter) || (e.Key.Equals(Key.Tab)))
            {
                if (txtItogo.Text.Equals("")) txtItogo.Text = "0";
                if ((!txtProductMassa.Text.Equals("0")) || (!txtItogo.Text.Equals("0")))
                {
                   float flMassa = float.Parse(txtProductMassa.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                   float flItogo = float.Parse(txtItogo.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    
                    float Prod1ed = (flItogo) / flMassa;

                    txtProductPrice.Text = String.Format("{0:0.##}", Prod1ed);
                }



                //    //txtProductPrice.IsEnabled = true;
                //    //txtProductPrice.IsTabStop = false;
                //    //txtProductItogo.IsTabStop = true;
                //    //txtProductItogo.IsEnabled = false;
                //    cmbShopName.Focus();


            }
            else if (e.Key.Equals(Key.OemComma))
            {
                e.Handled = true;
            }
        }

        private void cmbCatShop_LostFocus(object sender, RoutedEventArgs e)
        {
            
            
        }
        void MoveToNextElement(KeyEventArgs e)
        {
            ///https://answacode.com/questions/8203329/perehod-k-sleduyushemu-elementu-upravleniya-nazhatiem-klavishi-enter-v-wpf
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }


        private void BtnSave2DB_Click(object sender, RoutedEventArgs e)
        {
            dtShop = (DateTime)dtShoping.SelectedDate;
            string strdtShop;

               
            #region Записываем данные в БД
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                string sqlExpression;
                SQLiteCommand command;

                sqlExpression = "INSERT INTO  PriceShops(productName,ProductCategoriaName,productDateTime,productPrice,productMassa,productRashod,ShopName,ProductDiscont,productUnit,CategoriaShopping,ItogoSkigka) " +
                    "VALUES (@strproductName,@strProductCategoriaName,@strproductDateTime,@strProductPrice,@strproductMassa,@strproductRashod,@strShopName,@strProductDiscont,@strproductUnit,@strCategoriaShopping,@strItogoskidka)";
                command = new SQLiteCommand(sqlExpression, connection);

                SQLiteParameter productName = new SQLiteParameter("@strproductName", cmbProductName.Text);
                command.Parameters.Add(productName);
                SQLiteParameter ProductCategoriaName = new SQLiteParameter("@strProductCategoriaName", cmbCategoriaProduct.Text);
                command.Parameters.Add(ProductCategoriaName);
                SQLiteParameter productDateTime = new SQLiteParameter("@strproductDateTime", dtShop.ToString("dd.MM.yyyy"));
                command.Parameters.Add(productDateTime);
                SQLiteParameter ProductPrice = new SQLiteParameter("@strProductPrice", float.Parse(txtProductPrice.Text.Replace(",", "."), CultureInfo.InvariantCulture));
                command.Parameters.Add(ProductPrice);
                SQLiteParameter productMassa = new SQLiteParameter("@strproductMassa", float.Parse(txtProductMassa.Text.Replace(",", "."), CultureInfo.InvariantCulture));
                command.Parameters.Add(productMassa);
                SQLiteParameter productRashod = new SQLiteParameter("@strproductRashod", float.Parse(txtItogo.Text.Replace(",", "."), CultureInfo.InvariantCulture));
                command.Parameters.Add(productRashod);
                SQLiteParameter ShopName = new SQLiteParameter("@strShopName", cmbShopName.Text);
                command.Parameters.Add(ShopName);
                SQLiteParameter ProductDiscont = new SQLiteParameter("@strProductDiscont", float.Parse(txtProductSkidka.Text.Replace(",", "."), CultureInfo.InvariantCulture));
                command.Parameters.Add(ProductDiscont);
                SQLiteParameter productUnit = new SQLiteParameter("@strproductUnit", cmbProdUnit.Text);
                command.Parameters.Add(productUnit);
                SQLiteParameter CategoriaShopping = new SQLiteParameter("@strCategoriaShopping", cmbCatShop.Text);
                command.Parameters.Add(CategoriaShopping);
                SQLiteParameter Itogoskidka = new SQLiteParameter("@strItogoskidka", float.Parse(txtItogoSkidka.Text.Replace(",", "."), CultureInfo.InvariantCulture));
                command.Parameters.Add(Itogoskidka);

                

                int result = command.ExecuteNonQuery();

                if (!result.Equals(0))
                {
                    //txtProductName.Text = ""; 
                    cmbProductName.Text = "";

                    cmbCategoriaProduct.Text = "";
                    txtProductPrice.Text = "";
                    txtProductMassa.Text = "";
                    txtItogo.Text = "";
                    txtProductSkidka.Text = "";
                    cmbProdUnit.Text = "";
                    txtItogoSkidka.Text = "";

                    dtShoping.TabIndex = 1;
                    cmbShopName.TabIndex = 2;
                    cmbCategoriaProduct.TabIndex = 3;
                    cmbProductName.TabIndex = 4;
                    cmbCatShop.TabIndex = 5;
                    txtProductPrice.TabIndex = 6;
                    txtProductMassa.TabIndex = 7;
                    cmbProdUnit.TabIndex = 8;
                    txtProductSkidka.TabIndex = 9;
                    txtItogo.IsTabStop = false;

                    btnSaveDB.TabIndex = 10;
                    txtItogo.TabIndex = 11;
                    txtProductPrice.IsEnabled = true;
                    txtProductPrice.IsTabStop = true;
                    txtItogo.IsTabStop = false;
                    txtItogo.IsEnabled = false;
                    txtItogoSkidka.IsTabStop = false;
                    txtItogoSkidka.IsEnabled = false;

                    System.Diagnostics.Debug.WriteLine($"Сброс-> КатРасход,Цена1ед,Кол-во,ЕдИзм, ,Итого index={cmbCatShop.TabIndex},{txtProductPrice.TabIndex},{txtProductMassa.TabIndex},{cmbProdUnit.TabIndex}, ,{txtItogo.TabIndex}");


                }

                connection.Close();

            }

            #endregion

            LoadComboBox();
        }

        private void cmbProdUnit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string Value;
            int iVal;

            if (Int32.TryParse(e.Text,out iVal))
            {
                e.Handled = true;
            }
        }

        private void cmbShopName_GotFocus(object sender, RoutedEventArgs e)
        {
            setKLName("00000419"); //Автоматическое переключение на Рус
        }

       

        private void txtProductSkidka_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Ввод только чисел
            int val;
            if (!Int32.TryParse(e.Text,out val))
            {
                e.Handled = true; //отклоняем ввод если не число
            }
        }

    }
}
