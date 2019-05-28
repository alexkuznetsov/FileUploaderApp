USE [authcatalog]
GO
/****** Object:  Table [dbo].[users]    Script Date: 10.04.2019 23:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NULL,
	[username] [nvarchar](256) NOT NULL,
	[passwhash] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_clients] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [created_at]
GO

--- Insert client for authentication
--- rex / 1qaz!QAZ

SET IDENTITY_INSERT [dbo].[users] ON 
GO
INSERT [dbo].[users] ([id], [created_at], [updated_at], [username], [passwhash]) VALUES (1, CAST(N'2019-04-08T18:31:18.7866667' AS DateTime2), NULL, N'rex', N'ACAtzWwMbSeKmzfKNcL5SNAy09YbZbEJ4oelQWY+x/O9ai8czW0Dx79HplyLTKGr0A==')
GO
SET IDENTITY_INSERT [dbo].[users] OFF
GO