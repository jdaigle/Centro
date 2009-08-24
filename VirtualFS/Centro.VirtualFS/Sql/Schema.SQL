SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
BEGIN
CREATE TABLE [dbo].[filebinaries](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[binarydata] [image] NOT NULL,
	[size] [int] NOT NULL,
 CONSTRAINT [PK_filebinaries] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

EXEC dbo.sp_executesql N'CREATE DEFAULT [dbo].[df_true]
AS
   1
'
GO

EXEC dbo.sp_executesql N'
CREATE DEFAULT [dbo].[df_false]
AS
   0
'
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
BEGIN
CREATE TABLE [dbo].[AccessControlLists](
	[aclid] [bigint] IDENTITY(1,1) NOT NULL,
	[directoryid] [int] NOT NULL,
	[aclreadall] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_aclreadall]  DEFAULT ((1)),
	[aclchangeall] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_aclchangeall]  DEFAULT ((0)),
	[acladminall] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_acladminall]  DEFAULT ((0)),
	[inherit] [bit] NOT NULL CONSTRAINT [DF_AccessControlLists_inherit]  DEFAULT ((1)),
 CONSTRAINT [PK_AccessControlLists] PRIMARY KEY CLUSTERED 
(
	[aclid] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
BEGIN
CREATE TABLE [dbo].[ACLMembers](
	[aclmemberid] [bigint] IDENTITY(1,1) NOT NULL,
	[aclid] [bigint] NOT NULL,
	[membername] [varchar](100) NOT NULL,
	[aclread] [bit] NOT NULL CONSTRAINT [DF_ACLMembers_aclread]  DEFAULT ((0)),
	[aclchange] [bit] NOT NULL CONSTRAINT [DF_ACLMembers_aclchange]  DEFAULT ((0)),
	[acladmin] [bit] NOT NULL CONSTRAINT [DF_ACLMembers_acladmin]  DEFAULT ((0)),
 CONSTRAINT [PK_ACLMembers] PRIMARY KEY CLUSTERED 
(
	[aclmemberid] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
BEGIN
CREATE TABLE [dbo].[files](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[directory_id] [int] NOT NULL,
	[creationtime] [datetime] NOT NULL,
	[isdeleted] [bit] NOT NULL,
 CONSTRAINT [PK_files] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_bindefault @defname=N'[dbo].[df_false]', @objname=N'[dbo].[files].[isdeleted]' , @futureonly='futureonly'
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
BEGIN
CREATE TABLE [dbo].[fileversions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[file_id] [int] NOT NULL,
	[previous_id] [int] NULL,
	[filebinary_id] [int] NOT NULL,
	[lastwritedate] [datetime] NOT NULL,
	[latestversion] [bit] NOT NULL,
 CONSTRAINT [PK_fileversions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_bindefault @defname=N'[dbo].[df_true]', @objname=N'[dbo].[fileversions].[latestversion]' , @futureonly='futureonly'
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[directories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[directories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[parent_id] [int] NULL,
	[isdeleted] [bit] NOT NULL,
	[isroot] [bit] NOT NULL,
 CONSTRAINT [PK_directories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
EXEC sys.sp_bindefault @defname=N'[dbo].[df_false]', @objname=N'[dbo].[directories].[isdeleted]' , @futureonly='futureonly'
GO
EXEC sys.sp_bindefault @defname=N'[dbo].[df_false]', @objname=N'[dbo].[directories].[isroot]' , @futureonly='futureonly'
GO

ALTER TABLE [dbo].[AccessControlLists]  WITH CHECK ADD  CONSTRAINT [FK_AccessControlLists_Directories] FOREIGN KEY([directoryid])
REFERENCES [dbo].[directories] ([id])
GO
ALTER TABLE [dbo].[AccessControlLists] CHECK CONSTRAINT [FK_AccessControlLists_Directories]
GO

ALTER TABLE [dbo].[ACLMembers]  WITH CHECK ADD  CONSTRAINT [FK_ACLMembers_AccessControlLists] FOREIGN KEY([aclid])
REFERENCES [dbo].[AccessControlLists] ([aclid])
GO
ALTER TABLE [dbo].[ACLMembers] CHECK CONSTRAINT [FK_ACLMembers_AccessControlLists]
GO

ALTER TABLE [dbo].[files]  WITH CHECK ADD  CONSTRAINT [FK_files_directories] FOREIGN KEY([directory_id])
REFERENCES [dbo].[directories] ([id])
GO
ALTER TABLE [dbo].[files] CHECK CONSTRAINT [FK_files_directories]
GO

ALTER TABLE [dbo].[fileversions]  WITH CHECK ADD  CONSTRAINT [FK_fileversions_filebinaries] FOREIGN KEY([filebinary_id])
REFERENCES [dbo].[filebinaries] ([id])
GO
ALTER TABLE [dbo].[fileversions] CHECK CONSTRAINT [FK_fileversions_filebinaries]
GO

ALTER TABLE [dbo].[fileversions]  WITH CHECK ADD  CONSTRAINT [FK_fileversions_files] FOREIGN KEY([file_id])
REFERENCES [dbo].[files] ([id])
GO
ALTER TABLE [dbo].[fileversions] CHECK CONSTRAINT [FK_fileversions_files]
GO

ALTER TABLE [dbo].[fileversions]  WITH CHECK ADD  CONSTRAINT [FK_fileversions_fileversions] FOREIGN KEY([previous_id])
REFERENCES [dbo].[fileversions] ([id])
GO
ALTER TABLE [dbo].[fileversions] CHECK CONSTRAINT [FK_fileversions_fileversions]
GO

ALTER TABLE [dbo].[directories]  WITH CHECK ADD  CONSTRAINT [FK_directories_directories] FOREIGN KEY([parent_id])
REFERENCES [dbo].[directories] ([id])
GO
ALTER TABLE [dbo].[directories] CHECK CONSTRAINT [FK_directories_directories]
GO
