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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Part1
{
    public enum StockType
    {
        Low,
        Normal,
        Overstocked
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NORTHWNDEntities northwnd = new NORTHWNDEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var query = from a in northwnd.Suppliers
                        orderby a.CompanyName
                        select new
                        {
                            CompanyName = a.CompanyName,
                            SupplierID = a.SupplierID,
                            Country = a.Country
                        };

            var query2 = query.OrderBy(s => s.Country).Select(s => s.Country);

            var query3 = from a in northwnd.Products
                         orderby a.ProductName
                         select a.ProductName;

            stockLbx.ItemsSource = Enum.GetNames(typeof(StockType));
            suppliersLbx.ItemsSource = query.ToList();
            countryLbx.ItemsSource = query2.ToList().Distinct();
            productsLbx.ItemsSource = query3.ToList();

        }

        private void stockLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = stockLbx.SelectedItem as string;
            var query = from a in northwnd.Products
                        where a.UnitsInStock < 50
                        orderby a.ProductName
                        select a.ProductName;
            switch(selection)
            {
                case "Low":
                    break;
                case "Normal":
                    query = from a in northwnd.Products
                            where a.UnitsInStock >= 50 && a.UnitsInStock <= 100
                            orderby a.ProductName
                            select a.ProductName;
                    break;
                case "Overstocked":
                    query = from a in northwnd.Products
                            where a.UnitsInStock > 100
                            orderby a.ProductName
                            select a.ProductName;
                    break;
            }

            productsLbx.ItemsSource = query.ToList();
        }

        private void countryLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string country = countryLbx.SelectedItem as string;

            var query = from a in northwnd.Products
                        where a.Supplier.Country == country
                        orderby a.ProductName
                        select a.ProductName;

            productsLbx.ItemsSource = query.ToList();
        }

        private void suppliersLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(suppliersLbx.SelectedValue);

            var query = from a in northwnd.Products
                        where a.SupplierID == id
                        orderby a.ProductName
                        select a.ProductName;

            productsLbx.ItemsSource = query.ToList();
        }
    }
}
