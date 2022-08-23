USE [CRCTest]
GO

/****** Object:  Table [dbo].[EmployeePresence]    Script Date: 23.8.2022 14:05:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmployeePresence](
	[ResourceID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[DeviceID] [int] NOT NULL,
	[LastRegistrationDateTime] [datetime] NULL,
 CONSTRAINT [PK_EmployeePresence] PRIMARY KEY CLUSTERED 
(
	[ResourceID] ASC,
	[SiteID] ASC,
	[DeviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeePresence]  WITH CHECK ADD  CONSTRAINT [FK_EmployeePresence_Employee] FOREIGN KEY([ResourceID])
REFERENCES [dbo].[Employee] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EmployeePresence] CHECK CONSTRAINT [FK_EmployeePresence_Employee]
GO

