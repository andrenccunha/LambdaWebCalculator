using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LogToDynamoDB;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
		context.Logger.LogDebug("Starting operation");
		var x = (int)Convert.ChangeType(request.PathParameters["x"], typeof(int));
		var y = (int)Convert.ChangeType(request.PathParameters["y"], typeof(int));
		var sum = x + y;
		context.Logger.LogInformation("Adding {x} with {y} is {sum}", x, y, sum);


		await SaveToDynamoDBAsync(new Message
		{
			uuid = Guid.NewGuid().ToString(),
			text = $"Adding {x} with {y} is {sum}"
		}, context);

		await SendToSqsAsync(new Message
		{
			uuid = Guid.NewGuid().ToString(),
			text = $"Adding {x} with {y} is {sum}"
		}, context);

		var response = new APIGatewayHttpApiV2ProxyResponse
		{
			StatusCode = 200,
			Headers = new Dictionary<string, string>
			{

				{ "Content-Type", "application/json" }
			},
			Body = sum.ToString()
		};
		return response;
	}

	private async Task SaveToDynamoDBAsync(Message message, ILambdaContext context)
	{
		// Configura o cliente DynamoDB
		using var client = new AmazonDynamoDBClient();

		// Configura o contexto do DynamoDB
		var dynamoDbContext = new DynamoDBContext(client);

		try
		{
			// Salva o objeto no DynamoDB
			await dynamoDbContext.SaveAsync(message);
			context.Logger.LogInformation($"Apple message com notificationUUID {message.uuid} salvo no DynamoDB com sucesso.");
		}
		catch (Exception ex)
		{
			context.Logger.LogError($"Erro ao salvar no DynamoDB: {ex.Message}");
			throw;
		}
	}

	//send message to sqs
	private async Task SendToSqsAsync(Message message, ILambdaContext context)
	{
		// Configura o cliente SQS
		using var sqsClient = new AmazonSQSClient();
		// Configura a URL da fila SQS
		var queueUrl = Environment.GetEnvironmentVariable("AWS__Resources__MessageQueueUrl");
		try
		{
			// Envia a mensagem para a fila SQS
			var sendMessageRequest = new SendMessageRequest
			{
				QueueUrl = queueUrl,
				MessageBody = message.text
			};
			await sqsClient.SendMessageAsync(sendMessageRequest);
			context.Logger.LogInformation($"Mensagem enviada para a fila SQS com notificationUUID {message.uuid}.");
		}
		catch (Exception ex)
		{
			context.Logger.LogError($"Erro ao enviar mensagem para a fila SQS: {ex.Message}");
			throw;
		}
	}
}
