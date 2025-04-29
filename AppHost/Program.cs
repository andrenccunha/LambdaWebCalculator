#pragma warning disable CA2252

using Amazon;

var builder = DistributedApplication.CreateBuilder(args);
var awsConfig = builder.AddAWSSDKConfig()
						.WithProfile("lab")
						.WithRegion(RegionEndpoint.USEast1);

var awsResources = builder.AddAWSCloudFormationTemplate("AWSResources", "AWSTemplate.json")
						  .WithReference(awsConfig);

var localDynamoDB = builder.AddAWSDynamoDBLocal("DynamoDBLocal",
		new Aspire.Hosting.AWS.DynamoDB.DynamoDBLocalOptions() { LocalStorageDirectory = "./db" });

var addFunction = builder.AddAWSLambdaFunction<Projects.LambdaWebCalculator>("AddFunction",
		lambdaHandler: "LambdaWebCalculator::LambdaWebCalculator.Functions::AddFunctionHandler");
var minusFunction = builder.AddAWSLambdaFunction<Projects.LambdaWebCalculator>("MinusFunction",
		lambdaHandler: "LambdaWebCalculator::LambdaWebCalculator.Functions::MinusFunctionHandler");
var multiplyFunction = builder.AddAWSLambdaFunction<Projects.LambdaWebCalculator>("MultipyFunction",
		lambdaHandler: "LambdaWebCalculator::LambdaWebCalculator.Functions::MultiplyFunctionHandler");
var divideFunction = builder.AddAWSLambdaFunction<Projects.LambdaWebCalculator>("DivideFunction",
		lambdaHandler: "LambdaWebCalculator::LambdaWebCalculator.Functions::DivideFunctionHandler")
	.WithReference(addFunction);

var dynamoDBFunction = builder.AddAWSLambdaFunction<Projects.LogToDynamoDB>("LogToDynamoDB",
		lambdaHandler: "LogToDynamoDB::LogToDynamoDB.Function::FunctionHandler")
	.WithReference(localDynamoDB)
	.WithReference(awsResources);

builder.AddAWSAPIGatewayEmulator("APIGatewayEmulator", Aspire.Hosting.AWS.Lambda.APIGatewayType.HttpV2)
		.WithReference(addFunction, Aspire.Hosting.AWS.Lambda.Method.Get, "/add/{x}/{y}")
		.WithReference(minusFunction, Aspire.Hosting.AWS.Lambda.Method.Get, "/minus/{x}/{y}")
		.WithReference(multiplyFunction, Aspire.Hosting.AWS.Lambda.Method.Get, "/multiply/{x}/{y}")
		.WithReference(divideFunction, Aspire.Hosting.AWS.Lambda.Method.Get, "/divide/{x}/{y}");

builder.Build().Run();
