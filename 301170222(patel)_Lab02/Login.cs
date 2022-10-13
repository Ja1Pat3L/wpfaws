using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301170222_patel__Lab02
{
    internal class Login
    {

        AmazonDynamoDBClient client;
        BasicAWSCredentials credentials;
        string tableName = "LoginTable";

        public Login()
        {
            credentials = new BasicAWSCredentials(
                                 ConfigurationManager.AppSettings["accessId"],
                                 ConfigurationManager.AppSettings["secretKey"]);

            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
        }
        public async Task CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="Id",
                        AttributeType="S"
                    },
                     new AttributeDefinition
                    {
                        AttributeName="Password",
                        AttributeType="S"
                    },


                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName="Id",
                        KeyType="HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName="Password",
                        KeyType="RANGE"
                    },


                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 2,
                    WriteCapacityUnits = 1
                }
            };

            try
            {
                var response = await client.CreateTableAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) Console.WriteLine("Table created successfully");

            }
            catch (InternalServerErrorException iee)
            {
                Console.WriteLine("An error occurred on the server side " + iee.Message);
            }
            catch (LimitExceededException lee)
            {
                Console.WriteLine("you are creating a table with one or more secondary indexes+ " + lee.Message);
            }

        }

        public async Task AddLoginCredentials()
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {

                    { "Id", new AttributeValue{S="jaipatel01@hotmail.com" } },
                    { "Password", new AttributeValue{S="psw01" } }
                }
            };

            try
            {
                var response = await client.PutItemAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) Console.WriteLine("Item added successfully!");
            }
            catch (InternalServerErrorException iee)
            { Console.WriteLine("An error occurred on the server side " + iee.Message); }

            catch (ResourceNotFoundException ex)
            { Console.WriteLine("The operation tried to access a nonexistent table or index. "); }
        }
     
    }
}