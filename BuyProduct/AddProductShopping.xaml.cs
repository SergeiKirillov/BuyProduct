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

namespace BuyProduct
{
    /// <summary>
    /// Логика взаимодействия для AddProductShopping.xaml
    /// </summary>
    public partial class AddProductShopping : Window
    {
        public AddProductShopping()
        {
            InitializeComponent();

            dtShoping.Text = DateTime.Now.ToString();

            LoadComboBox();

        }

        private void LoadComboBox()
        {
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
                sqlExpression = "select name,City from ShopName order by name asc";
                command = new SQLiteCommand(sqlExpression, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cmbShopName.Items.Add(reader.GetString(1)+","+ reader.GetString(0));
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
            
        }

        
    }
}
