using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyProduct
{


    /// <summary>
    /// https://metanit.com/sharp/wpf/14.php Валидация данных
    /// </summary>
    public class PriceShop
    {
        private string tovarName;
        private string tovarCategoriaName;
        private DateTime tovarDatetime;
        private float tovarPrice1Poz;
        private float tovarMassa;
        private string tovarUnit;
        private float tovarPrice;
        private float tovarDiscont;
        private string CategoriaShop;
        private string ShopNames;

        

        public string productName 
        {
            get{ return tovarName;}
            set { tovarName = value;}
        }

        public string ProductCategoriaName
        {
            get { return tovarCategoriaName; }
            set { tovarCategoriaName = value; }
        }

        public string productDateTime
        {
            get { return tovarDatetime.ToString(); }
            set { tovarDatetime = Convert.ToDateTime(value); }
        }

        public float productPrice
        {
            get { return tovarPrice1Poz; }
            set { tovarPrice1Poz = value; }
        }



        public float productMassa
        {
            get 
            { 
                return tovarMassa; 
            }
            set { tovarMassa = value; }
        }

        public string productUnit
        {
            get { return tovarUnit; }
            set { tovarUnit = value; }
        }

        public float ProductDiscont
        {
            get { return tovarDiscont; }
            set { tovarDiscont = value; }
        }

        public float productRashod
        {
            get { return tovarPrice= (tovarPrice1Poz * tovarMassa) - tovarDiscont; }
            
        }

        public string CategoriaShopping
        {
            get { return CategoriaShop; }
            set { CategoriaShop = value; }
        }

        public string ShopName
        {
            get { return ShopNames; }
            set { ShopNames = value; }
        }










    }
}
