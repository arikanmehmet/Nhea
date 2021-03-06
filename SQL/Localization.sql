GO
/****** Object:  Table [dbo].[Language]    Script Date: 13.10.2019 22:39:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Culture] [nvarchar](50) NOT NULL,
	[Status] [smallint] NOT NULL,
	[TwoLetterIsoName] [nvarchar](2) NOT NULL
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Localization]    Script Date: 13.10.2019 22:39:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Localization](
	[Id] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](max) NOT NULL,
	[Translation] [nvarchar](max) NOT NULL,
	[TargetEntityId] [uniqueidentifier] NULL,
	[LanguageId] [int] NOT NULL,
	[TargetEntityName] [nvarchar](150) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Localization] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Language] ON 
GO
INSERT [dbo].[Language] ([Id], [Title], [Culture], [Status], [TwoLetterIsoName]) VALUES (1, N'English', N'en-US', 1, N'en')
GO
GO
SET IDENTITY_INSERT [dbo].[Language] OFF
GO
INSERT [dbo].[Localization] ([Id], [Key], [Translation], [TargetEntityId], [LanguageId], [TargetEntityName], [Status]) VALUES (N'1dcd75ec-cd97-4ace-b1c0-0b6ae5d70f54', N'TestTranslation', N'Translation in English', NULL, 1, NULL, 1)
GO
ALTER TABLE [dbo].[Localization] ADD  CONSTRAINT [DF_Localization_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Localization] ADD  CONSTRAINT [DF_Localization_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Localization]  WITH CHECK ADD  CONSTRAINT [FK_Localization_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
GO
ALTER TABLE [dbo].[Localization] CHECK CONSTRAINT [FK_Localization_Language]
GO
