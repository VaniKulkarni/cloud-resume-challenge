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

namespace GetCount
{
    public class Function
    {
        private static readonly HttpClient client = new HttpClient();

        public static Dictionary<string, string> ALL_HEADERS = new Dictionary<string, string>()
        {
            { "Access-Control-Allow-Headers", "Content-Type,Authorization" },
            { "Access-Control-Allow-Origin", "*" },
            { "Access-Control-Allow-Methods", "*" },
            { "Access-Control-Allow-Credentials", "*" }
        };

        private static async Task<string> GetCallingIP()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

            var msg = await client
                .GetStringAsync("http://checkip.amazonaws.com/")
                .ConfigureAwait(continueOnCapturedContext: false);

            return msg.Replace("\n", "");
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(
            APIGatewayProxyRequest apigProxyEvent,
            ILambdaContext context
        )
        {
            var id = "VK";

            var tableName = "Cloud-Resume-Challenge";
            var region = RegionEndpoint.GetBySystemName("us-east-1");

            Console.WriteLine($"Region: {region}");
            Console.WriteLine($"Table:  {tableName}");

            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = region,
            };
            var dbClient = new AmazonDynamoDBClient(clientConfig);

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

            var output = $"getResponse db code {getResponse.HttpStatusCode}";

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
