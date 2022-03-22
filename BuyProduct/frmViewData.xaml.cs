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
            view2("");
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
                string sqlQuery = "select date(productDateTime) as dtProduct, ProductCategoriaName, productName, productRashod from PriceShops order by dtProduct ASC";

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

        private void view2(string SortID)
        {
            using (var connection = new SQLiteConnection("Data Source = product.db"))
            {
                connection.Open();
                DataSet dSet = new DataSet();
                //select substr(productDateTime,7,4)|| "-" || substr(productDateTime, 4, 2) || "-" || substr(productDateTime, 1, 2) as date1, productName from PriceShops order by date1 DESC
                //string sqlQuery = "select date(substr(productDateTime,7,4)||'-'||substr(productDateTime, 4, 2)||'-'||substr(productDateTime, 1, 2)) as date1, CategoriaShopping, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops order by date1 ASC";

                string sqlQuery = "";
                

                if ((dt1.Text=="")&&(dt2.Text == ""))
                {
                    sqlQuery = "select date(productDateTime) as date1, CategoriaShopping, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops order by date1 DESC";
                }
                else
                {
                    if ((dt2.Text=="")&&(dt1.Text!=""))
                    {
                        string strdt1 = Convert.ToDateTime(dt1.Text).ToString("yyyy-MM-dd");
                        sqlQuery = "select date(productDateTime) as date1, CategoriaShopping, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops where date1='" + strdt1 + "' order by date1 DESC";
                    }
                    else if ((dt2.Text != "") && (dt1.Text != ""))
                    {
                        string strdt1 = Convert.ToDateTime(dt1.Text).ToString("yyyy-MM-dd");
                        string strdt2 = Convert.ToDateTime(dt2.Text).ToString("yyyy-MM-dd");
                        sqlQuery = "select date(productDateTime) as date1, CategoriaShopping, ProductCategoriaName, productName, productPrice, productMassa, productUnit, productRashod from PriceShops where date1 between '" + strdt1 + "' and '"+ strdt2 + "' order by date1 DESC";
                    }
                    
                }
                


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
                if(SortID!="")
                {
                    //dv.RowFilter = "productName='1 категория'";
                    dv.RowFilter = SortID;
                }
                #endregion

                #region Таблица

              

                #endregion



                dgViewClass.ItemsSource = dv;
                dgViewClass.Items.Refresh();

                //dgViewClass.Columns[1].Header = "111";



            }
        }

       

        

       

        private void cmbViewCatShop_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbViewCatShop.Text != "")
            {
                #region Заполняем таблицу при выборк группы продуктов 
                view2("CategoriaShopping='" + cmbViewCatShop.Text + "'");
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
                view2("ProductCategoriaName='" + cmbViewCatName.Text + "'");
                #endregion


                #region Заполняем таблицу подгруппы продуктов
                viewProductName(cmbViewCatName.Text);
                #endregion



            }
        }

        private void cmbViewProductName_LostFocus(object sender, RoutedEventArgs e)
        {
            view2("productName='" + cmbViewProductName.Text + "'");
        }

       
    }
}
