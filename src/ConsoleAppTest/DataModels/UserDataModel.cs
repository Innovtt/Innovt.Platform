using Amazon.DynamoDBv2.DataModel;
using ConsoleAppTest.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest.DataModels
{
    [DynamoDBTable("Users")]
    public class UserDataModel:User
    {
        [DynamoDBProperty("Contacts")]
        public new List<Contact> Contacts { get; set; }

    }
}
