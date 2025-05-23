USE [TechnomediaTestTaskDB]
GO
ALTER TABLE [dbo].[requests] DROP CONSTRAINT [CHK_Request_Status]
GO
ALTER TABLE [dbo].[work_logs] DROP CONSTRAINT [FK_Work_Logs_Assignments]
GO
ALTER TABLE [dbo].[users] DROP CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[requests] DROP CONSTRAINT [FK_Request_Clients]
GO
ALTER TABLE [dbo].[assignments] DROP CONSTRAINT [FK_Assignments_Teams]
GO
ALTER TABLE [dbo].[assignments] DROP CONSTRAINT [FK_Assignments_Requests]
GO
/****** Object:  Index [UQ__users__F3DBC5720F0E5684]    Script Date: 02.03.2025 21:13:51 ******/
ALTER TABLE [dbo].[users] DROP CONSTRAINT [UQ__users__F3DBC5720F0E5684]
GO
/****** Object:  Index [UQ__roles__72E12F1BA41ED0EA]    Script Date: 02.03.2025 21:13:51 ******/
ALTER TABLE [dbo].[roles] DROP CONSTRAINT [UQ__roles__72E12F1BA41ED0EA]
GO
/****** Object:  Table [dbo].[work_logs]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[work_logs]') AND type in (N'U'))
DROP TABLE [dbo].[work_logs]
GO
/****** Object:  Table [dbo].[users]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[users]') AND type in (N'U'))
DROP TABLE [dbo].[users]
GO
/****** Object:  Table [dbo].[teams]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[teams]') AND type in (N'U'))
DROP TABLE [dbo].[teams]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[roles]') AND type in (N'U'))
DROP TABLE [dbo].[roles]
GO
/****** Object:  Table [dbo].[requests]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[requests]') AND type in (N'U'))
DROP TABLE [dbo].[requests]
GO
/****** Object:  Table [dbo].[clients]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[clients]') AND type in (N'U'))
DROP TABLE [dbo].[clients]
GO
/****** Object:  Table [dbo].[assignments]    Script Date: 02.03.2025 21:13:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[assignments]') AND type in (N'U'))
DROP TABLE [dbo].[assignments]
GO
/****** Object:  Table [dbo].[assignments]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[assignments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[team_id] [int] NOT NULL,
	[request_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[clients]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[clients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[contact_info] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[requests]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[client_id] [int] NOT NULL,
	[create_date] [datetime] NULL,
	[notes] [nvarchar](255) NULL,
	[status] [nvarchar](255) NULL,
	[research_notes] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[teams]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[teams](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[role_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[work_logs]    Script Date: 02.03.2025 21:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[work_logs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[assignment_id] [int] NOT NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[comments] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[assignments] ON 

INSERT [dbo].[assignments] ([id], [team_id], [request_id]) VALUES (1, 1, 1)
INSERT [dbo].[assignments] ([id], [team_id], [request_id]) VALUES (2, 1, 2)
INSERT [dbo].[assignments] ([id], [team_id], [request_id]) VALUES (3, 2, 3)
INSERT [dbo].[assignments] ([id], [team_id], [request_id]) VALUES (4, 2, 4)
SET IDENTITY_INSERT [dbo].[assignments] OFF
GO
SET IDENTITY_INSERT [dbo].[clients] ON 

INSERT [dbo].[clients] ([id], [name], [contact_info]) VALUES (1, N'Client 1', N'contact info')
INSERT [dbo].[clients] ([id], [name], [contact_info]) VALUES (2, N'Client 2', N'contact info')
SET IDENTITY_INSERT [dbo].[clients] OFF
GO
SET IDENTITY_INSERT [dbo].[requests] ON 

INSERT [dbo].[requests] ([id], [client_id], [create_date], [notes], [status], [research_notes]) VALUES (1, 1, CAST(N'2025-03-02T18:41:49.260' AS DateTime), N'zametka', N'Completed', N'research')
INSERT [dbo].[requests] ([id], [client_id], [create_date], [notes], [status], [research_notes]) VALUES (2, 1, CAST(N'2025-03-02T18:42:05.580' AS DateTime), N'zametka', N'Completed', N'research')
INSERT [dbo].[requests] ([id], [client_id], [create_date], [notes], [status], [research_notes]) VALUES (3, 2, CAST(N'2025-03-02T18:42:13.747' AS DateTime), N'zametka', N'Completed', N'research')
INSERT [dbo].[requests] ([id], [client_id], [create_date], [notes], [status], [research_notes]) VALUES (4, 2, CAST(N'2025-03-02T18:42:17.740' AS DateTime), N'zametka', N'Completed', N'research')
SET IDENTITY_INSERT [dbo].[requests] OFF
GO
SET IDENTITY_INSERT [dbo].[roles] ON 

INSERT [dbo].[roles] ([id], [name]) VALUES (4, N'Administrator')
INSERT [dbo].[roles] ([id], [name]) VALUES (2, N'Director')
INSERT [dbo].[roles] ([id], [name]) VALUES (1, N'Secretary')
INSERT [dbo].[roles] ([id], [name]) VALUES (3, N'Worker')
SET IDENTITY_INSERT [dbo].[roles] OFF
GO
SET IDENTITY_INSERT [dbo].[teams] ON 

INSERT [dbo].[teams] ([id], [name]) VALUES (1, N'Brigada 1')
INSERT [dbo].[teams] ([id], [name]) VALUES (2, N'Brigada 2')
SET IDENTITY_INSERT [dbo].[teams] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([id], [username], [password], [role_id]) VALUES (1, N'Administrator', N'pass', 4)
INSERT [dbo].[users] ([id], [username], [password], [role_id]) VALUES (2, N'Director', N'pass', 2)
INSERT [dbo].[users] ([id], [username], [password], [role_id]) VALUES (3, N'Secretary', N'pass', 1)
INSERT [dbo].[users] ([id], [username], [password], [role_id]) VALUES (4, N'Worker', N'pass', 3)
SET IDENTITY_INSERT [dbo].[users] OFF
GO
SET IDENTITY_INSERT [dbo].[work_logs] ON 

INSERT [dbo].[work_logs] ([id], [assignment_id], [start_time], [end_time], [comments]) VALUES (1, 1, CAST(N'2025-03-03T15:53:14.363' AS DateTime), CAST(N'2025-04-02T15:53:14.363' AS DateTime), N'string')
INSERT [dbo].[work_logs] ([id], [assignment_id], [start_time], [end_time], [comments]) VALUES (2, 2, CAST(N'2025-03-03T15:53:14.363' AS DateTime), CAST(N'2025-03-04T18:53:14.363' AS DateTime), N'string')
INSERT [dbo].[work_logs] ([id], [assignment_id], [start_time], [end_time], [comments]) VALUES (3, 2, CAST(N'2025-03-03T15:53:14.363' AS DateTime), CAST(N'2025-03-03T18:53:14.363' AS DateTime), N'string')
INSERT [dbo].[work_logs] ([id], [assignment_id], [start_time], [end_time], [comments]) VALUES (4, 3, CAST(N'2025-03-03T15:53:14.363' AS DateTime), CAST(N'2025-03-04T18:53:14.363' AS DateTime), N'string')
INSERT [dbo].[work_logs] ([id], [assignment_id], [start_time], [end_time], [comments]) VALUES (5, 4, CAST(N'2025-03-03T15:53:14.363' AS DateTime), CAST(N'2025-03-04T18:53:14.363' AS DateTime), N'string')
SET IDENTITY_INSERT [dbo].[work_logs] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__roles__72E12F1BA41ED0EA]    Script Date: 02.03.2025 21:13:51 ******/
ALTER TABLE [dbo].[roles] ADD UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__users__F3DBC5720F0E5684]    Script Date: 02.03.2025 21:13:51 ******/
ALTER TABLE [dbo].[users] ADD UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[assignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Requests] FOREIGN KEY([request_id])
REFERENCES [dbo].[requests] ([id])
GO
ALTER TABLE [dbo].[assignments] CHECK CONSTRAINT [FK_Assignments_Requests]
GO
ALTER TABLE [dbo].[assignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Teams] FOREIGN KEY([team_id])
REFERENCES [dbo].[teams] ([id])
GO
ALTER TABLE [dbo].[assignments] CHECK CONSTRAINT [FK_Assignments_Teams]
GO
ALTER TABLE [dbo].[requests]  WITH CHECK ADD  CONSTRAINT [FK_Request_Clients] FOREIGN KEY([client_id])
REFERENCES [dbo].[clients] ([id])
GO
ALTER TABLE [dbo].[requests] CHECK CONSTRAINT [FK_Request_Clients]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([role_id])
REFERENCES [dbo].[roles] ([id])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[work_logs]  WITH CHECK ADD  CONSTRAINT [FK_Work_Logs_Assignments] FOREIGN KEY([assignment_id])
REFERENCES [dbo].[assignments] ([id])
GO
ALTER TABLE [dbo].[work_logs] CHECK CONSTRAINT [FK_Work_Logs_Assignments]
GO
ALTER TABLE [dbo].[requests]  WITH CHECK ADD  CONSTRAINT [CHK_Request_Status] CHECK  (([status]='NotStarted' OR [status]='InProgress' OR [status]='Completed'))
GO
ALTER TABLE [dbo].[requests] CHECK CONSTRAINT [CHK_Request_Status]
GO
