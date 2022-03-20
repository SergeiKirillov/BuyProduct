using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

namespace BuyProduct
{
    /// <summary>
    /// Логика взаимодействия для frmViewData.xaml
    /// </summary>
    public partial class frmViewData : Window
    {
        

        public frmViewData()
        {
        
            InitializeComponent();
            //начало просмотра данныъ
            //view1();
            view2();
            ViewCatShop();
            ViewCatName("");
            viewProductName("");
        }

        private void ViewCatName(string si)
        {
            string sqlQuery = "";

            cmbViewCatName.ItemsSource=null;

            if (si != "")
            {
                sqlQuery = "select DISTINCT ProductCategoriaName from ProductNames where CategoriaShopping='" + cmbViewCatShop.Text + "'  order by ProductCategoriaName";
            }
            else
            {
                sqlQuery = "select DISTINCT ProductCategoriaName from ProductNames order by ProductCategoriaName";
            }


            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataTable dTable = new DataTable();


                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    //dgViewClass.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    cmbViewCatName.ItemsSource = dTable.DefaultView;
                    cmbViewCatName.Items.Refresh();
                    cmbViewCatName.DisplayMemberPath = "ProductCategoriaName";

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                }


            }
            
        }

        private void ViewCatShop()
        {
            cmbViewCatShop.ItemsSource = null;
            cmbViewCatShop.Items.Refresh();

            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataTable dTable = new DataTable();
                string sqlQuery = "select DISTINCT CategoriaShopping from ProductNames order by CategoriaShopping";

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dTable);

                

                if (dTable.Rows.Count > 0)
                {
                    //dgViewClass.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    cmbViewCatShop.ItemsSource = dTable.DefaultView;
                    cmbViewCatShop.Items.Refresh();
                    cmbViewCatShop.DisplayMemberPath = "CategoriaShopping";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                }


            }
        }

        private void viewProductName(string si)
        {
            string sqlQuery;
            cmbViewProductName.ItemsSource = null;

            if (si=="")
            {
                sqlQuery = "select DISTINCT productName from ProductNames order by productName";
            }
            else
            {
                sqlQuery = "select DISTINCT productName from ProductNames where ProductCategoriaName='"+ cmbViewCatName.Text + "'  order by productName";
            }

            using (SQLiteConnection connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataTable dTable = new DataTable();
                

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    //dgViewClass.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    cmbViewProductName.ItemsSource = dTable.DefaultView;
                    cmbViewProductName.Items.Refresh();
                    cmbViewProductName.DisplayMemberPath = "productName";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                }


            }


        }

        private void view1()
        {
           
            using (SQLiteConnection connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataTable dTable = new DataTable();
                string sqlQuery = "select productDateTime, ProductCategoriaName, productName, productRashod from PriceShops order by productDateTime ASC";

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    //dgViewProduct.ItemsSource = dTable.DefaultView;
                    
                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                }


            }
        }

        private void view2()
        {
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataSet dSet = new DataSet();
                string sqlQuery = "select productDateTime, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops order by productDateTime ASC";

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dSet, "PriceShops");

                //dSet.Tables[0].DefaultView.RowFilter = "productName='1 категория'";



                //DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();

                //DataView dv = new DataView(dSet.Tables["PriceShops"]);
                //dv.Sort = "productDateTime,ProductCategoriaName";


                //dv.Sort = "productDateTime ASC";
                #region 3
                DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();
                DataView dv = new DataView(dataTable);
                dv.RowFilter = "productName='1 категория'";

                #endregion

                dgViewClass.ItemsSource = dv;
                dgViewClass.Items.Refresh();

                
            }
        }

       

        

       

        private void cmbViewCatShop_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbViewCatShop.Text != "")
            {
                #region Заполняем таблицу при выборк группы продуктов 
                using (var connection = new SQLiteConnection("Data Source = product.db"))
                {
                    connection.Open();
                    DataSet dSet = new DataSet();
                    string sqlQuery = "select productDateTime, ProductCategoriaName, CategoriaShopping, productName, productPrice, productMassa, productUnit, productRashod from PriceShops order by productDateTime ASC";

                    if (connection.State != ConnectionState.Open)
                    {
                        System.Diagnostics.Debug.WriteLine("База не найдена!!");
                        return;
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                    adapter.Fill(dSet, "PriceShops");

                    //dSet.Tables[0].DefaultView.RowFilter = "productName='1 категория'";



                    //DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();

                    //DataView dv = new DataView(dSet.Tables["PriceShops"]);
                    //dv.Sort = "productDateTime,ProductCategoriaName";


                    //dv.Sort = "productDateTime ASC";
                    #region 3
                    DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();
                    DataView dv = new DataView(dataTable);

                    //ComboBoxItem cb1 = (ComboBoxItem)cmbViewCatShop.SelectedItem;
                    //string Val = cb1.Content.ToString();

                    string query = "CategoriaShopping='" + cmbViewCatShop.Text + "'";
                    dv.RowFilter = query;

                    #endregion

                    dgViewClass.ItemsSource = dv;
                    dgViewClass.Items.Refresh();


                }
                #endregion


                #region Заполняем таблицу подгруппы продуктов
                ViewCatName(cmbViewCatShop.Text);
                #endregion



            }
        }

        private void cmbViewCatName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbViewCatName.Text != "")
            {
                #region Заполняем таблицу при выборк группы продуктов 
                using (var connection = new SQLiteConnection("Data Source = product.db"))
                {
                    connection.Open();
                    DataSet dSet = new DataSet();
                    string sqlQuery = "select productDateTime, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops order by productDateTime ASC";

                    if (connection.State != ConnectionState.Open)
                    {
                        System.Diagnostics.Debug.WriteLine("База не найдена!!");
                        return;
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                    adapter.Fill(dSet, "PriceShops");

                    #region 3
                    DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();
                    DataView dv = new DataView(dataTable);

                    string query = "ProductCategoriaName='" + cmbViewCatName.Text + "'";
                    dv.RowFilter = query;

                    #endregion

                    dgViewClass.ItemsSource = dv;
                    dgViewClass.Items.Refresh();


                }
                #endregion


                #region Заполняем таблицу подгруппы продуктов
                viewProductName(cmbViewCatName.Text);
                #endregion



            }
        }

        private void cmbViewProductName_LostFocus(object sender, RoutedEventArgs e)
        {
            #region Заполняем таблицу продуктов 
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataSet dSet = new DataSet();
                string sqlQuery = "select productDateTime, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops order by productDateTime ASC";

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dSet, "PriceShops");

                #region 3
                DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();
                DataView dv = new DataView(dataTable);

                string query = "productName='" + cmbViewProductName.Text + "'";
                dv.RowFilter = query;

                #endregion

                dgViewClass.ItemsSource = dv;
                dgViewClass.Items.Refresh();


            }
            #endregion
        }
    }
}
