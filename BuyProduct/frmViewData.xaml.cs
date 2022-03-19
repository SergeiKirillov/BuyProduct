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
            view1();
            view2();
            ViewCatShop();
            ViewCatName();
            viewProductName();
        }

        private void ViewCatName()
        {
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataTable dTable = new DataTable();
                string sqlQuery = "select DISTINCT ProductCategoriaName from ProductNames order by ProductCategoriaName";

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
                    dgViewClass.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    cmbViewCatShop.ItemsSource = dTable.DefaultView;
                    cmbViewCatShop.DisplayMemberPath = "CategoriaShopping";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                }


            }
        }

        private void viewProductName()
        {
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataTable dTable = new DataTable();
                string sqlQuery = "select DISTINCT productName from ProductNames order by productName";

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dTable);

                if (dTable.Rows.Count > 0)
                {
                    dgViewClass.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    cmbViewProductName.ItemsSource = dTable.DefaultView;
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
            using (var connection = new SQLiteConnection("Data Source = product.db"))
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
                    dgViewProduct.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    
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
                string sqlQuery = "select productDateTime, ProductCategoriaName, productName, productRashod from PriceShops order by productDateTime ASC";

                if (connection.State != ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dSet, "PriceShops");

                dSet.Tables[0].DefaultView.RowFilter = "productName='1 категория'";
                
                

                //DataTable dataTable = dSet.Tables[0].DefaultView.ToTable();

                //DataView dv = new DataView(dSet.Tables["PriceShops"]);
                //dv.Sort = "productDateTime,ProductCategoriaName";
                //dgViewClass.ItemsSource = dv;

                //dv.Sort = "productDateTime ASC";

                //dgViewClass.ItemsSource = dv;

                //dv.RowFilter = "productName='1 категория'";
                //dgViewClass.ItemsSource = dv;





                //if (dTable.Rows.Count > 0)
                //{
                //    dgViewProduct.ItemsSource = dTable.DefaultView;
                //    //dgViewClass.ItemsSource = dTable.AsDataView();

                //}
                //else
                //{
                //    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                //}


            }
        }

    }
}
