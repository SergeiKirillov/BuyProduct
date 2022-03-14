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
                DataTable dTable = new DataTable();
                string sqlQuery= "select DISTINCT ProductCategoriaName from ProductNames order by ProductCategoriaName";

                if (connection.State!=ConnectionState.Open)
                {
                    System.Diagnostics.Debug.WriteLine("База не найдена!!");
                    return;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                adapter.Fill(dTable);

                if (dTable.Rows.Count>0)
                {
                    dgViewClass.ItemsSource = dTable.DefaultView;
                    //dgViewClass.ItemsSource = dTable.AsDataView();
                    cmbViewCat.ItemsSource = dTable.DefaultView;
                    cmbViewCat.DisplayMemberPath = "ProductCategoriaName";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("В таблице нету данных для просмотра");
                }


            }
   
            
        }
    }
}
