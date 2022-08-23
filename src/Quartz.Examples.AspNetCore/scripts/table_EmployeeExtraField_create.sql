USE [CRCTest]
GO

/****** Object:  Table [dbo].[EmployeeExtraField]    Script Date: 23.8.2022 14:04:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmployeeExtraField](
	[EmployeeID] [int] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_EmployeeExtraField] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC,
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[EmployeeExtraField]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeExtraField_Employee] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[EmployeeExtraField] CHECK CONSTRAINT [FK_EmployeeExtraField_Employee]
GO

