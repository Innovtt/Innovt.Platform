// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Migration
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Data.SqlClient;
using Dapper;

namespace Innovt.Data.Migration
{
    public static class SecurityMigration
    {
        public static void CreateTablesIfNotExist(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            var query = @" /****** Object:  Table [dbo].[Permission] Script Date: 10/1/2018 2:58:00 PM ******/
                          IF OBJECT_ID (N'Permission', N'U') IS NULL 
                           BEGIN
                            CREATE TABLE [dbo].[Permission](
	                            [Id] [int] IDENTITY(1,1) NOT NULL,
	                            [Domain] [nvarchar](30) NOT NULL,
	                            [Name] [nvarchar](50) NOT NULL,
	                            [Resource] [nvarchar](300) NOT NULL,
                             CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
                            (
	                            [Id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]
                            END

                            /****** Object:  Table [dbo].[Policy]    Script Date: 10/1/2018 2:58:01 PM ******/
                           IF OBJECT_ID (N'[Policy]', N'U') IS NULL 
                            BEGIN
                            CREATE TABLE [dbo].[Policy](
	                            [Id] [int] IDENTITY(1,1) NOT NULL,
	                            [Name] [nvarchar](50) NOT NULL,
	                            [Description] [nvarchar](100) NULL,
                             CONSTRAINT [PK_Policy] PRIMARY KEY CLUSTERED 
                            (
	                            [Id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]
                            END
                            /****** Object:  Table [dbo].[PolicyPermission]    Script Date: 10/1/2018 2:58:02 PM ******/
                            IF OBJECT_ID (N'[PolicyPermission]', N'U') IS NULL 
                            BEGIN
                                CREATE TABLE [dbo].[PolicyPermission](
	                                [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [PermissionId] [int] NOT NULL,
	                                [PolicyId] [int] NOT NULL,
                                 CONSTRAINT [PK_PolicyPermission] PRIMARY KEY CLUSTERED 
                                (
	                                [Id] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY]
                            END
                            /****** Object:  Table [dbo].[SecurityGroup]    Script Date: 10/1/2018 2:58:02 PM ******/
                            IF OBJECT_ID (N'[SecurityGroup]', N'U') IS NULL 
                            BEGIN
                                CREATE TABLE [dbo].[SecurityGroup](
	                                [Id] [int] IDENTITY(1,1) NOT NULL,
	                                [CreatedAt] [datetimeoffset](7) NULL,
	                                [Name] [nvarchar](50) NOT NULL,
	                                [Description] [nvarchar](100) NULL,
                                 CONSTRAINT [PK_SecurityGroup] PRIMARY KEY CLUSTERED 
                                (
	                                [Id] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY]
                            END
                            /****** Object:  Table [dbo].[SecurityGroupPolicy]    Script Date: 10/1/2018 2:58:02 PM ******/
                             IF OBJECT_ID (N'[SecurityGroupPolicy]', N'U') IS NULL 
                            BEGIN
                            CREATE TABLE [dbo].[SecurityGroupPolicy](
	                            [Id] [int] IDENTITY(1,1) NOT NULL,
	                            [SecurityGroupId] [int] NOT NULL,
	                            [PolicyId] [int] NOT NULL,
                             CONSTRAINT [PK_SecurityGroupPolicy] PRIMARY KEY CLUSTERED 
                            (
	                            [Id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]
                            END
                            /****** Object:  Table [dbo].[SecurityGroupUser]    Script Date: 10/1/2018 2:58:03 PM ******/
                             IF OBJECT_ID (N'[SecurityGroupUser]', N'U') IS NULL 
                            BEGIN
                            CREATE TABLE [dbo].[SecurityGroupUser](
	                            [Id] [int] IDENTITY(1,1) NOT NULL,
	                            [UserId] [int] NOT NULL,
	                            [SecurityGroupId] [int] NOT NULL,
                             CONSTRAINT [PK_SecurityGroupUser] PRIMARY KEY CLUSTERED 
                            (
	                            [Id] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]
                            END

                            ALTER TABLE [dbo].[PolicyPermission]  WITH CHECK ADD  CONSTRAINT [FK_PolicyPermission_Permission_PermissionId] FOREIGN KEY([PermissionId])
                            REFERENCES [dbo].[Permission] ([Id])
                            ON DELETE CASCADE
                            
                            ALTER TABLE [dbo].[PolicyPermission] CHECK CONSTRAINT [FK_PolicyPermission_Permission_PermissionId]
                            
                            ALTER TABLE [dbo].[PolicyPermission]  WITH CHECK ADD  CONSTRAINT [FK_PolicyPermission_Policy_PolicyId] FOREIGN KEY([PolicyId])
                            REFERENCES [dbo].[Policy] ([Id])
                            ON DELETE CASCADE
                           
                            ALTER TABLE [dbo].[PolicyPermission] CHECK CONSTRAINT [FK_PolicyPermission_Policy_PolicyId]
                           
                            ALTER TABLE [dbo].[SecurityGroupPolicy]  WITH CHECK ADD  CONSTRAINT [FK_SecurityGroupPolicy_Policy_PolicyId] FOREIGN KEY([PolicyId])
                            REFERENCES [dbo].[Policy] ([Id])
                            ON DELETE CASCADE
                           
                            ALTER TABLE [dbo].[SecurityGroupPolicy] CHECK CONSTRAINT [FK_SecurityGroupPolicy_Policy_PolicyId]
                            
                            ALTER TABLE [dbo].[SecurityGroupPolicy]  WITH CHECK ADD  CONSTRAINT [FK_SecurityGroupPolicy_SecurityGroup_SecurityGroupId] FOREIGN KEY([SecurityGroupId])
                            REFERENCES [dbo].[SecurityGroup] ([Id])
                            ON DELETE CASCADE
                           
                            ALTER TABLE [dbo].[SecurityGroupPolicy] CHECK CONSTRAINT [FK_SecurityGroupPolicy_SecurityGroup_SecurityGroupId]
                           
                            ALTER TABLE [dbo].[SecurityGroupUser]  WITH CHECK ADD  CONSTRAINT [FK_SecurityGroupUser_SecurityGroup_SecurityGroupId] FOREIGN KEY([SecurityGroupId])
                            REFERENCES [dbo].[SecurityGroup] ([Id])
                            ON DELETE CASCADE
                            
                            ALTER TABLE [dbo].[SecurityGroupUser] CHECK CONSTRAINT [FK_SecurityGroupUser_SecurityGroup_SecurityGroupId]";

            //ALTER TABLE [dbo].[SecurityGroupUser]  WITH CHECK ADD  CONSTRAINT [FK_SecurityGroupUser_User_UserId] FOREIGN KEY([UserId])
            //REFERENCES [dbo].[User] ([Id])
            //ON DELETE CASCADE

            // ALTER TABLE[dbo].[SecurityGroupUser]
            // CHECK CONSTRAINT[FK_SecurityGroupUser_User_UserId]

            using var con = new SqlConnection(connectionString);
            con.Open();

            con.Execute(query);
        }
    }
}