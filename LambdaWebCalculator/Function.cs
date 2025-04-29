using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaWebCalculator;

public class Functions
{

	public APIGatewayHttpApiV2ProxyResponse AddFunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
	{
		context.Logger.LogDebug("Starting adding operation");
		var x = (int)Convert.ChangeType(request.PathParameters["x"], typeof(int));
		var y = (int)Convert.ChangeType(request.PathParameters["y"], typeof(int));
		var sum = x + y;
		context.Logger.LogInformation("Adding {x} with {y} is {sum}", x, y, sum);
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

	public APIGatewayHttpApiV2ProxyResponse MinusFunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
	{
		context.Logger.LogDebug("Starting subtracting operation");
		var x = (int)Convert.ChangeType(request.PathParameters["x"], typeof(int));
		var y = (int)Convert.ChangeType(request.PathParameters["y"], typeof(int));
		var difference = x - y;
		context.Logger.LogInformation("Subtracting {y} from {x} is {difference}", x, y, difference);
		var response = new APIGatewayHttpApiV2ProxyResponse
		{
			StatusCode = 200,
			Headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" }
			},
			Body = difference.ToString()
		};
		return response;
	}

	public APIGatewayHttpApiV2ProxyResponse MultiplyFunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
	{
		context.Logger.LogDebug("Starting multiplication operation");
		var x = (int)Convert.ChangeType(request.PathParameters["x"], typeof(int));
		var y = (int)Convert.ChangeType(request.PathParameters["y"], typeof(int));
		var product = x * y;
		context.Logger.LogInformation("Multiplying {x} with {y} is {product}", x, y, product);
		var response = new APIGatewayHttpApiV2ProxyResponse
		{
			StatusCode = 200,
			Headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" }
			},
			Body = product.ToString()
		};
		return response;
	}

	public APIGatewayHttpApiV2ProxyResponse DivideFunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
	{
		context.Logger.LogDebug("Starting division operation");
		var x = (int)Convert.ChangeType(request.PathParameters["x"], typeof(int));
		var y = (int)Convert.ChangeType(request.PathParameters["y"], typeof(int));
		var quotient = x / y;
		context.Logger.LogInformation("Dividing {x} by {y} is {quotient}", x, y, quotient);
		var response = new APIGatewayHttpApiV2ProxyResponse
		{
			StatusCode = 200,
			Headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" }
			},
			Body = quotient.ToString()
		};
		return response;
	}
}
