using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
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

namespace _301170222_patel__Lab02
{

    public partial class PdfViewer : Window
    {
        AmazonS3Client client;
        BasicAWSCredentials credentials;
        private string userName;
        public PdfViewer(string bookName, string user)
        {
            InitializeComponent();



            userName = user;



            credentials = new BasicAWSCredentials(
            ConfigurationManager.AppSettings["accessId"],
            ConfigurationManager.AppSettings["secretKey"]);



            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(GetBook1(bookName));
            pdfViewer.Load(loadedDocument);
        }



        private MemoryStream GetBook1(string book)
        {

            try
            {
                GetObjectRequest request = new GetObjectRequest();
                request.BucketName = "awsbookshelf";
                request.Key = book;
                GetObjectResponse response = client.GetObjectAsync(request).Result;



                MemoryStream docStream = new MemoryStream();
                response.ResponseStream.CopyTo(docStream);
                return docStream;      
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw amazonS3Exception;
            }
        }



        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Bookshelf bs = new Bookshelf(userName);



            bs.Show();

        }
    
    }
}