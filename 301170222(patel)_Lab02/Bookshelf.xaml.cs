using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _301170222_patel__Lab02
{
    /// <summary>
    /// Interaction logic for Bookshelf.xaml
    /// </summary>
    public partial class Bookshelf : Window
    {
        string userName;
        long isbn1, isbn2, isbn3;
        long isbn01, isbn02, isbn03;
        string book;
        Table bookShelf;
        string tableName = "Books";
        AmazonDynamoDBClient client;
        BasicAWSCredentials credentials;
        Stack<string> stack;
        public Bookshelf(string user)
        {
            InitializeComponent();
            userName = user;
            bookShelf_label.Content = "Hello " + user; 
            credentials = new BasicAWSCredentials(
            ConfigurationManager.AppSettings["accessId"],
            ConfigurationManager.AppSettings["secretKey"]);



            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            bookShelf = Table.LoadTable(client, tableName, DynamoDBEntryConversion.V2);
            loadDataAsync();
        }



        public async Task loadDataAsync()
        {
            var request = new ScanRequest

            {
                TableName = "Books",

            };

            var response = client.ScanAsync(request);
            var result = response.Result.Items;
            int x = 0;
            string title1 = "", title2 = "", title3 = "";
            DateTime time1 = DateTime.Now, time2 = DateTime.Now, time3 = DateTime.Now;
            foreach (Dictionary<string, AttributeValue> item in result)
            {

                string k = item.ElementAt(1).Value.S.ToString();
                if (k.Equals(userName))
                {
                    string title = item.ElementAt(0).Value.S.ToString();

                    if (x == 0)
                    {
                        title1 = item.ElementAt(0).Value.S.ToString();
                        string h = item.ElementAt(3).Value.N.ToString();
                        isbn1 = long.Parse(h);
                        string dt1 = item.ElementAt(2).Value.S.ToString();
                        time1 = DateTime.Parse(dt1);
                    }
                    if (x == 1)
                    {
                        title2 = item.ElementAt(0).Value.S.ToString();
                        string h = item.ElementAt(3).Value.N.ToString();
                        isbn2 = long.Parse(h);
                        string dt1 = item.ElementAt(2).Value.S.ToString();
                        time2 = DateTime.Parse(dt1);
                    }
                    if (x == 2)
                    {
                        title3 = item.ElementAt(0).Value.S.ToString();
                        string h = item.ElementAt(3).Value.N.ToString();
                        isbn3 = long.Parse(h);
                        string dt1 = item.ElementAt(2).Value.S.ToString();
                        time3 = DateTime.Parse(dt1);
                    }
                    x++;
                }
            }
            if (DateTime.Compare(time1, time2) > 0 && DateTime.Compare(time2, time3) > 0)
            {
                button_book1.Content = title1;
                button_book2.Content = title2;
                button_book3.Content = title3;
                isbn01 = isbn1;
                isbn02 = isbn2;
                isbn03 = isbn3;




            }
            else if (DateTime.Compare(time2, time1) > 0 && DateTime.Compare(time1, time3) > 0)
            {
                button_book1.Content = title2;
                button_book2.Content = title1;
                button_book3.Content = title3;
                isbn01 = isbn2;
                isbn02 = isbn1;
                isbn03 = isbn3;



            }
            else if (DateTime.Compare(time3, time2) > 0 && DateTime.Compare(time2, time1) > 0)
            {
                button_book1.Content = title3;
                button_book2.Content = title2;
                button_book3.Content = title1;
                isbn01 = isbn3;
                isbn02 = isbn2;
                isbn03 = isbn1;




            }



            else if (DateTime.Compare(time1, time3) > 0 && DateTime.Compare(time3, time2) > 0)
            {
                button_book1.Content = title1;
                button_book2.Content = title3;
                button_book3.Content = title2;
                isbn01 = isbn1;
                isbn02 = isbn3;
                isbn03 = isbn2;


            }



            else if (DateTime.Compare(time3, time1) > 0 && DateTime.Compare(time1, time2) > 0)
            {

                button_book1.Content = title3;
                button_book2.Content = title1;
                button_book3.Content = title2;
                isbn01 = isbn3;
                isbn02 = isbn1;
                isbn03 = isbn2;

            }
            else if (DateTime.Compare(time2, time3) > 0 && DateTime.Compare(time3, time1) > 0)
            {
                stack = new Stack<string>();
                stack.Clear();
                button_book1.Content = title2;
                button_book2.Content = title3;
                button_book3.Content = title1;
                isbn01 = isbn2;
                isbn02 = isbn3;
                isbn03 = isbn1;



            }
            else
            {



                button_book1.Content = title1;
                button_book2.Content = title2;
                button_book3.Content = title3;
                isbn01 = isbn1;
                isbn02 = isbn2;
                isbn03 = isbn3;



            }

        }



        private void bt_book1_Click(object sender, RoutedEventArgs e)
        {
            String title = button_book1.Content.ToString();
            string dt = "";
            title = button_book1.Content.ToString();
            string bookName = title + ".pdf";
            PdfViewer pdfViewer = new PdfViewer(bookName, userName);
            pdfViewer.Show();
           
            dt = DateTime.Now.ToString();



            Table table = Table.LoadTable(client, "Books");
            var book = new Document();

            book["Id"] = userName; 
            book["ISBN"] = isbn01; 
            book["DateTime"] = dt;
            book["Title"] = title; ;

            table.UpdateItemAsync(book);

        }




        private void bt_book2_Click(object sender, RoutedEventArgs e)
        {
            String title = button_book2.Content.ToString(); ;
            string dt = "";

            string bookName = title + ".pdf";
            PdfViewer pdfViewer = new PdfViewer(bookName, userName);
            pdfViewer.Show();

            dt = DateTime.Now.ToString();



            Table table = Table.LoadTable(client, "Books");



            var book = new Document();

            book["Id"] = userName; 
            book["ISBN"] = isbn02; 
            book["Title"] = title;



            table.UpdateItemAsync(book);
        }

        private void bt_book3_Click(object sender, RoutedEventArgs e)
        {
            String title = button_book3.Content.ToString(); ;
            string dt = "";
            string bookName = title + ".pdf";
            PdfViewer pdfViewer = new PdfViewer(bookName, userName);
            pdfViewer.Show();

            dt = DateTime.Now.ToString();



            Table table = Table.LoadTable(client, "Books");



            var book = new Document();
            
            book["Id"] = userName; 
            book["ISBN"] = isbn03; 
            book["DateTime"] = dt;
            book["Title"] = title;
            table.UpdateItemAsync(book);
            UpdateLayout();
        }



        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        }
    }




