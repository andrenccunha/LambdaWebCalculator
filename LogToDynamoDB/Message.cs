using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogToDynamoDB
{
	[DynamoDBTable("messageTable")]
	internal class Message
	{
		[DynamoDBHashKey]
		public string uuid { get; set; }

		[DynamoDBProperty]
		public string text { get; set; }
	}
}
