using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(
    typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer)
)]

namespace PutCount
{
    public class Function
    {
        private static readonly HttpClient client = new HttpClient();

        public static Dictionary<string, string> ALL_HEADERS = new Dictionary<string, string>()
        {
            { "Access-Control-Allow-Headers", "Content-Type,Authorization" },
            { "Access-Control-Allow-Origin", "https://vani.kulkarnisworklife.uk" },
            { "Access-Control-Allow-Methods", "GET" },
            { "Access-Control-Allow-Credentials", "*" }
        };

        [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
        public async Task<APIGatewayProxyResponse> FunctionHandler(
            APIGatewayProxyRequest request,
            ILambdaContext context
        )
        {
            Console.WriteLine($"HTTP Method: {request.HttpMethod}");

            var tableName = "Cloud-Resume-Challenge";
            var region = RegionEndpoint.GetBySystemName("us-east-1");

            Console.WriteLine($"Region: {region}");
            Console.WriteLine($"Table:  {tableName}");

            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = region,
            };

            var dbClient = new AmazonDynamoDBClient(clientConfig);
            Console.WriteLine($"dbClient: {dbClient}");
            var id = "VK";
            var updateRequest = new UpdateItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    {
                        "Id",
                        new AttributeValue { S = id }
                    }
                },
                UpdateExpression = "ADD VisitorCount :incrementExp",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {
                        ":incrementExp",
                        new AttributeValue { N = "1" }
                    }
                }
            };

            Console.WriteLine("updateRequest being made");
            var updateResponse = await dbClient.UpdateItemAsync(updateRequest);
            Console.WriteLine(
                $"updateResponse done, HttpStatusCode = {updateResponse.HttpStatusCode}"
            );

            var getRequest = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    {
                        "Id",
                        new AttributeValue { S = id }
                    }
                },
            };
            var getResponse = await dbClient.GetItemAsync(getRequest);
            Console.WriteLine($"getResponse: {getResponse}");
            Console.WriteLine($"getResponse done, HttpStatusCode = {getResponse.HttpStatusCode}");
            var attributeMap = getResponse.Item;
            int counter = 0;
            if (attributeMap == null || attributeMap.Count == 0)
            {
                Console.WriteLine("attributeMap is null");
            }
            else
            {
                counter = Int32.Parse(attributeMap["VisitorCount"].N);
                Console.WriteLine("attributeMap is NOT null");
            }

            return new APIGatewayProxyResponse()
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(
                    new Dictionary<string, string> { { "count", counter.ToString() } }
                ),
                Headers = ALL_HEADERS
            };
        }
    }
}
