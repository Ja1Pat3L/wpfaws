using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace _301170222_patel__Lab02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IDynamoDBContext DDBContext { get; set; }
        Amazon.DynamoDBv2.DocumentModel.Table userTable;
        String userName;
        String password;
        String tableName = "LoginTable";
        AmazonDynamoDBClient client;
        BasicAWSCredentials credentials;
        public MainWindow()
        {
            InitializeComponent();
            Login login = new Login();
            login.CreateTable();
            login.AddLoginCredentials();
            credentials = new BasicAWSCredentials(
                                 ConfigurationManager.AppSettings["accessId"],
                                 ConfigurationManager.AppSettings["secretKey"]);

            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            userTable = Amazon.DynamoDBv2.DocumentModel.Table.LoadTable(client, tableName, DynamoDBEntryConversion.V2);
        }

        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            userName = Textbox_UserName.Text;
            password = Textbox_Password.Text;
            CredentialChecker();

            // GetAllLogins();
        }
        private async void CredentialChecker()
        {


            GetItemRequest request = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                     { "Id", new AttributeValue{S=userName} },
                    { "Password", new AttributeValue{ S=password} },

                }
            };

            try
            {
                var response = await client.GetItemAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Item.Count > 0)
                    {
                        MessageBox.Show("Login Successful");
                        Bookshelf bb = new Bookshelf(userName);
                        bb.Show();
                        Hide();
                        Console.WriteLine("Item(s) retrieved successfully");

                    }
                    else
                    {
                        MessageBox.Show("Wrong Credentials");
                    }
                }
            }
            catch (InternalServerErrorException iee)
            { Console.WriteLine("An error occurred on the server side " + iee.Message); }

            catch (ResourceNotFoundException ex)
            {
            }

        }
    }
}
