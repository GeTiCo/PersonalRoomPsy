USE [personalRoomPsy]
GO
/****** Object:  Table [dbo].[city]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[city](
	[idCity] [int] IDENTITY(1,1) NOT NULL,
	[idFromCountry] [int] NOT NULL,
	[nameCity] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_city] PRIMARY KEY CLUSTERED 
(
	[idCity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[contacts]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contacts](
	[idContact] [int] IDENTITY(1,1) NOT NULL,
	[idUserContact] [int] NOT NULL,
	[nameContact] [nvarchar](150) NOT NULL,
	[phoneContact] [varchar](11) NULL,
	[emailContact] [varchar](50) NULL,
	[informationContact] [nvarchar](250) NULL,
 CONSTRAINT [PK_contacts] PRIMARY KEY CLUSTERED 
(
	[idContact] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[country]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[country](
	[idCountry] [int] IDENTITY(1,1) NOT NULL,
	[nameCountry] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_country] PRIMARY KEY CLUSTERED 
(
	[idCountry] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[historyOrder]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[historyOrder](
	[idOldOrder] [int] NOT NULL,
	[idWorker] [int] NOT NULL,
	[FioClient] [nvarchar](150) NOT NULL,
	[dateCompete] [date] NOT NULL,
	[commentClient] [nvarchar](250) NULL,
	[rateWorker] [int] NULL,
	[statusOrder] [int] NOT NULL,
 CONSTRAINT [PK_historyOrder] PRIMARY KEY CLUSTERED 
(
	[idOldOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lineProblem]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lineProblem](
	[idLineProblem] [int] NOT NULL,
	[nameLineProblem] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_lineProblem] PRIMARY KEY CLUSTERED 
(
	[idLineProblem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[messageProblem]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[messageProblem](
	[idProblem] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[commentProblem] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_messageProblem] PRIMARY KEY CLUSTERED 
(
	[idProblem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order](
	[idOrder] [int] IDENTITY(1,1) NOT NULL,
	[FioClient] [nvarchar](150) NOT NULL,
	[dateMaking] [date] NOT NULL,
	[dateStart] [date] NOT NULL,
	[timeStart] [time](7) NOT NULL,
	[idProblemOrder] [int] NOT NULL,
	[idLineProblemOrder] [int] NOT NULL,
	[idWorker] [int] NOT NULL,
	[statusOrder] [int] NOT NULL,
	[infoFromUserOrder] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_order] PRIMARY KEY CLUSTERED 
(
	[idOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role](
	[idRole] [int] NOT NULL,
	[nameRole] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED 
(
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[statusOrder]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[statusOrder](
	[idStatusOrder] [int] NOT NULL,
	[nameStatusOrder] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_statusOrder] PRIMARY KEY CLUSTERED 
(
	[idStatusOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[typeProblem]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[typeProblem](
	[idTypeProblem] [int] NOT NULL,
	[idLineProblem] [int] NOT NULL,
	[nameTypeProblem] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_typeProblem] PRIMARY KEY CLUSTERED 
(
	[idTypeProblem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[idUser] [int] IDENTITY(1,1) NOT NULL,
	[loginUser] [varchar](50) NOT NULL,
	[passwordUser] [varchar](50) NOT NULL,
	[fullNameUser] [nvarchar](150) NOT NULL,
	[dateBirth] [date] NOT NULL,
	[emailUser] [varchar](50) NOT NULL,
	[phoneUser] [varchar](25) NOT NULL,
	[idRoleUser] [int] NOT NULL,
	[countryUser] [int] NOT NULL,
	[cityUser] [int] NOT NULL,
	[sexUser] [nvarchar](10) NOT NULL,
	[urlPhoto] [nvarchar](500) NOT NULL,
	[verificationUser] [int] NULL,
	[lineWorking] [int] NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[verification]    Script Date: 25.06.2024 15:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[verification](
	[idVerification] [int] NOT NULL,
	[nameVerification] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_verification] PRIMARY KEY CLUSTERED 
(
	[idVerification] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[city]  WITH CHECK ADD  CONSTRAINT [FK_city_country] FOREIGN KEY([idFromCountry])
REFERENCES [dbo].[country] ([idCountry])
GO
ALTER TABLE [dbo].[city] CHECK CONSTRAINT [FK_city_country]
GO
ALTER TABLE [dbo].[contacts]  WITH CHECK ADD  CONSTRAINT [FK_contacts_user] FOREIGN KEY([idUserContact])
REFERENCES [dbo].[user] ([idUser])
GO
ALTER TABLE [dbo].[contacts] CHECK CONSTRAINT [FK_contacts_user]
GO
ALTER TABLE [dbo].[historyOrder]  WITH CHECK ADD  CONSTRAINT [FK_historyOrder_statusOrder] FOREIGN KEY([statusOrder])
REFERENCES [dbo].[statusOrder] ([idStatusOrder])
GO
ALTER TABLE [dbo].[historyOrder] CHECK CONSTRAINT [FK_historyOrder_statusOrder]
GO
ALTER TABLE [dbo].[historyOrder]  WITH CHECK ADD  CONSTRAINT [FK_historyOrder_user] FOREIGN KEY([idWorker])
REFERENCES [dbo].[user] ([idUser])
GO
ALTER TABLE [dbo].[historyOrder] CHECK CONSTRAINT [FK_historyOrder_user]
GO
ALTER TABLE [dbo].[messageProblem]  WITH CHECK ADD  CONSTRAINT [FK_messageProblem_user] FOREIGN KEY([idUser])
REFERENCES [dbo].[user] ([idUser])
GO
ALTER TABLE [dbo].[messageProblem] CHECK CONSTRAINT [FK_messageProblem_user]
GO
ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_lineProblem] FOREIGN KEY([idLineProblemOrder])
REFERENCES [dbo].[lineProblem] ([idLineProblem])
GO
ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_lineProblem]
GO
ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_statusOrder] FOREIGN KEY([statusOrder])
REFERENCES [dbo].[statusOrder] ([idStatusOrder])
GO
ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_statusOrder]
GO
ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_typeProblem] FOREIGN KEY([idProblemOrder])
REFERENCES [dbo].[typeProblem] ([idTypeProblem])
GO
ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_typeProblem]
GO
ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_user] FOREIGN KEY([idWorker])
REFERENCES [dbo].[user] ([idUser])
GO
ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_user]
GO
ALTER TABLE [dbo].[typeProblem]  WITH CHECK ADD  CONSTRAINT [FK_typeProblem_lineProblem] FOREIGN KEY([idLineProblem])
REFERENCES [dbo].[lineProblem] ([idLineProblem])
GO
ALTER TABLE [dbo].[typeProblem] CHECK CONSTRAINT [FK_typeProblem_lineProblem]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_city] FOREIGN KEY([cityUser])
REFERENCES [dbo].[city] ([idCity])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_city]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_country] FOREIGN KEY([countryUser])
REFERENCES [dbo].[country] ([idCountry])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_country]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_lineProblem] FOREIGN KEY([lineWorking])
REFERENCES [dbo].[lineProblem] ([idLineProblem])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_lineProblem]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_role] FOREIGN KEY([idRoleUser])
REFERENCES [dbo].[role] ([idRole])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_role]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_verification] FOREIGN KEY([verificationUser])
REFERENCES [dbo].[verification] ([idVerification])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_verification]
GO
